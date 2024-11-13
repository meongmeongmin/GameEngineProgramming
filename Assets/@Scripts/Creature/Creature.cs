using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Creature : MonoBehaviour
{
    public EObjectType ObjectType { get; protected set; }

    public Rigidbody2D RigidBody { get; private set; }
    public CircleCollider2D Collider { get; private set; }

    public float ColliderRadius { get { return Collider != null ? Collider.radius : 0.0f; } }
    public Vector3 CenterPosition { get { return transform.position + Vector3.up * ColliderRadius; } }

    protected Animator _animator;

    public int DataID { get; set; }
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
                State = ECreatureState.Move;
            }
        }
    }

    void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();

        State = ECreatureState.Idle;
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
                break;
            case ECreatureState.Move:
                UpdateMoveAnimation();
                State = ECreatureState.Idle;
                break;
            case ECreatureState.Skill:
                break;
            case ECreatureState.OnDamaged:
                break;
            case ECreatureState.Dead:
                break;
        }
    }

    protected void UpdateMoveAnimation()
    {
        switch (Dir)
        {
            case EDir.Up:
                _animator.Play("Up");
                break;
            case EDir.Left:
                _animator.Play("Left");
                break;
            case EDir.Down:
                _animator.Play("Down");
                break;
            case EDir.Right:
                _animator.Play("Right");
                break;
        }
    }
}
