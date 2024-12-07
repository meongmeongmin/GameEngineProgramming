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
    public Data.TileData Data { get; protected set; }
    public ETileType TileType { get { return Data.TileType; } }
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
        Data = Managers.Data.TileDataDic[dataID];
        gameObject.name = $"{Data.DataID}_{Data.Name}";

        // SpriteRenderer
        Sprite sprite = Managers.Resource.Load<Sprite>(Data.SpriteName);
        if (sprite == null)
            SpriteRenderer.sprite = Util.FindTileMapsSprite(Data.SpriteName);
        else
            SpriteRenderer.sprite = sprite;

        switch (Data.TileType)
        {
            case ETileType.Exit:
                Interaction = new ExitInteraction();
                break;
            case ETileType.LockedDoor:
                Interaction = new LockedDoorInteraction();
                break;
            case ETileType.TreasureChest:
                Interaction = new TreasureChestInteraction();
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
