using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Player : Creature
{
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

    public override void Init()
    {
        base.Init();
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
