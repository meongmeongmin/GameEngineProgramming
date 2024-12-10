using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Define;

[Serializable]
public class GameSaveData
{
    public PlayerSaveData Player;
    public List<ItemSaveData> Items = new List<ItemSaveData>();
}

[Serializable]
public class PlayerSaveData
{
    public int DataID;
    public int Level = 1;
    public float Exp = 0;
    public float Hp;
}

[Serializable]
public class ItemSaveData
{
    public int DataID;
    public int Count;
}

public class Inventory
{
    int _keyCount = 0;
    public int KeyCount 
    {
        get { return _keyCount; } 
        protected set
        {
            _keyCount = value;
            Managers.Game.KeyCountChanged?.Invoke(_keyCount);
        }
    }

    public bool UseItem(EItemType type)
    {
        if (type == EItemType.Key && KeyCount > 0)
        {
            KeyCount--;
            return true;
        }

        return false;
    }

    public void AddItem(EItemType type)
    {
        if (type == EItemType.Key)
            KeyCount++;
    }
}

public class GameManager
{
    GameSaveData _saveData;
    public GameSaveData SaveData
    { 
        get { return _saveData; } 
        set { _saveData = value; } 
    }

    public Inventory Inventory { get; set; }
    public UI_GameScene UI_GameScene { get; set; }

    public Action<int> KeyCountChanged;

    public string Path { get { return Application.persistentDataPath + "/SaveData.json"; } }

    public void InitGame()  // TODO
    {
        _saveData = new GameSaveData();
        Inventory = new Inventory();
        //if (File.Exists(Path))
        //    return;

        //string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        //File.WriteAllText(Path, jsonStr);
        //Debug.Log($"Save Game Completed : {Path}");
    }

    public void SaveGame()  // TODO
    {
        string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        File.WriteAllText(Path, jsonStr);
        Debug.Log($"Save Game Completed : {Path}");
    }

    public bool LoadGame()  // TODO
    {
        if (File.Exists(Path) == false)
            return false;

        string fileStr = File.ReadAllText(Path);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(fileStr);

        if (data != null)
            Managers.Game.SaveData = data;

        Debug.Log($"Save Game Loaded : {Path}");
        return true;
    }
}

