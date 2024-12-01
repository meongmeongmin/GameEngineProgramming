using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Player : Creature
{
    public float StaggerTime { get; protected set; } = 0.3f;    // 피격 당할 때 움찔거리는 시간
    float _knockbackDistance = 1.5f;    // 넉백 거리

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

    public override void OnDamaged(Creature owner, SkillBase skill)
    {
        if (State == ECreatureState.Dead || State == ECreatureState.OnDamaged)  // OnDamaged일 때는 무적 상태
            return;

        base.OnDamaged(owner, skill);
        Vector3 direction = (transform.position - owner.transform.position).normalized;
        StartCoroutine(CoKnockback(direction, _knockbackDistance, StaggerTime));
    }

    public override void OnDead()
    {
        base.OnDead();
        // TODO: 플레이어 사망 처리
    }

    void Update()
    {
        if (State == ECreatureState.Dead || State == ECreatureState.OnDamaged)
            return;

        GetDirInput();
        GetSkillInput();
    }

    void FixedUpdate()
    {
        //RigidBody.velocity = Axis * MoveSpeed;
        //transform.position += (Vector3)Axis * MoveSpeed * Time.deltaTime;
        if (State == ECreatureState.Move)
        {
            Vector3 nextPosition = transform.position + ((Vector3)Axis * MoveSpeed * Time.deltaTime);
            RigidBody.MovePosition(nextPosition);
            Managers.Map.StageTransition.CheckMapChanged(transform.position);
        }
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
            _angle = Util.GetAngle(fromPos, fromPos + Axis);

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

    void GetSkillInput()
    {
        // 스킬 중복 사용 방지
        if (CurrentSkill != null)
            return;

        if (Input.GetKeyDown(KeyCode.Z))    // 기본 스킬
            CurrentSkill = Skills.DefaultSkill;
        else if (Input.GetKeyDown(KeyCode.X))   // 보조 스킬
            CurrentSkill = Skills.AuxiliarySkill;
        else
            return;

        CurrentSkill.DoSkill();
    }

    IEnumerator CoKnockback(Vector2 direction, float distance, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + (Vector3)(direction * distance);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (RigidBody == null)
                yield break;

            elapsedTime += Time.deltaTime;
            SpriteRenderer.enabled = !SpriteRenderer.enabled;
            
            Vector3 nextPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            RigidBody.MovePosition(nextPosition);

            yield return null;
        }

        SpriteRenderer.enabled = true;

        if (State != ECreatureState.Dead)
            State = ECreatureState.Idle;
    }
}
