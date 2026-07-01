using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Sprite emptyIcon;

    public void SetSlot(SkillData skill, int level)
    {
        iconImage.sprite = skill.icon;
        iconImage.enabled = true;

        levelText.text = "x" + level;
        levelText.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        iconImage.sprite = emptyIcon;
        iconImage.enabled = false;

        levelText.text = "";
        levelText.gameObject.SetActive(false);
    }
}