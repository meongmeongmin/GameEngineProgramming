using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Player : Creature
{
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

    public override void OnDead()
    {
        base.OnDead();
        // TODO: 플레이어 사망 처리
    }

    void Update()
    {
        if (State == ECreatureState.Dead)
            return;

        GetDirInput();
        GetSkillInput();
    }

    void FixedUpdate()
    {
        //RigidBody.velocity = Axis * MoveSpeed;
        //transform.position += (Vector3)Axis * MoveSpeed * Time.deltaTime;
        Vector3 nextPosition = transform.position + ((Vector3)Axis * MoveSpeed * Time.deltaTime);
        RigidBody.MovePosition(nextPosition);
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
            _angle = GetAngle(fromPos, fromPos + Axis);

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

        Vector2 deltaPos = (toPos - fromPos).normalized;
        float radian = Mathf.Atan2(deltaPos.y, deltaPos.x);
        angle = radian * Mathf.Rad2Deg;

        return angle;
    }

    void GetSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))    // 기본 스킬
            Skills.DefaultSkill.DoSkill();

        else if (Input.GetKeyDown(KeyCode.X))   // 보조 스킬
            Skills.AuxiliarySkill.DoSkill();

        else if (State == ECreatureState.Skill)
            State = ECreatureState.Idle;
    }
}
