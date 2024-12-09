using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Boss : Monster
{
    public override bool Init()
    {
        base.Init();
        return true;
    }

    void Update()
    {
        if (State == ECreatureState.Dead || State == ECreatureState.OnDamaged)
            return;

        Target = Managers.Object.Player;
        if (Target == null)
        {
            Debug.Log("플레이어를 찾지 못했다");
            State = ECreatureState.Idle;
            return;
        }

        float dist = Vector2.Distance(transform.position, Target.transform.position);

        if (dist < _searchScope)
            State = ECreatureState.Skill;
        else
            State = ECreatureState.Idle;
    }

    public override void OnDead()
    {
        base.OnDead();

    }

    protected override void UpdateIdleAnimation()
    {
        _animator.Play("Idle");
    }

    protected override void UpdateSkillAnimation()
    {
        _animator.Play("Jump");
    }

    public void Attack()
    {
        Skills.DefaultSkill.DoSkill();
    }
}