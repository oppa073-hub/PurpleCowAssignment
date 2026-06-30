using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillInventory : MonoBehaviour
{
    [SerializeField] private List<PlayerSkill> ownedSkills = new List<PlayerSkill>();
    public IReadOnlyList<PlayerSkill> OwnedSkills => ownedSkills;

    public bool HasSkill(SkillData skill)
    {
        for (int i = 0; i < ownedSkills.Count; i++)
        {
            if (ownedSkills[i].skillData == skill) return true;
        }

        return false;
    }

    public int GetLevel(SkillData skill)
    {

        for (int i = 0; i < ownedSkills.Count; i++)
        {
            if (ownedSkills[i].skillData == skill) return ownedSkills[i].currentLevel;
        }
        return 0;
    }

    public void AddOrLevelUpSkill(SkillData skill)
    {
        for (int i = 0; i < ownedSkills.Count; i++)
        {
            if (ownedSkills[i].skillData == skill)
            {
                ownedSkills[i].currentLevel++; 
                return;
            }
        }

        PlayerSkill newSkill = new PlayerSkill();
        newSkill.skillData = skill;
        newSkill.currentLevel = 1;

        ownedSkills.Add(newSkill);
    }
    public int GetOwnedCount(SkillType skillType)  //몇개 가지고있는지 세기
    {
        int count = 0;

        for (int i = 0; i < ownedSkills.Count; i++)
        {
            if (ownedSkills[i].skillData.skillType == skillType) count++;
        }

        return count;
    }
    public PlayerSkill GetPlayerSkill(SkillData skill) 
    {
        for (int i = 0; i < ownedSkills.Count; i++)
        {
            if (ownedSkills[i].skillData == skill)
            {
                return ownedSkills[i];
            }
        }

        return null;
    }
}
