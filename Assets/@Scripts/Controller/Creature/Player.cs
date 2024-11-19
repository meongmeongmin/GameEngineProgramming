using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Player : Creature
{
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

    float _angle = -90.0f;

    public override bool Init()
    {
        base.Init();

        ObjectType = EObjectType.Player;
        return true;
    }

    public override void SetInfo(int dataID)
    {
        base.SetInfo(dataID);
    }

    void Update()
    {
        GetDirInput();
        GetSkillInput();
    }

    void FixedUpdate()
    {
        RigidBody.velocity = Axis * MoveSpeed;
        Managers.Map.StageTransition.CheckMapChanged(transform.position);
    }

    void GetDirInput()
    {
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");
        Axis = new Vector2(axisH, axisV);

        if (Axis != Vector2.zero)   // 방향키 입력 감지
        {
            // 이동 각도 구하기
            Vector2 fromPos = transform.position;
            Vector2 toPos = fromPos + Axis;
            _angle = GetAngle(fromPos, toPos);

            // 방향 구하기
            if (_angle >= 45 && _angle <= 135)
                Dir = EDir.Up;
            else if (_angle > 135 || _angle < -135)
                Dir = EDir.Left;
            else if (_angle >= -135 && _angle <= -45)
                Dir = EDir.Down;
            else if (_angle >= -45 && _angle < 45)
                Dir = EDir.Right;

            State = ECreatureState.Move;
        }
        else if (State == ECreatureState.Move)
            State = ECreatureState.Idle;
    }

    float GetAngle(Vector2 fromPos, Vector2 toPos)
    {
        float angle = 0;
        if (Axis.x != 0 || Axis.y != 0)
        {
            Vector2 deltaPos = new Vector2(toPos.x - fromPos.x, toPos.y - fromPos.y);
            float radian = Mathf.Atan2(deltaPos.y, deltaPos.x);
            angle = radian * Mathf.Rad2Deg;
        }
        else
            angle = _angle;

        return angle;
    }

    void GetSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            Skills.DefaultSkill.DoSkill();
        else if (State == ECreatureState.Skill)
            State = ECreatureState.Idle;
    }
}
