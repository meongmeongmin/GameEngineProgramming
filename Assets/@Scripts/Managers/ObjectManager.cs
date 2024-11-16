using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ObjectManager
{
    public HashSet<Player> Player { get; } = new HashSet<Player>();
    // TODO: 몬스터

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

        Managers.Resource.Destroy(obj.gameObject);
    }
}
