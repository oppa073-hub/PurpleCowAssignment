[System.Serializable]
public class SkillLevelData
{
    public int damage;

    public float chance;      //확률
    public float duration;    //지속시간
    public float value;       //감속률, 추가피해율 등
    public int subDamage;     //화상, 클러스터 특수볼 데미지
    public int stackCount;    //최대 중첩
}