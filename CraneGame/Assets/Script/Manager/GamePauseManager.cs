using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // 拖入暂停菜单UI

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);  // 显示UI
        Time.timeScale = 0f;          // 暂停游戏
        isPaused = true;
        AudioListener.pause = true;   // 暂停声音
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // 隐藏UI
        Time.timeScale = 1f;          // 恢复游戏
        isPaused = false;
        AudioListener.pause = false;  // 恢复声音
    }
}
