using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        Scene = EScene.GameScene;

        Managers.Object.Spawn<Player>(Vector3.zero, PLAYER_ID);
    }

    public override void Clear()
    {
    }
}
