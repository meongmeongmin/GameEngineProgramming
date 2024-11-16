using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

[CreateAssetMenu]
public class CustomTile : Tile
{
    [Space] [Header("CustomTile")]
    public Define.EObjectType ObjectType  = Define.EObjectType.Exit;
    public int DataID;
    public string Name;
}
