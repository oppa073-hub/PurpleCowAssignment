using UnityEngine;
public enum SkillType
{
    Active,
    Passive
}

[CreateAssetMenu(fileName = "SkillData", menuName = "PurpleCow/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public SkillType skillType;
    public Sprite icon;
    public int maxLevel = 3;
    public SkillLevelData[] levels;
}