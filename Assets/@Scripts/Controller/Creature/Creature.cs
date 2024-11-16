using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Creature : BaseObject
{
    protected Animator _animator;

    public Data.CreatureData CreatureData { get; protected set; }

    public int Level { get; protected set; }
    public float Exp { get; protected set; }

    #region Stat
    public float MaxHp { get; protected set; }
    public float Hp { get; protected set; }
    public float Atk { get; protected set; }
    public float Def { get; protected set; }
    public float MoveSpeed { get; protected set; }
    #endregion

    ECreatureState _state = ECreatureState.Idle;
    public ECreatureState State
    {
        get { return _state; }
        protected set
        {
            if (_state != value)
            {
                _state = value;
                UpdateAnimation();
            }
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

    public override bool Init()
    {
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

        // TODO: Skill

        State = ECreatureState.Idle;
    }

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
                break;
            case ECreatureState.OnDamaged:
                break;
            case ECreatureState.Dead:
                break;
        }
    }

    protected void UpdateIdleAnimation()
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

    protected void UpdateMoveAnimation()
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
}
