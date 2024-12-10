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
        Managers.Sound.Play(ESound.Bgm, "BGM_game_00");

        Player player = Managers.Object.Spawn<Player>(new Vector3(-0.5f, -4.5f), PLAYER_ID);
        Managers.Map.StageTransition.CheckMapChanged(player.transform.position);
        CameraController camera = Camera.main.GetOrAddComponent<CameraController>();
        camera.Target = player;

        Managers.Game.UI_GameScene = Managers.Resource.Instantiate("UI_GameScene").GetComponent<UI_GameScene>();
        Managers.Game.UI_GameScene.SetInfo(player);
    }

    public override void Clear()
    {
    }
}
