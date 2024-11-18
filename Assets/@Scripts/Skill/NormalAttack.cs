using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using static Define;

public class NormalAttack : SkillBase
{
    float _angle = 90f; // 스킬 각도 (부채꼴 각도)

    public override void SetInfo(Creature owner, int skillID)
    {
        base.SetInfo(owner, skillID);
    }

    public override void DoSkill()
    {
        base.DoSkill();

        Player owner = Owner as Player;
        if (owner == null)
            return;

        Vector3 dir = Util.DirToVector3(Owner.Dir);
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(Owner.transform.position, Data.Range);

        foreach (Collider2D target in hitTargets)
        {
            Creature creature = target.GetComponent<Creature>();
            if (creature == null || creature == Owner) 
                continue;

            // Owner와 target 사이의 방향 벡터 계산
            Vector3 toTarget = (target.transform.position - Owner.transform.position).normalized;

            // 방향과 타겟의 각도 계산
            float angle = Vector3.Angle(dir, toTarget);

            // 부채꼴 범위 안에 있는지 확인
            if (angle <= _angle / 2)
            {
                Debug.Log($"타격 대상: {target.name}");
                creature.OnDamaged(Owner, this);
            }
        }
    }
}
