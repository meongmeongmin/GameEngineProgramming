using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Creature : MonoBehaviour
{
    public float Speed { get; protected set; } = 3.0f;

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

    protected Rigidbody2D _rigidBody;
    protected Animator _animator;

    void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

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
