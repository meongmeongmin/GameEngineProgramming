using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        Scene = EScene.GameScene;

        Player player = Managers.Object.Spawn<Player>(Vector3.zero, PLAYER_ID);
        CameraController camera = Camera.main.GetOrAddComponent<CameraController>();
        camera.Target = player;
    }

    public override void Clear()
    {
    }
}
