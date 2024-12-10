using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Boss : Monster
{
    bool _IsNextPage = false;

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
            State = ECreatureState.Idle;
            return;
        }

        float dist = Vector2.Distance(transform.position, Target.transform.position);

        if (dist < _searchScope)
        {
            State = ECreatureState.Skill;
        }
        else
            State = ECreatureState.Idle;
    }

    public override void OnDamaged(Creature owner, SkillBase skill)
    {
        base.OnDamaged(owner, skill);
        if (_IsNextPage == false && Hp <= MaxHp * 0.5)
            _IsNextPage = true;
    }

    public override void OnDead()
    {
        base.OnDead();
        Managers.Sound.Play(ESound.Effect, "ME_Clear");
        Managers.Game.UI_GameScene.GameClear();
        Managers.Object.Despawn(Target);
    }

    protected override void UpdateIdleAnimation()
    {
        _animator.Play("Idle");
    }

    protected override void UpdateSkillAnimation()
    {
        if (_IsNextPage)
            _animator.Play("Skill2");
        else
            _animator.Play("Skill");
    }

    public void Attack()
    {
        Skills.DefaultSkill.DoSkill();
    }
}
