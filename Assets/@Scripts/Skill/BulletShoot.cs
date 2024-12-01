using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BulletShoot : SkillBase
{
    public override void SetInfo(Creature owner, int skillID)
    {
        base.SetInfo(owner, skillID);
    }

    public override bool DoSkill()
    {
        if (base.DoSkill() == false)
            return false;

        Vector3 skillDir = (Owner.Target.transform.position - Owner.transform.position).normalized;
        GenerateProjectile(Owner, Owner.CenterPosition, skillDir, Data.PrefabLabel);
        return true;
    }
}
