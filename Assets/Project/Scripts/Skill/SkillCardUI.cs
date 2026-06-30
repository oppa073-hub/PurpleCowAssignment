using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCardUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private Image[] levelIcons;
    [SerializeField] private Sprite emptyLevelIcon;
    [SerializeField] private Sprite filledLevelIcon;

    public void SetCard(SkillData skill, int targetLevel)
    {
        iconImage.sprite = skill.icon;
        nameText.text = skill.skillName;

        SkillLevelData levelData = skill.levels[targetLevel - 1];
        descriptionText.text = levelData.description;

        for (int i = 0; i < levelIcons.Length; i++)
        {
            if (i < targetLevel)
            {
                levelIcons[i].sprite = filledLevelIcon;
            }
            else
            {
                levelIcons[i].sprite = emptyLevelIcon;
            }
        }
    }
}