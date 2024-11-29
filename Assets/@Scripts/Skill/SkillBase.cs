using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillBase : MonoBehaviour
{
    public Creature Owner { get; protected set; }
    public Data.SkillData Data { get; private set; }
    public float CoolTime { get; protected set; }
    public float StaggerTime { get; protected set; }

    bool _isSkillUsable;

    public virtual void SetInfo(Creature owner, int skillID)
    {
        Owner = owner;
        Data = Managers.Data.SkillDataDic[skillID];

        CoolTime = Data.CoolTime;
        StaggerTime = Data.StaggerTime;
        
        _isSkillUsable = true;
    }

    public virtual bool DoSkill()
    {
        if (_isSkillUsable)
        {
            Owner.State = ECreatureState.Skill;
            StartCoroutine(CoCountdownCooldown());
            return true;
        }

        return false;
    }

    IEnumerator CoCountdownCooldown()
    {
        _isSkillUsable = false;
        yield return new WaitForSeconds(CoolTime);
        _isSkillUsable = true;
    }

    protected virtual void GenerateProjectile(Creature owner, Vector3 spawnPos, Vector3 dir, string name = null)
    {
        Projectile projectile = Managers.Object.Spawn<Projectile>(spawnPos, Data.ProjectileID, name);
        projectile.SetSpawnInfo(Owner, this, dir);
    }
}
