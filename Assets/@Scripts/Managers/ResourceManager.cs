using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager
{
    Dictionary<string, Object> _resources = new Dictionary<string, Object>();
    Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();

    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (_resources.TryGetValue(key, out Object resource))
            return resource as T;

        if (typeof(T) == typeof(Sprite))
        {
            //key = $"{key}.sprite";
            if (_resources.TryGetValue(key, out Object tmp))
                return tmp as T;
        }

        return null;
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {key}");
            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }

    #region Addressable
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : Object
    {
        // 스프라이트인 경우 하위 객체의 이름으로 로드해야 한다.
        string loadkey = key;
        //if (key.Contains(".sprite"))
        //    loadkey = $"{key}[{key.Replace(".sprite", "")}]";

        var asyncOperation = Addressables.LoadAssetAsync<T>(key);
        asyncOperation.Completed += (op) =>
        {
            if (_resources.TryGetValue(key, out Object resource))
            {
                callback?.Invoke(resource as T);
                return;
            }

            _resources.Add(key, op.Result);
            _handles.Add(key, asyncOperation);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback = null) where T : Object
    {
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                if (result.PrimaryKey.Contains(".sprite"))
                {
                    LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
                else
                {
                    LoadAsync<T>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
            }
        };
    }

    public void Clear()
    {
        _resources.Clear();

        foreach (var handle in _handles)
            Addressables.Release(handle);
        _handles.Clear();
    }
    #endregion
}
