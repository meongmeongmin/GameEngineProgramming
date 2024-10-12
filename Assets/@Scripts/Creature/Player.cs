using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Player : MonoBehaviour
{
    public float Speed { get; } = 3.0f;

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

    EDir _dir = EDir.Down;
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

    Vector2 _axis;
    public Vector2 Axis // 입력 키 방향
    {
        get { return _axis; }
        protected set
        {
            if (_axis == value)
                _axis = value;
        }
    }

    float _angle = -90.0f;

    Rigidbody2D _rigidBody;
    Animator _animator;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        State = ECreatureState.Idle;
    }

    void Update()
    {
        if (State == ECreatureState.Idle)
        {
            float axisH = Input.GetAxisRaw("Horizontal");
            float axisV = Input.GetAxisRaw("Vertical");
            _axis = new Vector2(axisH, axisV);

            if (_axis != Vector2.zero)
            {
                Vector2 fromPos = transform.position;
                Vector2 toPos = new Vector2(fromPos.x + Axis.x, fromPos.y + Axis.y);
                _angle = GetAngle(fromPos, toPos);

                if (_angle >= 45 && _angle <= 135)
                    Dir = EDir.Up;
                else if (_angle > 135 || _angle < -135)
                    Dir = EDir.Left;
                else if (_angle >= -135 && _angle <= -45)
                    Dir = EDir.Down;
                else if (_angle >= -45 && _angle < 45)
                    Dir = EDir.Right;
            }
        }
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = Axis * Speed;
    }

    void UpdateAnimation()
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

    void UpdateMoveAnimation()
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

    float GetAngle(Vector2 fromPos, Vector2 toPos)
    {
        float angle = 0;
        if (Axis.x != 0 || Axis.y != 0)
        {
            Vector2 destPos = new Vector2(toPos.x - fromPos.x, toPos.y - fromPos.y);
            float radian = Mathf.Atan2(destPos.y, destPos.x);
            angle = radian * Mathf.Rad2Deg;
        }
        else
            angle = _angle;

        return angle;
    }
}
