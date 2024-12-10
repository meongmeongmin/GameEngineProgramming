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
        StartLoadAssets();
    }

    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Managers.Data.Init();
                Managers.Game.InitGame();
                Invoke("LoadIslandScene", 1.0f);    // 씬 전환 Test
            }
        });
    }

    void LoadIslandScene()    // 씬 전환 Test
    {
        Managers.Scene.LoadScene(EScene.IslandScene);
    }

    public override void Clear()
    {
    }
}
