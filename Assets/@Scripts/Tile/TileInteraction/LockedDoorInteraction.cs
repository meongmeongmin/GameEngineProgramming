using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LockedDoorInteraction : ITileInteraction
{
    EffectTile _tile;
    public bool IsLocked { get; set; }

    public bool CanInteract()
    {
        if (Managers.Game.Inventory.UseItem(EItemType.Key))
            IsLocked = false;

        return !IsLocked;
    }

    public void HandleOnCollisionEvent(BaseObject target)
    {
        Player player = target as Player;
        if (player != null && CanInteract())
        {
            Managers.Sound.Play(ESound.Effect, "se_doorOpen");
            Managers.Object.Despawn(_tile);
        }
        else
            Managers.Sound.Play(ESound.Effect, "se_doorClosed");
    }

    public void SetInfo(EffectTile tile)
    {
        _tile = tile;
        IsLocked = true;
    }
}
