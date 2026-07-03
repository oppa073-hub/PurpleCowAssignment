using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
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

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;

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
                ShowResult("Stage Clear");
                break;

            case GameState.GameOver:
                PauseGame();
                ShowResult("Game Over");
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
    private void ShowResult(string message)
    {
        resultText.text = message;
        resultPanel.SetActive(true);
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
