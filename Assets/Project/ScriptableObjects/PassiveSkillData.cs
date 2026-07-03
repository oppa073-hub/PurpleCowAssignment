using UnityEngine;

public enum PassiveType
{
    WarmHeart,
    MagicMirror,
    RubyDagger,
    EmeraldDagger,
    LastMatch
}

[CreateAssetMenu(fileName = "PassiveSkillData", menuName = "PurpleCow/Passive Skill Data")]
public class PassiveSkillData : SkillData
{
    public PassiveType passiveType;
}