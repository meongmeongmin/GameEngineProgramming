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
        if (IsClosed)
            return true;

        return false;
    }

    public void HandleOnCollisionEvent(BaseObject target)
    {
        if (CanInteract())
        {
            _tile.SpriteRenderer.sprite = Util.FindTileMapsSprite("Items_4");
            // TODO: 아이템 획득
            IsClosed = false;
        }
    }

    public void SetInfo(EffectTile tile)
    {
        _tile = tile;
        IsClosed = true;
    }
}
