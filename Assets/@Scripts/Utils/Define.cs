using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum EScene
    {
        None,
        LobbyScene,
        GameScene,
    }

    public enum EDir
    {
        Up,
        Left,
        Down,
        Right
    }

    public enum ECreatureState
    {
        Idle,
        Move,
        Skill,
        OnDamaged,
        Dead,
    }
}
