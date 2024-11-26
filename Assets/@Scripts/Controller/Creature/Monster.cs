using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    public override bool Init()
    {
        base.Init();

        ObjectType = EObjectType.Monster;
        return true;
    }

    public override void SetInfo(int dataID)
    {
        base.SetInfo(dataID);
    }

    public override void OnDead()
    {
        base.OnDead();
        Managers.Object.Despawn(this);
    }
}
