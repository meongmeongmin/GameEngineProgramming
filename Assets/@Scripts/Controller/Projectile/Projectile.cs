using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Projectile : BaseObject
{
    public Creature Owner { get; private set; }
    public SkillBase Skill { get; private set; }
    public Data.ProjectileData Data { get; private set; }
    public ProjectileMotionBase ProjectileMotion { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false) 
            return false;

        ObjectType = EObjectType.Projectile;
        return true;
    }

    public void SetInfo(int dataID)
    {
        Data = Managers.Data.ProjectileDataDic[dataID];
    }

    public void SetSpawnInfo(Creature owner, SkillBase skill, Vector3 dir)
    {
        Owner = owner;
        Skill = skill;

        if (ProjectileMotion != null)
            Destroy(ProjectileMotion);

        string componentName = Data.MotionComponentName;
        ProjectileMotion = gameObject.AddComponent(Type.GetType(componentName)) as ProjectileMotionBase;

        StraightMotion straightMotion = ProjectileMotion as StraightMotion;
        if (straightMotion != null)
            straightMotion.SetInfo(Data.DataID, owner.CenterPosition, dir, () => { Managers.Object.Despawn(this); });

        StartCoroutine(CoReserveDestroy(Data.Duration));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BaseObject target = other.GetComponent<BaseObject>();
        if (target == Owner)
            return;

        if (target != null)
        {
            Debug.Log($"타격 대상: {target.name}");
            target.OnDamaged(Owner, Skill);
        }

        Managers.Object.Despawn(this);
    }

    IEnumerator CoReserveDestroy(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Managers.Object.Despawn(this);
    }
}
