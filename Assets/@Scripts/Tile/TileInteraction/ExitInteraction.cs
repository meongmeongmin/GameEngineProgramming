using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ExitInteraction : ITileInteraction
{
    public bool CanInteract()
    {
        return true;
    }

    public void HandleOnCollisionEvent(BaseObject target)
    {
        Player player = target as Player;
        if (player != null)
        {
            Managers.Game.SaveGame();
            switch (Managers.Scene.CurrentScene.Scene)
            {
                case EScene.IslandScene:
                    Managers.Scene.LoadScene(EScene.DungeonScene);
                    break;
                case EScene.DungeonScene:
                    Managers.Scene.LoadScene(EScene.IslandScene);
                    break;
            }
        }
    }

    public void SetInfo(EffectTile tile)
    {

    }
}
