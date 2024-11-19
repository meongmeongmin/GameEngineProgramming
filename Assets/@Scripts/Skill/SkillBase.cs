using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillBase : MonoBehaviour
{
    public Creature Owner { get; protected set; }
    public Data.SkillData Data { get; private set; }

    public virtual void SetInfo(Creature owner, int skillID)
    {
        Owner = owner;
        Data = Managers.Data.SkillDataDic[skillID];
    }

    public virtual void DoSkill()
    {
        Debug.Log("DoSkill");
        Owner.State = ECreatureState.Skill;
    }

