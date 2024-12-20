using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Creature : BaseObject
{
    protected Animator _animator;

    public Data.CreatureData CreatureData { get; protected set; }
    public SkillComponent Skills { get; protected set; }
    public SkillBase CurrentSkill { get; protected set; }

    public BaseObject Target { get; protected set; }

    public int Level { get; protected set; }
    public float Exp { get; protected set; }

    #region Stat
    public float MaxHp { get; protected set; }
    public float Hp { get; protected set; }
    public float Atk { get; protected set; }
    public float Def { get; protected set; }
    public float MoveSpeed { get; protected set; }
    #endregion

    [SerializeField]
    ECreatureState _state = ECreatureState.Idle;
    
    public ECreatureState State
    {
        get { return _state; }
        set
        {
            _state = value;
            UpdateAnimation();
        }
    }

    protected EDir _dir = EDir.Down;
    public EDir Dir
    {
        get { return _dir; }
        protected set
        {
            if (_dir != value)
            {
                _dir = value;
                UpdateAnimation();
            }
        }
    }

    Vector2 _axis;  // 입력한 방향키
    public Vector2 Axis
    {
        get { return _axis; }
        set
        {
            if (_axis != value)
                _axis = value;
        }
    }

    protected float _angle = -90.0f;

    public override bool Init()
    {
        base.Init();

        _animator = GetComponent<Animator>();
        State = ECreatureState.Idle;

        return true;
    }

    public virtual void SetInfo(int dataID)
    {
        DataID = dataID;

        if (ObjectType == EObjectType.Player)
            CreatureData = Managers.Data.PlayerDataDic[dataID];
        else if (ObjectType == EObjectType.Monster)
            CreatureData = Managers.Data.MonsterDataDic[dataID];

        gameObject.name = CreatureData.PrefabLabel;

        // Stat
        MaxHp = CreatureData.MaxHp;
        Hp = CreatureData.MaxHp;
        Atk = CreatureData.Atk;
        Def = CreatureData.Def;
        MoveSpeed = CreatureData.MoveSpeed;

        // Skill
        Skills = gameObject.GetComponent<SkillComponent>();
        Skills.SetInfo(this);

        State = ECreatureState.Idle;
    }

    public override void OnHeal(float healAmount)
    {
        base.OnHeal(healAmount);
        Hp = Mathf.Min(Hp + healAmount, MaxHp);
        Debug.Log($"힐링! 현재 Hp: {Hp}");
    }

    public override void OnDamaged(Creature owner, SkillBase skill)
    {
        base.OnDamaged(owner, skill);
        Managers.Sound.Play(ESound.Effect, "se_damage");

        float damage;
        if (skill == null)
            damage = Mathf.Clamp(owner.Atk - Def, 1, Hp);
        else
            damage = Mathf.Clamp((owner.Atk * skill.Data.DamageMultiplier) - Def, 1, Hp);

        Hp -= damage;

        State = ECreatureState.OnDamaged;
        StartCoroutine(CoUpdateOnDamaged(skill));

        Debug.Log($"스킬에 맞은 {gameObject.name}의 HP가 {Hp}로 됐다.");
    }

    public override void OnDead()
    {
        base.OnDead();
        Collider.enabled = false;
        StartCoroutine(CoUpdateDead());
    }

    // TODO: 아래의 코루틴 함수를 State 패턴으로 변환
    IEnumerator CoUpdateOnDamaged(SkillBase skill)
    {
        if (skill == null)
        {
            SpriteRenderer.enabled = true;

            if (ObjectType == EObjectType.Monster && Hp > 0)
                State = ECreatureState.Idle;
            else if (Hp <= 0)
            {
                State = ECreatureState.Dead;
                OnDead();
            }

            yield break;
        }

        // 스킬 움찔 시간만큼 스프라이트가 계속 깜빡인다
        float elapsedTime = 0f;
        while (elapsedTime < skill.StaggerTime)
        {
            elapsedTime += Time.deltaTime;
            SpriteRenderer.enabled = !SpriteRenderer.enabled;
            yield return null;
        }

        SpriteRenderer.enabled = true;

        if (ObjectType == EObjectType.Monster && Hp > 0)
            State = ECreatureState.Idle;
        else if (Hp <= 0)
        {
            State = ECreatureState.Dead;
            OnDead();
        }
    }

    public IEnumerator CoUpdateSkill()
    {
        if (ObjectType != EObjectType.Player)
            yield break;

        yield return new WaitForSeconds(0.5f);
        
        if (State != ECreatureState.Dead)
            State = ECreatureState.Idle;

        CurrentSkill = null;    // 스킬 중복 사용 방지
    }

    IEnumerator CoUpdateDead()
    {
        RigidBody.gravityScale = 1;
        RigidBody.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        RigidBody.velocity = Vector2.one;

        yield return new WaitForSeconds(0.5f);
        Managers.Object.Despawn(this);
    }

    #region 애니메이션
    protected void UpdateAnimation()
    {
        switch (State)
        {
            case ECreatureState.Idle:
                UpdateIdleAnimation();
                break;
            case ECreatureState.Move:
                UpdateMoveAnimation();
                break;
            case ECreatureState.Skill:
                UpdateSkillAnimation();
                break;
            case ECreatureState.OnDamaged:
                break;
            case ECreatureState.Dead:
                UpdateDeadAnimation();
                break;
        }
    }

    protected virtual void UpdateIdleAnimation()
    {
        switch (Dir)
        {
            case EDir.Up:
                _animator.Play("Idle_Up");
                break;
            case EDir.Left:
                _animator.Play("Idle_Left");
                break;
            case EDir.Down:
                _animator.Play("Idle_Down");
                break;
            case EDir.Right:
                _animator.Play("Idle_Right");
                break;
        }
    }

    protected virtual void UpdateMoveAnimation()
    {
        switch (Dir)
        {
            case EDir.Up:
                _animator.Play("Walk_Up");
                break;
            case EDir.Left:
                _animator.Play("Walk_Left");
                break;
            case EDir.Down:
                _animator.Play("Walk_Down");
                break;
            case EDir.Right:
                _animator.Play("Walk_Right");
                break;
        }
    }

    protected virtual void UpdateSkillAnimation()
    {
        switch (Dir)
        {
            case EDir.Up:
                _animator.Play("Attak_Up");
                break;
            case EDir.Left:
                _animator.Play("Attak_Left");
                break;
            case EDir.Down:
                _animator.Play("Attak_Down");
                break;
            case EDir.Right:
                _animator.Play("Attak_Right");
                break;
        }
    }

    protected virtual void UpdateDeadAnimation()
    {
        _animator.Play("Dead");
    }
    #endregion
}
