using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }
    static bool s_init = false;

    DataManager _data = new DataManager();
    public static DataManager Data { get { return Instance?._data; } }
    ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instance?._resource; } }

    public static void Init()
    {
        if (s_init == false)
        {
            s_init = true;

            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject() { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }

    public static void Clear()
    {

    }
}
