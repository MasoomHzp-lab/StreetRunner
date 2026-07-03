using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerGameManager : MonoBehaviour
{
    [Header("Speed Settings")]
    public float startSpeed = 8f;
    public float speedIncreasePerStage = 2f;

    [Header("Stage Settings")]
    public int currentStage = 1;
    public int maxStage = 3;

    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    public float CurrentSpeed { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsWin { get; private set; }

    private void Awake()
    {
        CurrentSpeed = startSpeed;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void CompleteStage(int stageNumber)
    {
        if (IsGameOver || IsWin)
            return;

        if (stageNumber != currentStage)
            return;

        if (currentStage < maxStage)
        {
            currentStage++;
            CurrentSpeed += speedIncreasePerStage;

            Debug.Log("Stage Complete. New Stage: " + currentStage + " Speed: " + CurrentSpeed);
        }
        else
        {
            WinGame();
        }
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;
        CurrentSpeed = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("Game Over");
    }

    public void WinGame()
    {
        if (IsWin)
            return;

        IsWin = true;
        CurrentSpeed = 0f;

        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log("You Win");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}