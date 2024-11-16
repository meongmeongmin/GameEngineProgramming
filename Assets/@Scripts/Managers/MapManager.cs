using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    public GameObject Map { get; private set; }
    public string MapName { get; private set; }
    public Grid CellGrid { get; private set; }

    public Vector3Int WorldToCell(Vector3 worldPos) { return CellGrid.WorldToCell(worldPos); }
    public Vector3 CellToWorld(Vector3Int cellPos) { return CellGrid.CellToWorld(cellPos); }

    public void LoadMap(string mapName)
    {
        DestroyMap();

        GameObject map = Managers.Resource.Instantiate(mapName);
        map.transform.position = Vector3.zero;
        map.name = $"@Map_{mapName}";

        Map = map;
        MapName = mapName;
        CellGrid = map.GetComponent<Grid>();
    }

    public void DestroyMap()
    {
        if (Map != null)
            Managers.Resource.Destroy(Map);
    }
}
