using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelector : MonoBehaviour
{
    public static StageSelector Instance;
    //public StageConfig easyConfig;
    //public StageConfig normalConfig;
    //public StageConfig hardConfig;
    [SerializeField] private bool[] isStageClear = new bool[20] ;
    [SerializeField] private Button[] stageButton;

    [SerializeField] private Image grayBackground;
    [SerializeField] private Image explainHowToPlayBackground;
    [SerializeField] private TMP_Text sousa_text;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject Scroll;

    [SerializeField] private SpriteRenderer[] sr_HowToPlay;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Init_StageSelectScene();
        Init_StageButton();
        CheckIsStageCanPlay(isStageClear);
    }

    public void CheckIsStageCanPlay(bool[] isStageClear)
    {
        for(int i = 0; i < isStageClear.Length; i++)
        {
            if (GameManager.Instance.GetIsStageClear(i + 1))
            {
                stageButton[i].enabled = true;
            }
        }
    }

    public void Init_StageButton()
    {
        for (int i = 0;i < stageButton.Length;i++)
        {
            stageButton[i].enabled = false;
        }
    }
    //public void SetIsStageClear(int _stageNum, bool _isClear)
    //{
    //    if (_stageNum < 1 || _stageNum > isStageClear.Length)
    //    {
    //        Debug.LogWarning("无效的关卡编号：" + _stageNum);
    //        return;
    //    }

    //    isStageClear[_stageNum - 1] = _isClear; // 数组是0开始的，所以要-1
    //}
    public void Init_StageSelectScene()
    {
        grayBackground.gameObject.SetActive(false);
        explainHowToPlayBackground.gameObject.SetActive(false);
        sousa_text.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        Scroll.gameObject.SetActive(false);
    }

    public void ClickOnHowToPlayButton()
    {
        grayBackground.gameObject.SetActive(true);
        explainHowToPlayBackground.gameObject.SetActive(true);
        sousa_text.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        Scroll.gameObject.SetActive(true);
    }

    public void ClickOnCloseButton()
    {
        grayBackground.gameObject.SetActive(false);
        explainHowToPlayBackground.gameObject.SetActive(false);
        sousa_text.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        Scroll.gameObject.SetActive(false);

    }

    // Easy 按钮调用
    //public void SelectLevel1()
    //{
    //    //GameManager.Instance.currentConfig = easyConfig;
    //    GameManager.Instance.SetGameLevel(GameManager.GameLevel.Level_1);
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene"); // 切换到游戏场景
    //}

    //// Normal 按钮调用
    //public void SelectLevel2()
    //{
    //    //GameManager.Instance.currentConfig = normalConfig;
    //    GameManager.Instance.SetGameLevel(GameManager.GameLevel.Level_2);
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene");
    //}

    //// Hard 按钮调用
    //public void SelectLevel3()
    //{
    //    //GameManager.Instance.currentConfig = hardConfig;
    //    GameManager.Instance.SetGameLevel(GameManager.GameLevel.Level_3);
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene");
    //}

    //public void SelectLevel4()
    //{
    //    GameManager.Instance.SetGameLevel(GameManager.GameLevel.Level_4);
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene");
    //}

    //public void SelectLevel5()
    //{
    //    GameManager.Instance.SetGameLevel(GameManager.GameLevel.Level_5);
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene");
    //}

    //public void SelectLevel6()
    //{
    //    GameManager.Instance.SetGameLevel(GameManager.GameLevel.Level_6);
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene");
    //}

    public void ClickOnBackToTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }

    public void SelectLevel(int levelNum)
    {
        GameManager.Instance.SetGameLevel((GameManager.GameLevel)(levelNum - 1));
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene", LoadSceneMode.Single);
    }
}
