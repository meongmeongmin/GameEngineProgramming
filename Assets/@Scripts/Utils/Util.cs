using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static Define;

public static class Util
{
    public static Vector3 DirToVector3(EDir dir)
    {
        switch (dir)
        {
            case EDir.Up:
                return Vector3.up; // (0, 1, 0)
            case EDir.Left:
                return Vector3.left; // (-1, 0, 0)
            case EDir.Down:
                return Vector3.down; // (0, -1, 0)
            case EDir.Right:
                return Vector3.right; // (1, 0, 0)
            default:
                return Vector3.zero; // 예외 처리
        }
    }

    public static float GetAngle(Vector2 fromPos, Vector2 toPos)
    {
        Vector2 deltaPos = (toPos - fromPos).normalized;
        float radian = Mathf.Atan2(deltaPos.y, deltaPos.x);
        float angle = radian * Mathf.Rad2Deg;
        return angle;
    }

    public static Sprite FindTileMapsSprite(string spriteName)
    {
        string name = Regex.Replace(spriteName, "_.*", "");
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath($"Assets/@Resources/TileMaps/02_sprites/{name}.png")
                             .OfType<Sprite>()
                             .ToArray();
        Sprite spr = sprites.FirstOrDefault(s => s.name == spriteName);
        return spr;
    }
}
