using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;

public struct ObjectSpawnInfo
{
    public ObjectSpawnInfo(string name, int dataID, Vector3 worldPos, EObjectType type)
    {
        Name = name;
        DataID = dataID;
        WorldPos = worldPos;
        ObjectType = type;
    }

    public string Name;
    public int DataID;
    public Vector3 WorldPos;
    public EObjectType ObjectType;
}

public class Stage : MonoBehaviour
{
    [SerializeField]
    List<BaseObject> _spawnObjects = new List<BaseObject>();
    public List<ObjectSpawnInfo> SpawnInfos = new List<ObjectSpawnInfo>();

    public ObjectSpawnInfo ExitSpawnInfo;
    public ObjectSpawnInfo WaypointSpawnInfo;

    public int StageIndex { get; set; }
    public Tilemap TilemapObject;   // 스폰 정보를 저장
    public Tilemap Tilemap;
    public bool IsActive = false;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (TilemapObject != null && Tilemap != null)
            return;

        Tilemap = transform.Find("Tilemap").GetComponent<Tilemap>();
        TilemapObject = transform.Find("Objects").GetComponent<Tilemap>();
    }

    public void SetInfo(int stageIdx)
    {
        StageIndex = stageIdx;
        SaveSpawnInfos();
        IsActive = true;
        UnLoadStage();
    }

    public bool IsPointInStage(Vector3 position)    // 특정 위치가 현재 스테이지 영역 안에 있는지 확인
    {
        Vector3Int pos = Tilemap.layoutGrid.WorldToCell(position);
        TileBase tile = Tilemap.GetTile(pos);

        if (tile == null)
            return false;

        return true;
    }

    public void LoadStage()
    {
        if (IsActive)
            return;

        IsActive = true;
        gameObject.SetActive(true);
        SpawnObjects();
    }

    public void UnLoadStage()
    {
        if (IsActive == false)
            return;

        IsActive = false;
        gameObject.SetActive(false);
        DespawnObjects();
    }

    void SpawnObjects()
    {
        foreach (ObjectSpawnInfo info in SpawnInfos)
        {
            Vector3 worldPos = info.WorldPos;

            // 타일 크기를 고려하여 타일 중심으로 조정
            Vector3 tileOffset = TilemapObject.layoutGrid.cellSize * 0.5f; // 타일 크기의 절반
            worldPos += tileOffset;

            switch (info.ObjectType)
            {
                case EObjectType.Monster:
                    Monster monster;
                    if (info.Name == "Golem")   // 골렘은 임시적으로 name을 지정한다
                        monster = Managers.Object.Spawn<Monster>(worldPos, info.DataID, "Golem");
                    else
                        monster = Managers.Object.Spawn<Monster>(worldPos, info.DataID);
                    _spawnObjects.Add(monster);
                    break;
                case EObjectType.Tile:
                    EffectTile tile = Managers.Object.Spawn<EffectTile>(worldPos, info.DataID);
                    _spawnObjects.Add(tile);
                    break;
            }
        }
    }

    void DespawnObjects()
    {
        foreach (BaseObject obj in _spawnObjects)
        {
            if (obj == null)
                continue;

            switch (obj.ObjectType)
            {
                case EObjectType.Monster:
                    Managers.Object.Despawn(obj as Monster);
                    break;
                case EObjectType.Tile:
                    Managers.Object.Despawn(obj as EffectTile);
                    break;
            }
        }

        _spawnObjects.Clear();
    }

    void SaveSpawnInfos()
    {
        if (TilemapObject != null)
            TilemapObject.gameObject.SetActive(false);

        for (int y = TilemapObject.cellBounds.yMax; y >= TilemapObject.cellBounds.yMin; y--)
        {
            for (int x = TilemapObject.cellBounds.xMin; x <= TilemapObject.cellBounds.xMax; x++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                CustomTile tile = TilemapObject.GetTile(new Vector3Int(x, y, 0)) as CustomTile;

                if (tile == null)
                    continue;

                Vector3 worldPos = Managers.Map.CellToWorld(cellPos);
                ObjectSpawnInfo info = new ObjectSpawnInfo(tile.Name, tile.DataID, worldPos, tile.ObjectType);

                if (tile.ObjectType == EObjectType.Tile)
                    ExitSpawnInfo = info;

                if (tile.ObjectType == EObjectType.Waypoint)
                    WaypointSpawnInfo = info;

                SpawnInfos.Add(info);
            }
        }
    }
}
