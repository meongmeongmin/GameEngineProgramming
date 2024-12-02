using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.U2D;
using static Define;

public interface ITileInteraction
{
    public void SetInfo(EffectTile tile);
    public void HandleOnCollisionEvent(BaseObject target);
    public bool CanInteract();
}

public class EffectTile : BaseObject
{
    Data.TileData _data;
    public ETileType TileType { get { return _data.TileType; } }
    public ITileInteraction Interaction { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Tile;
        return true;
    }

    public void SetInfo(int dataID)
    {
        _data = Managers.Data.TileDataDic[dataID];
        gameObject.name = $"{_data.DataID}_{_data.Name}";

        // SpriteRenderer
        Sprite sprite = Managers.Resource.Load<Sprite>(_data.SpriteName);
        if (sprite == null)
            SpriteRenderer.sprite = Util.FindTileMapsSprite(_data.SpriteName);
        else
            SpriteRenderer.sprite = sprite;

        switch (_data.TileType)
        {
            case ETileType.Exit:
                Interaction = new ExitInteraction();
                break;
            case ETileType.LockedDoor:
                Interaction = new LockedDoorInteraction();
                break;
        }

        Interaction?.SetInfo(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BaseObject obj = collision.gameObject.GetComponent<BaseObject>();
        if (obj != null)
            Interaction?.HandleOnCollisionEvent(obj);
    }
}
