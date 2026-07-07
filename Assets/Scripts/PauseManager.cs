using UnityEngine;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private RunnerGameManager gameManager;

    private bool isPaused = false;


    private void Start()
    {
        // اطمینان از اینکه بازی با سرعت عادی شروع شود
        Time.timeScale = 1f;

        AudioListener.pause = false;

        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }


    private void Update()
    {
        // وقتی بازی تمام شده Pause باز نشود
        if (gameManager != null &&
            gameManager.IsGameOver)
        {
            return;
        }


        if (EscapePressed())
        {
            TogglePause();
        }
    }


    private bool EscapePressed()
    {
#if ENABLE_INPUT_SYSTEM

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            return true;
        }

#endif


#if ENABLE_LEGACY_INPUT_MANAGER

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }

#endif

        return false;
    }


    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }


    public void PauseGame()
    {
        isPaused = true;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        // توقف کامل بازی
        Time.timeScale = 0f;

        // توقف صدا
        AudioListener.pause = true;
    }


    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // ادامه بازی
        Time.timeScale = 1f;

        // ادامه صدا
        AudioListener.pause = false;
    }


    public void RestartGame()
    {
        // خیلی مهم:
        // قبل از Reload باید زمان را برگردانیم
        Time.timeScale = 1f;

        AudioListener.pause = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }


    public void ExitGame()
    {
        // قبل از خروج وضعیت Pause را پاک کن
        Time.timeScale = 1f;

        AudioListener.pause = false;

#if UNITY_EDITOR

        Debug.Log("EXIT GAME");

#else

        Application.Quit();

#endif
    }
}