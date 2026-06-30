using UnityEngine;

public class SkillSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject selectionPanel;

    private void Awake()
    {
        selectionPanel.SetActive(false);
    }

    public void OpenSelection()
    {
        selectionPanel.SetActive(true);
        Time.timeScale = 0f; // 선택 중 게임 정지
    }

    public void SelectSkill()
    {
        selectionPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
