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
        // TODO: 특정 조건(예: 열쇠 소지 여부) 충족 시 문이 열리도록 설정
        return !IsLocked;
    }

    public void HandleOnCollisionEvent(BaseObject target)
    {
        if (CanInteract())
            Managers.Object.Despawn(_tile);
    }

    public void SetInfo(EffectTile tile)
    {
        _tile = tile;
        IsLocked = true;
    }
}
