using UnityEngine;
public enum GameState
{
    Playing,
    SkillSelection,
    GameClear,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState state)
    {
        CurrentState = state;

        switch (state)
        {
            case GameState.Playing:
                ResumeGame();
                break;

            case GameState.SkillSelection:
                PauseGame();
                break;

            case GameState.GameClear:
                PauseGame();
                break;

            case GameState.GameOver:
                Debug.Log("게임 오버");
                PauseGame();
                break;
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
