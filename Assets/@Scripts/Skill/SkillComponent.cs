using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillComponent : MonoBehaviour
{
    public List<SkillBase> SkillList { get; } = new List<SkillBase>();
    public SkillBase DefaultSkill { get; set; }
    public SkillBase AuxiliarySkill { get; set; }

    Creature _owner;

    void Awake()
    {
        Init();
    }

    public void Init()
    {

    }

    public void SetInfo(Creature owner)
    {
        _owner = owner;
        foreach (int skillID in owner.CreatureData.SkillIDList)
            AddSkill(skillID);
    }

    public void AddSkill(int skillID)
    {
        if (skillID == 0)
            return;

        if (Managers.Data.SkillDataDic.TryGetValue(skillID, out var data) == false)
        {
            Debug.LogWarning($"AddSkill 실패 {skillID}");
            return;
        }

        SkillBase skill = gameObject.AddComponent(Type.GetType(data.Name)) as SkillBase;
        if (skill == null)
            return;

        skill.SetInfo(_owner, skillID);
        SkillList.Add(skill);

        // 기본값
        if (_owner.ObjectType == Define.EObjectType.Player)
        {
            if (skillID == 30001)
                DefaultSkill = skill;
            else if (skillID == 30011)
                AuxiliarySkill = skill;
        }
        else if (_owner.ObjectType == Define.EObjectType.Monster)
        {
            if (skillID == 30021)
                DefaultSkill = skill;
        }
    }
}
