using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Define;

public class ObjectManager
{
    public Player Player { get; private set; }
    public HashSet<Monster> Monsters { get; } = new HashSet<Monster>();
    public HashSet<EffectTile> Tiles { get; } = new HashSet<EffectTile>();
    public HashSet<Projectile> Projectiles { get; } = new HashSet<Projectile>();

    #region Roots
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }

    public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
    #endregion

    public T Spawn<T>(Vector3Int cellPos, int dataID) where T : BaseObject
    {
        Vector3 position = Managers.Map.CellToWorld(cellPos);
        return Spawn<T>(position, dataID);
    }

    public T Spawn<T>(Vector3 position, int dataID, string name = null) where T : BaseObject
    {
        string prefabName = string.IsNullOrWhiteSpace(name) ? typeof(T).Name : name;

        GameObject go = Managers.Resource.Instantiate(prefabName);
        go.name = prefabName;
        go.transform.position = position;

        BaseObject obj = go.GetComponent<BaseObject>();

        if (obj.ObjectType == EObjectType.Player)
        {
            Player player = go.GetComponent<Player>();
            Player = player;
            player.SetInfo(dataID);
        }
        else if (obj.ObjectType == EObjectType.Monster)
        {
            obj.transform.parent = MonsterRoot;
            Monster monster = go.GetComponent<Monster>();
            Monsters.Add(monster);
            monster.SetInfo(dataID);
        }
        else if (obj.ObjectType == EObjectType.Tile)
        {
            EffectTile tile = go.GetComponent<EffectTile>();
            Tiles.Add(tile);
            tile.SetInfo(dataID);
        }
        else if (obj.ObjectType == EObjectType.Projectile)
        {
            Projectile projectile = go.GetComponent<Projectile>();
            Projectiles.Add(projectile);
            projectile.SetInfo(dataID);
        }

        return obj as T;
    }

    public void Despawn<T>(T obj) where T : BaseObject
    {
        EObjectType objectType = obj.ObjectType;

        if (obj.ObjectType == EObjectType.Monster)
        {
            Monster monster = obj.GetComponent<Monster>();
            Monsters.Remove(monster);
        }
        else if (obj.ObjectType == EObjectType.Tile)
        {
            EffectTile tile = obj.GetComponent<EffectTile>();
            Tiles.Remove(tile);
        }
        else if (obj.ObjectType == EObjectType.Projectile)
        {
            Projectile projectile = obj.GetComponent<Projectile>();
            Projectiles.Remove(projectile);
        }

        Managers.Resource.Destroy(obj.gameObject);
    }
}