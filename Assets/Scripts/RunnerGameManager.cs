using System.Collections;
using UnityEngine;
using TMPro;

public class RunnerGameManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip hitSound;

    [Header("Coin System")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text highScoreText;

    private int collectedCoins = 0;
    private int highScore = 0;

    private const int totalCoins = 21;
    private const string HighScoreKey = "HighScore";

    [Header("Stage Speeds")]
    [SerializeField] private float stage1Speed = 8f;
    [SerializeField] private float stage2Speed = 10f;
    [SerializeField] private float stage3Speed = 12f;

    [Header("Stage Message UI")]
    [SerializeField] private GameObject stageMessagePanel;
    [SerializeField] private TMP_Text stageMessageText;
    [SerializeField] private float stageMessageDuration = 2f;

    [Header("Win / Lose UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Lose Settings")]
    [SerializeField] private float losePanelDelay = 2f;

    private int currentStage = 1;

    private Coroutine stageMessageCoroutine;

    public float CurrentSpeed { get; private set; }

    public bool IsGameOver { get; private set; }

    private void Start()
    {
        CurrentSpeed = stage1Speed;
        IsGameOver = false;

        collectedCoins = 0;

        highScore = PlayerPrefs.GetInt(
            HighScoreKey,
            0
        );

        UpdateCoinText();
        UpdateHighScoreText();

        // مخفی کردن پیام Stage
        if (stageMessagePanel != null)
        {
            stageMessagePanel.SetActive(false);
        }

        // مخفی کردن پنل برد
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        // مخفی کردن پنل باخت
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }

    public void CompleteStage()
    {
        if (IsGameOver)
            return;

        // پایان Stage 1
        if (currentStage == 1)
        {
            currentStage = 2;
            CurrentSpeed = stage2Speed;

            ShowStageMessage(2);

            return;
        }

        // پایان Stage 2
        if (currentStage == 2)
        {
            currentStage = 3;
            CurrentSpeed = stage3Speed;

            ShowStageMessage(3);

            return;
        }

        // پایان Stage 3
        if (currentStage == 3)
        {
            WinGame();
        }
    }

    private void ShowStageMessage(int newStage)
    {
        if (stageMessageCoroutine != null)
        {
            StopCoroutine(stageMessageCoroutine);
        }

        stageMessageCoroutine = StartCoroutine(
            ShowStageMessageRoutine(newStage)
        );
    }

    private IEnumerator ShowStageMessageRoutine(int newStage)
    {
        if (stageMessagePanel == null || stageMessageText == null)
        {
            yield break;
        }

        stageMessageText.text =
    "Congratulations!\nYou have reached Stage " + newStage + "!";

        stageMessagePanel.SetActive(true);

        yield return new WaitForSeconds(stageMessageDuration);

        stageMessagePanel.SetActive(false);

        stageMessageCoroutine = null;
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        // توقف حرکت Player
        CurrentSpeed = 0f;

        // نمایش پنل با تأخیر
        StartCoroutine(ShowLosePanelRoutine());
    }

    private IEnumerator ShowLosePanelRoutine()
    {
        // صبر برای پخش انیمیشن افتادن
        yield return new WaitForSeconds(losePanelDelay);

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
    }

    private void WinGame()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        // توقف Player
        CurrentSpeed = 0f;

        // نمایش پنل برد
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    public void CollectCoin()
{
    collectedCoins++;

        if (sfxSource != null && coinSound != null)
    {
        sfxSource.PlayOneShot(coinSound);
    }

    // اگر رکورد جدید ثبت شد
    if (collectedCoins > highScore)
    {
            highScore = collectedCoins;

            PlayerPrefs.SetInt(
                HighScoreKey,
                highScore
            );

            PlayerPrefs.Save();

            UpdateHighScoreText();
        }

        UpdateCoinText();
    }


    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text =
                "COINS: " +
                collectedCoins +
                " / " +
                totalCoins;
        }
    }


    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text =
                "HIGH SCORE: " +
                highScore;
        }
    }

    public void PlayHitSound()
    {
        if (sfxSource != null && hitSound != null)
        {
            sfxSource.PlayOneShot(hitSound);
        }
    }
}