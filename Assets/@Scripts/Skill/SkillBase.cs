using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillBase : MonoBehaviour
{
    public Creature Owner { get; protected set; }
    public Data.SkillData Data { get; private set; }
    public float RemainCoolTime { get; protected set; }
    
    bool _isSkillUsable;

    public virtual void SetInfo(Creature owner, int skillID)
    {
        Owner = owner;
        Data = Managers.Data.SkillDataDic[skillID];
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
        RemainCoolTime = Data.CoolTime;
        _isSkillUsable = false;
        yield return new WaitForSeconds(Data.CoolTime);
        
        RemainCoolTime = 0;
        _isSkillUsable = true;
    }

    protected virtual void GenerateProjectile(Creature owner, Vector3 spawnPos, string name = null)
    {
        Projectile projectile = Managers.Object.Spawn<Projectile>(spawnPos, Data.ProjectileID, name);
        projectile.SetSpawnInfo(Owner, this);
    }
}
