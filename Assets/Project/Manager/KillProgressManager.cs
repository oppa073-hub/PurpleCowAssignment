using UnityEngine;

public class KillProgressManager : MonoBehaviour
{
    [SerializeField] private int[] killMilestones;
    [SerializeField] private SkillSelectionManager skillSelectionManager;
    [SerializeField] private MonsterSpawnManager monsterSpawnManager;
    private int currentKillCount;
    private int milestoneIndex;
    private bool isFinalConditionReached;

    public void HandleMonsterDead(MonsterHealth monster)
    {
        monster.OnDead -= HandleMonsterDead;

        currentKillCount++;

        if (milestoneIndex >= killMilestones.Length)
            return;

        if (currentKillCount >= killMilestones[milestoneIndex])
        {
            milestoneIndex++;

            if (milestoneIndex >= killMilestones.Length)
            {
                isFinalConditionReached = true;
                monsterSpawnManager.StopSpawn();
                CheckStageClear();
                return;
            }

            skillSelectionManager.OpenSelection();
        }
    }
    public void CheckStageClear()
    {
        if (!isFinalConditionReached) return;

        if (monsterSpawnManager.ActiveMonsterCount <= 0)
        {
            GameManager.Instance.ChangeState(GameState.GameClear);
        }
    }
}