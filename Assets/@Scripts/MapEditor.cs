using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEngine.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using Newtonsoft.Json;
using UnityEditor;
#endif

public class MapEditor : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Create Object Tile Asset %#o")]
    public static void CreateObjectTile()
    {
        Dictionary<int, Data.MonsterData> MonsterDic = LoadJson<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData").MakeDict();
        foreach (var data in MonsterDic.Values)
        {
            CustomTile customTile = ScriptableObject.CreateInstance<CustomTile>();
            customTile.Name = data.PrefabLabel;

            #region Sprite
            string spriteName = data.SpriteName;
            spriteName = Regex.Replace(spriteName, "_.*", "");
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath($"Assets/@Resources/Sprites/Monsters/{spriteName}.png")
                                 .OfType<Sprite>()
                                 .ToArray();
            Sprite spr = sprites.FirstOrDefault(s => s.name == data.SpriteName);
            #endregion
            customTile.sprite = spr;
            customTile.DataID = data.DataID;
            customTile.ObjectType = Define.EObjectType.Monster;
            string name = $"{data.DataID}_{data.PrefabLabel}";
            string path = "Assets/@Resources/TileMaps/01_assets/Monster";
            path = Path.Combine(path, $"{name}.Asset");

            if (path == "")
                continue;

            if (File.Exists(path))
                continue;
            
            AssetDatabase.CreateAsset(customTile, path);
        }

        Dictionary<int, Data.TileData> TileDic = LoadJson<Data.TileDataLoader, int, Data.TileData>("TileData").MakeDict();
        foreach (var data in TileDic.Values)
        {
            CustomTile customTile = ScriptableObject.CreateInstance<CustomTile>();
            customTile.Name = data.Name;
            customTile.sprite = Util.FindTileMapsSprite(data.SpriteName); ;
            customTile.DataID = data.DataID;
            customTile.ObjectType = Define.EObjectType.Tile;
            string name = $"{data.DataID}_{data.Name}";
            string path = "Assets/@Resources/TileMaps/01_assets/Tile";
            path = Path.Combine(path, $"{name}.Asset");

            if (path == "")
                continue;

            if (File.Exists(path))
                continue;
            
            AssetDatabase.CreateAsset(customTile, path);
        }
    }

    private static Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/@Resources/Data/{path}.json");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
#endif
}
