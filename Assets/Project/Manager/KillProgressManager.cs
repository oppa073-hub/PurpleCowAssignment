using UnityEngine;

public class KillProgressManager : MonoBehaviour
{
    [SerializeField] private int[] killMilestones;
    [SerializeField] private SkillSelectionManager skillSelectionManager;
    private int currentKillCount;
    private int milestoneIndex;

    public void HandleMonsterDead(MonsterHealth monster)
    {
        monster.OnDead -= HandleMonsterDead;

        currentKillCount++;

        if (currentKillCount >= killMilestones[milestoneIndex])
        {
            milestoneIndex++;

            if (milestoneIndex >= killMilestones.Length)
            {
                GameManager.Instance.ChangeState(GameState.GameClear);
                return;
            }

            skillSelectionManager.OpenSelection();
        }
    }
}