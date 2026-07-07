using UnityEngine;
using UnityEngine.UI;

public class GlobalAudioSettings : MonoBehaviour
{
    private const string MuteKey = "GameMuted";

    [Header("Mute Icon - Optional")]
    [SerializeField] private Image muteIconImage;
    [SerializeField] private Sprite soundOnIcon;
    [SerializeField] private Sprite soundOffIcon;

    private void Awake()
    {
        ApplySavedAudioState();
    }

    public void ToggleMute()
    {
        bool isCurrentlyMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;

        SetMute(!isCurrentlyMuted);
    }

    private void SetMute(bool isMuted)
    {
        // قطع یا وصل کردن صدای کل بازی
        AudioListener.volume = isMuted ? 0f : 1f;

        // ذخیره وضعیت صدا
        PlayerPrefs.SetInt(MuteKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // تغییر آیکن
        UpdateMuteIcon(isMuted);
    }

    private void ApplySavedAudioState()
    {
        bool isMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;

        AudioListener.volume = isMuted ? 0f : 1f;

        UpdateMuteIcon(isMuted);
    }

    private void UpdateMuteIcon(bool isMuted)
    {
        if (muteIconImage == null)
            return;

        muteIconImage.sprite = isMuted
            ? soundOffIcon
            : soundOnIcon;
    }
}