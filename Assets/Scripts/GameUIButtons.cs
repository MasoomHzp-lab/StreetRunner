using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIButtons : MonoBehaviour
{
    public void RestartGame()
    {
        // برای اطمینان، زمان بازی به حالت عادی برگردد
        Time.timeScale = 1f;

        // بارگذاری دوباره همین Scene
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}