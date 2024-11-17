using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Define;

public class ObjectManager
{
    public HashSet<Player> Player { get; } = new HashSet<Player>();
    public HashSet<Projectile> Projectiles { get; } = new HashSet<Projectile>();
    // TODO: 몬스터

    public T Spawn<T>(Vector3Int cellPos, int dataID) where T : BaseObject
    {
        Vector3 position = Managers.Map.CellToWorld(cellPos);
        return Spawn<T>(position, dataID);
    }

    public T Spawn<T>(Vector3 position, int dataID) where T : BaseObject
    {
        string prefabName = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(prefabName);
        go.name = prefabName;
        go.transform.position = position;

        BaseObject obj = go.GetComponent<BaseObject>();

        if (obj.ObjectType == EObjectType.Player)
        {
            Player player = go.GetComponent<Player>();
            Player.Add(player);
            player.SetInfo(dataID);
        }
        else if (obj.ObjectType == EObjectType.Monster)
        {
            // TODO: 몬스터
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

        if (obj.ObjectType == EObjectType.Player)
        {
            Player player = obj.GetComponent<Player>();
            Player.Remove(player);
        }
        else if (obj.ObjectType == EObjectType.Monster)
        {
            // TODO: 몬스터
        }
        else if (obj.ObjectType == EObjectType.Projectile)
        {
            Projectile projectile = obj.GetComponent<Projectile>();
            Projectiles.Remove(projectile);
        }

        Managers.Resource.Destroy(obj.gameObject);
    }
}
