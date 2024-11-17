using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class DungeonScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        Scene = EScene.DungeonScene;

        Managers.Map.LoadMap("DungeonMap");
        Managers.Map.StageTransition.SetInfo();

        Player player = Managers.Object.Spawn<Player>(new Vector3(-0.5f, -4.5f), PLAYER_ID);
        CameraController camera = Camera.main.GetOrAddComponent<CameraController>();
        camera.Target = player;
    }

    public override void Clear()
    {
    }
}
