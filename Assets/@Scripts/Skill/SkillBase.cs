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

    bool _IsCooldownComplete;

    public virtual void SetInfo(Creature owner, int skillID)
    {
        Owner = owner;
        Data = Managers.Data.SkillDataDic[skillID];

        CoolTime = Data.CoolTime;
        StaggerTime = Data.StaggerTime;

        _IsCooldownComplete = true;
    }

    public virtual bool DoSkill()
    {
        if (IsSkillUsable())
        {
            Debug.Log("DoSkill");
            Owner.State = ECreatureState.Skill;
            StartCoroutine(CoCountdownCooldown());
            StartCoroutine(Owner.CoUpdateSkill());
            return true;
        }

        return false;
    }

    public bool IsSkillUsable()
    {
        // 쿨타임 검사
        if (_IsCooldownComplete)
            return true;

        return false;
    }

    IEnumerator CoCountdownCooldown()
    {
        _IsCooldownComplete = false;
        yield return new WaitForSeconds(CoolTime);
        _IsCooldownComplete = true;
    }

    public IEnumerator CoKnockback(BaseObject target, Vector2 direction, float distance, float duration)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null)
            yield break;

        Vector3 startPosition = target.transform.position;
        Vector3 targetPosition = startPosition + (Vector3)(direction * distance);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (rb == null)
                yield break;

            elapsedTime += Time.deltaTime;
            Vector3 nextPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            rb.MovePosition(nextPosition);

            yield return null;
        }
    }

    protected virtual void GenerateProjectile(Creature owner, Vector3 spawnPos, Vector3 dir, string name = null)
    {
        Projectile projectile = Managers.Object.Spawn<Projectile>(spawnPos, Data.ProjectileID, name);
        projectile.SetSpawnInfo(Owner, this, dir);
    }
}
