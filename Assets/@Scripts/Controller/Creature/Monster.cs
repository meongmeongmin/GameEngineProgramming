using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    public Data.MonsterData Data { get; protected set; }
    
    protected float _reactionDistance;

    public override bool Init()
    {
        base.Init();

        ObjectType = EObjectType.Monster;
        return true;
    }

    public override void SetInfo(int dataID)
    {
        base.SetInfo(dataID);

        Data = Managers.Data.MonsterDataDic[dataID];
        _reactionDistance = Data.ReactionDistance;
    }

    public override void OnDead()
    {
        base.OnDead();
        // TODO: 몬스터 사망 처리 (경험치, 보상)
        Managers.Object.Despawn(this);
    }

    void Update()
    {
        if (State == ECreatureState.Dead)
            return;

        Player player = Managers.Object.Player;
        if (player == null)
        {
            Debug.Log("플레이어를 찾지 못했다");
            State = ECreatureState.Idle;
            return;
        }

        float dist = Vector2.Distance(transform.position, player.transform.position);

        if (dist < _reactionDistance)   // 플레이어 탐지
        {
            _angle = GetAngle(transform.position, player.transform.position, out float radian);
            Axis = new Vector2(Mathf.Cos(radian) * MoveSpeed, Mathf.Sin(radian) * MoveSpeed);

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
        else
            State = ECreatureState.Idle;
    }

    void FixedUpdate()
    {
        if (State == ECreatureState.Move)
        {
            Vector3 nextPosition = transform.position + ((Vector3)Axis * MoveSpeed * Time.deltaTime);
            RigidBody.MovePosition(nextPosition);
        }
    }

    float GetAngle(Vector2 fromPos, Vector2 toPos, out float radian)
    {
        float angle = 0;
        
        Vector2 deltaPos = (toPos - fromPos).normalized;
        radian = Mathf.Atan2(deltaPos.y, deltaPos.x);
        angle = radian * Mathf.Rad2Deg;

        return angle;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (State == ECreatureState.Dead)
            return;

        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        player.OnDamaged(this, null);
    }
}
