using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TreasureChestInteraction : ITileInteraction
{
    EffectTile _tile;
    public bool IsClosed;

    public bool CanInteract()
    {
        return IsClosed;
    }

    public void HandleOnCollisionEvent(BaseObject target)
    {
        if (CanInteract())
        {
            _tile.SpriteRenderer.sprite = Util.FindTileMapsSprite("Items_4");   // 열린 상자 스프라이트
            Item key = Managers.Object.Spawn<Item>(_tile.transform.position, KEY_ID);
            IsClosed = false;
        }
    }

    public void SetInfo(EffectTile tile)
    {
        _tile = tile;
        IsClosed = true;
    }
}
