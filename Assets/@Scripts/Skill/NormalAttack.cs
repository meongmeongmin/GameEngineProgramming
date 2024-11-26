using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class NormalAttack : SkillBase
{
    float _angle = 90f; // 스킬 각도 (부채꼴 각도)
    float _knockbackDistance = 1.5f; // 넉백 거리
    float _knockbackDuration = 0.3f; // 넉백 시간

    public override void SetInfo(Creature owner, int skillID)
    {
        base.SetInfo(owner, skillID);
    }

    public override bool DoSkill()
    {
        if (base.DoSkill() == false)
            return false;

        Player owner = Owner as Player;
        if (owner == null)
            return false;

        Vector3 ownerDir = Util.DirToVector3(Owner.Dir);
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(Owner.transform.position, Data.Range);

        foreach (Collider2D target in hitTargets)
        {
            Creature creature = target.GetComponent<Creature>();
            if (creature == null || creature == Owner) 
                continue;

            // Owner와 target 사이의 방향 벡터 계산
            Vector3 skillDir = (target.transform.position - Owner.transform.position).normalized;

            // 오너가 바라보는 방향과 타겟 방향의 각도 계산
            float angle = Vector3.Angle(ownerDir, skillDir);

            // 부채꼴 범위 안에 있는지 확인
            if (angle <= _angle / 2)
            {
                Debug.Log($"타격 대상: {target.name}");
                creature.OnDamaged(Owner, this);
                StartCoroutine(CoKnockback(creature, skillDir, _knockbackDistance, _knockbackDuration));
            }
        }

        return true;
    }

    IEnumerator CoKnockback(BaseObject target, Vector2 dir, float distance, float duration)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null)
            yield break;

        Vector3 startPosition = target.transform.position;
        Vector3 targetPosition = startPosition + (Vector3)(dir.normalized * distance);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Vector3 nextPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            rb.MovePosition(nextPosition);
            yield return null;
        }
    }
}
