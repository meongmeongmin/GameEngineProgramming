using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class LevelData
    {
        public int Level;
        public float RequiredExp;
    }

    [Serializable]
    public class LevelDataLoader : ILoader<int, LevelData>
    {
        public List<LevelData> levels = new List<LevelData>();
        public Dictionary<int, LevelData> MakeDict()
        {
            Dictionary<int, LevelData> dict = new Dictionary<int, LevelData>();
            foreach (LevelData levelData in levels)
                dict.Add(levelData.Level, levelData);
            return dict;
        }
    }

    [Serializable]
    public class CreatureData
    {
        public int DataID;
        public string PrefabLabel;
        public int Level;
        public float Exp;
        public float MaxHp;
        public float Hp;
        public float HpRate;
        public float Atk;
        public float AtkRate;
        public float Def;
        public float DefRate;
        public float MoveSpeed;
        public List<int> SkillIDList;
    }

    [Serializable]
    public class CreatureDataLoader : ILoader<int, CreatureData>
    {
        public List<CreatureData> levels = new List<CreatureData>();
        public Dictionary<int, CreatureData> MakeDict()
        {
            Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();
            foreach (CreatureData levelData in levels)
                dict.Add(levelData.Level, levelData);
            return dict;
        }
    }

    [Serializable]
    public class PlayerData : CreatureData
    { }

    [Serializable]
    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
        public List<PlayerData> player = new List<PlayerData>();
        public Dictionary<int, PlayerData> MakeDict()
        {
            Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
            foreach (PlayerData p in player)
                dict.Add(p.DataID, p);
            return dict;
        }
    }

    [Serializable]
    public class MonsterData : CreatureData
    {
        public int DropExp;
    }

    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();
        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData monster in monsters)
                dict.Add(monster.DataID, monster);
            return dict;
        }
    }
}