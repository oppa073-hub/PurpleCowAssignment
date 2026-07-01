using UnityEngine;

public class SkillInventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerSkillInventory inventory;
    [SerializeField] private SkillSlotUI[] activeSlots;
    [SerializeField] private SkillSlotUI[] passiveSlots;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        ClearAllSlots();

        int activeIndex = 0;
        int passiveIndex = 0;

        for (int i = 0; i < inventory.OwnedSkills.Count; i++)
        {
            PlayerSkill playerSkill = inventory.OwnedSkills[i];
            SkillData skill = playerSkill.skillData;

            if (skill.skillType == SkillType.Active)
            {
                if (activeIndex < activeSlots.Length)
                {
                    activeSlots[activeIndex].SetSlot(skill, playerSkill.currentLevel);
                    activeIndex++;
                }
            }
            else if (skill.skillType == SkillType.Passive)
            {
                if (passiveIndex < passiveSlots.Length)
                {
                    passiveSlots[passiveIndex].SetSlot(skill, playerSkill.currentLevel);
                    passiveIndex++;
                }
            }
        }
    }

    private void ClearAllSlots()
    {
        for (int i = 0; i < activeSlots.Length; i++) activeSlots[i].ClearSlot();

        for (int i = 0; i < passiveSlots.Length; i++) passiveSlots[i].ClearSlot();
    }
}