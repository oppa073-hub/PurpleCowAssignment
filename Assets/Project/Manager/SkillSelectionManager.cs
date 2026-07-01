using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject selectionPanel;  //스킬 선택 UI 패널
    [SerializeField] private List<SkillData> allSkills = new List<SkillData>(); //전체 스킬 데이터 목록
    [SerializeField] private PlayerSkillInventory inventory; //플레이어 보유 스킬 정보
    [SerializeField] private SkillCardUI[] skillCards;
    [SerializeField] private BallManager ballManager;
    [SerializeField] private SkillInventoryUI skillInventoryUI;
    private List<SkillData> currentOptions = new List<SkillData>(); //현재 선택지로 표시될 스킬 목록

    private void Awake()
    {
        selectionPanel.SetActive(false);
        skillInventoryUI.gameObject.SetActive(false);
    }

    public void OpenSelection() //랜덤 스킬 선택지를 생성하고 UI 표시
    {
        currentOptions = GetRandomOptions();

        for (int i = 0; i < skillCards.Length; i++)
        {
            if (i < currentOptions.Count)
            {
                SkillData skill = currentOptions[i];
                int targetLevel = inventory.GetLevel(skill) + 1;

                skillCards[i].gameObject.SetActive(true);
                skillCards[i].SetCard(skill, targetLevel);
            }
            else
            {
                skillCards[i].gameObject.SetActive(false);
            }
        }
        selectionPanel.SetActive(true);
        skillInventoryUI.gameObject.SetActive(true);
        GameManager.Instance.ChangeState(GameState.SkillSelection); // 선택 중 게임 정지
    }
    private List<SkillData> GetRandomOptions() //선택 가능한 스킬 중 랜덤 3개 추출
    {
        List<SkillData> candidates = new List<SkillData>();

        int activeCount = inventory.GetOwnedCount(SkillType.Active);
        int passiveCount = inventory.GetOwnedCount(SkillType.Passive);

        for (int i = 0; i < allSkills.Count; i++)
        {
            SkillData skill = allSkills[i];

            bool hasSkill = inventory.HasSkill(skill);  //액티브 4개랑 패시브 2개 보유 제한
            if (!hasSkill)
            {
                if (skill.skillType == SkillType.Active && activeCount >= 4)
                    continue;

                if (skill.skillType == SkillType.Passive && passiveCount >= 2)
                    continue;
            }

            int currentLevel = inventory.GetLevel(skill);

            if (currentLevel >= skill.maxLevel) continue;

            candidates.Add(skill);
        }

        List<SkillData> result = new List<SkillData>();

        for (int i = 0; i < 3; i++)
        {
            if (candidates.Count <= 0) break;

            int randomIndex = Random.Range(0, candidates.Count);
            result.Add(candidates[randomIndex]);
            candidates.RemoveAt(randomIndex);
        }

        return result;
    }

    public void SelectSkill(int index) //선택한 스킬을 획득 또는 레벨업
    {
        if (index < 0 || index >= currentOptions.Count) return;

        SkillData selectedSkill = currentOptions[index];

        inventory.AddOrLevelUpSkill(selectedSkill);

        if (selectedSkill.skillType == SkillType.Active)
        {
            ActiveSkillData activeSkill = selectedSkill as ActiveSkillData;

            if (activeSkill != null)
            {
                ballManager.AddBall(activeSkill.linkedBallData);
            }
        }
        skillInventoryUI.Refresh();
        skillInventoryUI.gameObject.SetActive(false);
        selectionPanel.SetActive(false);
        GameManager.Instance.ChangeState(GameState.Playing);
    }
}
