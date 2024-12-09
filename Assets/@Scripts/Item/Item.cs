using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class Item : BaseObject
{
    public Data.ItemData Data { get; private set; }
    public EItemType ItemType { get; private set; }

    public override bool Init()
    {
        base.Init();
        ObjectType = EObjectType.Item;

        return true;
    }

    public void SetInfo(int dataID)
    {
        Data = Managers.Data.ItemDataDic[dataID];
        ItemType = Data.ItemType;

        SpriteRenderer.sprite = Util.FindTileMapsSprite(Data.SpriteName);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        Collider.enabled = false;

        if (ItemType == EItemType.Key)
        {
            Managers.Game.Inventory.AddItem(EItemType.Key);
        }
        else if (ItemType == EItemType.Life)
        {
            float healAmount = player.MaxHp * 0.2f; // 최대 HP의 20%만큼 회복
            player.OnHeal(healAmount);
        }

        Vector2 direction = (player.transform.position - transform.position).normalized;
        StartCoroutine(CoDrop(direction, player));
    }

    IEnumerator CoDrop(Vector2 direction, BaseObject target)
    {
        Vector3 startPosition = transform.position;
        
        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            Vector3 dir = target.transform.position - transform.position;
            if (dir.magnitude < 0.1f)
                break;

            elapsedTime += Time.deltaTime;
            Vector3 nextPosition = Vector3.Lerp(transform.position, target.transform.position, elapsedTime / duration);
            RigidBody.MovePosition(nextPosition);

            yield return null;
        }

        RigidBody.velocity = Vector2.zero;
        Managers.Sound.Play(ESound.Effect, "se_get");
        Managers.Object.Despawn(this);
    }

}
