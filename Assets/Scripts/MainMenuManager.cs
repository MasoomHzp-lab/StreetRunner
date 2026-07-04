using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Mute UI")]
    [SerializeField] private TMP_Text muteButtonText;

    private bool isMuted;

    private void Start()
    {
        // خواندن وضعیت قبلی صدا
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        ApplyMute();
        UpdateMuteText();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        ApplyMute();
        UpdateMuteText();
    }

    public void ExitGame()
    {
        Debug.Log("Game Closed");

        Application.Quit();
    }

    private void ApplyMute()
    {
        AudioListener.volume = isMuted ? 0f : 1f;
    }

    private void UpdateMuteText()
    {
        if (muteButtonText == null)
            return;

        muteButtonText.text = isMuted ? "UNMUTE" : "MUTE";
    }
}