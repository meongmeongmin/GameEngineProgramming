using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LobbyScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        Scene = EScene.LobbyScene;
    }

    public override void Clear()
    {
    }
}
