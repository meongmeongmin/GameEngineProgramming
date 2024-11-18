using System.Collections;
using System.Collections.Generic;
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
}
