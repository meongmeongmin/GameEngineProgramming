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
    public class PlayerData : CreatureData
    { }

    [Serializable]
    public class MonsterData : CreatureData
    {
        public int DropExp;
    }
}