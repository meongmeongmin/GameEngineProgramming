using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.EScene type)
    {
        SceneManager.LoadScene(GetSceneName(type));
    }

    public string GetSceneName(Define.EScene type)
    {
        return System.Enum.GetName(typeof(Define.EScene), type);
    }

    public void Clear()
    {
    }
}
