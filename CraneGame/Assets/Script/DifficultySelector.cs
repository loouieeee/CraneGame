using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    public StageConfig easyConfig;
    public StageConfig normalConfig;
    public StageConfig hardConfig;

    // Easy 按钮调用
    public void SelectEasy()
    {
        GameManager.Instance.currentConfig = easyConfig;
        SceneManager.LoadScene("InGameScene"); // 切换到游戏场景
    }

    // Normal 按钮调用
    public void SelectNormal()
    {
        GameManager.Instance.currentConfig = normalConfig;
        SceneManager.LoadScene("InGameScene");
    }

    // Hard 按钮调用
    public void SelectHard()
    {
        GameManager.Instance.currentConfig = hardConfig;
        SceneManager.LoadScene("InGameScene");
    }
}
