using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    public Data.MonsterData Data { get; protected set; }
    
    protected float _searchScope;

    float _lastDamageTime = 0f;
    float _damageInterval = 0.5f;

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
        _searchScope = Data.SearchScope;
    }

    void Update()
    {
        if (State == ECreatureState.Dead || State == ECreatureState.OnDamaged)
            return;

        // 플레이어 탐색
        Target = Managers.Object.Player;
        if (Target == null)
        {
            State = ECreatureState.Idle;
            return;
        }

        float dist = Vector2.Distance(transform.position, Target.transform.position);

        if (dist < _searchScope)   // 플레이어 탐지
        {
            _angle = GetAngle(transform.position, Target.transform.position, out float radian);
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
        Vector2 deltaPos = (toPos - fromPos).normalized;
        radian = Mathf.Atan2(deltaPos.y, deltaPos.x);
        return Util.GetAngle(fromPos, toPos);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        // 0.5초 간격으로 데미지를 준다
        if (Time.time - _lastDamageTime >= _damageInterval)
        {
            player.OnDamaged(this, null);
            _lastDamageTime = Time.time;
        }
    }

    public override void OnDead()
    {
        base.OnDead();

        Boss boss = this as Boss;
        if (boss != null)
            Managers.Sound.Play(ESound.Effect, "se_bosskilled");
        else
            Managers.Sound.Play(ESound.Effect, "se_killed");

        // SpawnInfos에서 본인 정보 중 상태를 변경
        ObjectSpawnInfo matchingInfo = Managers.Map.StageTransition.CurrentStage.SpawnInfos.FirstOrDefault(info => info.ObjectType == ObjectType && info.WorldPos == InitPosition);
        if (matchingInfo != null)
            matchingInfo.State = ECreatureState.Dead;

        CreateDropItem();
    }

    void CreateDropItem()
    {
        // 열쇠가 필요하면 바로 생성
        if (Data.DropItemIDList.Contains(KEY_ID) && IsKeyRequired())
        {
            Managers.Object.Spawn<Item>(transform.position, KEY_ID);
            return;
        }

        int id = KEY_ID; 
        
        if (IsKeyPresent())
        {
            // KEY_ID를 제외한 아이템 선택
            var nonKeyItems = Data.DropItemIDList.Where(item => item != KEY_ID).ToList();
            id = nonKeyItems[Random.Range(0, nonKeyItems.Count)];
        }
        else
        {
            // KEY_ID 포함, 랜덤으로 선택
            id = Data.DropItemIDList[Random.Range(0, Data.DropItemIDList.Count)];
        }

        Managers.Object.Spawn<Item>(transform.position, id);
    }

    bool IsKeyRequired()
    {
        if (IsKeyPresent())
            return false;

        var spawnInfos = Managers.Map.StageTransition.CurrentStage.SpawnInfos;
        int monsterCount = spawnInfos.Count(info => info.ObjectType == ObjectType && info.State != ECreatureState.Dead);    // 살아있는 몬스터 수
            
        return monsterCount <= 1;
    }

    bool IsKeyPresent()
    {
        return Managers.Game.Inventory.KeyCount > 0 || Managers.Object.Items.Any(item => item.ItemType == EItemType.Key);
    }

    protected override void UpdateIdleAnimation()
    {
        _animator.Play("Idle");
    }
}
