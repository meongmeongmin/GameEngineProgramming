using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Exit : BaseObject
{
    Data.TileData _data;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Exit;

        return true;
    }

    public void SetInfo(int dataID)
    {
        _data = Managers.Data.TileDataDic[dataID];
        gameObject.name = $"{_data.DataID}_{_data.Name}";
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
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
}
