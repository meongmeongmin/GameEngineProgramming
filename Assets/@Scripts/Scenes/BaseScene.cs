using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public abstract class BaseScene : MonoBehaviour
{
    public EScene Scene { get; protected set; } = EScene.None;

    void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType<EventSystem>();
        if (obj == null)
        {
            GameObject go = new GameObject() { name = "@EventSystem" };
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }
    }

    public abstract void Clear();
}
