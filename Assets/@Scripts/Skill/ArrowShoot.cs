using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ArrowShoot : SkillBase
{
    public override void SetInfo(Creature owner, int skillID)
    {
        base.SetInfo(owner, skillID);
    }

    public override bool DoSkill()
    {
        if (base.DoSkill() == false)
            return false;

        GenerateProjectile(Owner, Owner.CenterPosition, Util.DirToVector3(Owner.Dir), Data.PrefabLabel);
        Managers.Sound.Play(ESound.Effect, "se_shoot");
        return true;
    }
}
