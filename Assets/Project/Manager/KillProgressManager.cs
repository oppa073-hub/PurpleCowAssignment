using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KillProgressManager : MonoBehaviour
{
    [SerializeField] private int[] killMilestones;
    [SerializeField] private SkillSelectionManager skillSelectionManager;
    [SerializeField] private MonsterSpawnManager monsterSpawnManager;
    [SerializeField] private Image progressSlider;
    [SerializeField] private TMP_Text WaveText;
    private int currentKillCount;
    private int milestoneIndex;
    private bool isFinalConditionReached;

    private void Start()
    {
        UpdateWaveUI();
    }

    public void HandleMonsterDead(MonsterHealth monster)
    {
        monster.OnDead -= HandleMonsterDead;

        currentKillCount++;
        UpdateWaveUI();

        if (milestoneIndex >= killMilestones.Length)
            return;

        if (currentKillCount >= killMilestones[milestoneIndex])
        {
            milestoneIndex++;
            UpdateWaveUI();

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
    private void UpdateWaveUI()  //웨이브 UI 업데이트
    {
        if (milestoneIndex >= killMilestones.Length)
        {
            progressSlider.fillAmount = 1f;
            WaveText.text = killMilestones.Length.ToString();
            return;
        }

        int previousTarget = 0;

        if (milestoneIndex > 0)
        {
            previousTarget = killMilestones[milestoneIndex - 1];
        }

        int currentTarget = killMilestones[milestoneIndex];

        int currentWaveKill = currentKillCount - previousTarget;
        int requiredWaveKill = currentTarget - previousTarget;

        progressSlider.fillAmount = (float)currentWaveKill / requiredWaveKill;
        WaveText.text = (milestoneIndex + 1).ToString();
    }
}