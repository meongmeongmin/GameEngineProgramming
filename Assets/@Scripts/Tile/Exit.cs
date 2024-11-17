using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : BaseObject
{
    Data.TileData _data;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = Define.EObjectType.Exit;

        return true;
    }

    public void SetInfo(int dataID)
    {
        _data = Managers.Data.TileDataDic[dataID];
        gameObject.name = $"{_data.DataID}_{_data.Name}";
    }
}
