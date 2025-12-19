using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class StageSelector : MonoBehaviour
{
    public static StageSelector Instance;

    //[SerializeField] private bool[] isStageClear;
    [SerializeField] private Button[] stageButton;
    [SerializeField] private int stageButtonAmount;
    public Image background;
    public Vector3 offset;


    [SerializeField] private Image grayBackground;
    [SerializeField] private Image explainHowToPlayBackground;
    [SerializeField] private TMP_Text sousa_text;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject Scroll;
    [SerializeField] private Sprite[] stageIcon;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonPos1;
    [SerializeField] private Transform buttonPos2;

    [SerializeField] private Image stageSelectBackground;
    [SerializeField] private TMP_Text stage_text;
    [SerializeField] private TMP_Text clearScore_text;
    [SerializeField] private Button tryButton;
    [SerializeField] private Button backButton;

    [SerializeField] private Sprite[] sr_HowToPlay;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvas1;

    public List<StageButtonText> buttons;

    [SerializeField] private int stageNum;
    private int gameLevel;
    private string targetHexMap;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Init_StageSelectScene();
        Init_StageButton();
        CheckIsStageCanPlay();
        //AudioManager.Instance.StopBGM();
        //AudioManager.Instance.PlayBGM(AudioManager.Instance.StageSelectBGM);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("点击屏幕！");
            AudioManager.Instance.PlaySE(AudioManager.Instance.clickSE);
        }
    }

    private void LateUpdate()
    {
        background.transform.position = Camera.main.transform.position + offset;
        
    }

    //public void CheckIsStageCanPlay(bool[] isStageClear)
    //{
    //    for (int i = 0; i < isStageClear.Length; i++)
    //    {
    //        if (GameManager.Instance.GetIsStageClear(i + 1))
    //        {
    //            //stageButton[i].enabled = true;
    //            //buttons[i].enabled = true;
    //            Button btn = buttons[i].GetComponent<Button>();
    //            btn.enabled = true;

    //            Image img = buttons[i].GetComponent<Image>();
    //            img.sprite = stageIcon[1];

    //        }
    //    }
    //}

    //public void CheckIsStageCanPlay(bool[] isStageClear)
    //{
    //    int playableIndex = -1;
    //    // 第一关直接开放
    //    Button firstButton = buttons[0].GetComponent<Button>();
    //    firstButton.enabled = true;
    //    buttons[0].GetComponent<Image>().sprite = stageIcon[1];

    //    for (int i = 1; i < isStageClear.Length; i++)
    //    {
    //        if (!isStageClear[i])   // 第一个 false 就是下一个可玩关卡
    //        {
    //            playableIndex = i;
    //            break;
    //        }
    //        if (GameManager.Instance.GetIsStageClear(i))
    //        {
    //            Button btn = buttons[i].GetComponent<Button>();
    //            btn.enabled = true;

    //            Image img = buttons[i].GetComponent<Image>();
    //            img.sprite = stageIcon[2];
    //        }
    //    }
    //}

    //public void CheckIsStageCanPlay(bool[] isStageClear)
    //{

    //    // 第一关先开放
    //    Button firstButton = buttons[0].GetComponent<Button>();
    //    firstButton.enabled = true;

    //    // 判断第一关是否通关（如果第二关可玩了，第一关肯定是 clear）
    //    if (isStageClear[0])
    //        buttons[0].GetComponent<Image>().sprite = stageIcon[2]; // 已通关图标
    //    else
    //        buttons[0].GetComponent<Image>().sprite = stageIcon[1]; // 可玩图标

    //    int playableIndex = -1;

    //    // 找到下一个可玩的关卡（第一个 false）
    //    for (int i = 0; i < isStageClear.Length; i++)
    //    {
    //        if (!GameManager.Instance.GetIsStageClear(i))   // 第一个 false 就是下一个要开放的关卡
    //        {
    //            playableIndex = i;
    //            break;
    //        }
    //    }

    //    // 遍历每个关卡设置状态
    //    for (int i = 0; i < isStageClear.Length; i++)
    //    {
    //        Button btn = buttons[i].GetComponent<Button>();
    //        Image img = buttons[i].GetComponent<Image>();

    //        if (GameManager.Instance.GetIsStageClear(i)) // ★ 使用 GameManager 判断 clear
    //        {
    //            // 已通关 → 可点击 + 使用通关 icon
    //            btn.enabled = true;
    //            img.sprite = stageIcon[2];
    //        }
    //        else if (i == playableIndex) // ★ 下一个可玩的关卡
    //        {
    //            btn.enabled = true;
    //            img.sprite = stageIcon[1];
    //        }
    //        else
    //        {
    //            // 其他还没到的关卡，不可玩
    //            btn.enabled = false;
    //            img.sprite = stageIcon[0];

    //        }
    //    }
    //}

    public void CheckIsStageCanPlay()
    {
        int stageCount = buttons.Count;

        // 1. 找到第一个未通关的关卡（下一个可玩关卡）
        int playableIndex = -1;
        for (int i = 0; i < stageCount; i++)
        {
            if (!GameManager.Instance.GetIsStageClear(i + 1))
            {
                playableIndex = i;
                break;
            }
        }

        // 如果所有关卡都通关了，就不存在 playableIndex
        if (playableIndex == -1)
            playableIndex = stageCount - 1;

        // 2. 设置每个关卡按钮状态
        for (int i = 0; i < stageCount; i++)
        {
            Button btn = buttons[i].GetComponent<Button>();
            Image img = buttons[i].GetComponent<Image>();

            bool isClear = GameManager.Instance.GetIsStageClear(i + 1);

            if (isClear)
            {
                // 已通关 → 可点 + 通关图标
                btn.enabled = true;
                img.sprite = stageIcon[2];   // 通关图标
            }
            else if (i == playableIndex)
            {
                // 下一个可玩的关卡 → 可点 + 可玩图标
                btn.enabled = true;
                img.sprite = stageIcon[1];   // 可玩图标
            }
            else
            {
                // 其他关卡 → 不可点 + 锁图标
                btn.enabled = false;
                img.sprite = stageIcon[0];   // 锁图标
            }
        }
    }



    public void Init_StageButton()
    {
        //for (int i = 0; i < stageButton.Length; i++)
        //{
        //   // GameObject button;
        //    if (stageButton.Length % 2 == 0)
        //    {
        //        button =  Instantiate(buttonPrefab, stageButton[0].transform.position - new Vector3(0, 450, 0),Quaternion.identity);

        //    }

        //    else
        //    {
        //        button = Instantiate(buttonPrefab, stageButton[1].transform.position - new Vector3(0, 450, 0), Quaternion.identity);

        //    }

        //    Image img = button.GetComponent<Image>();
        //    img.preserveAspect = true;
        //    img.sprite = stageIcon[0];
        //    img.transform.localScale *= 3;
        //stageButton[i].enabled = false;
        //Image img = stageButton[i].GetComponent<Image>();
        //var text = stageButton[i].GetComponentInChildren<TextMeshPro>();
        //text.gameObject.SetActive(false);
        //img.preserveAspect = true;
        //img.sprite = stageIcon[0];
        //img.transform.localScale *= 3;
        //}

        //Canvas canvas = FindObjectOfType<Canvas>();
        Transform parent = canvas.transform;
        Transform parent1 = canvas.transform.Find("button");
        buttons = new List<StageButtonText>();

        for (int i = 0; i < stageButtonAmount; i++)
        {
            GameObject button;

            Vector2 offset = new Vector2(0, -300); // UI 间距（你自己调）

            button = Instantiate(buttonPrefab, parent, false);

            RectTransform btnRT = button.GetComponent<RectTransform>();


            if (i % 2 == 0)
            {
                btnRT.anchoredPosition =
                    buttonPos1.GetComponent<RectTransform>().anchoredPosition + offset * i;
            }
            else
            {
                btnRT.anchoredPosition =
                    buttonPos2.GetComponent<RectTransform>().anchoredPosition + offset * i;
            }

            Image img = button.GetComponent<Image>();
            img.preserveAspect = true;
            img.sprite = stageIcon[0];
            //img.transform.localScale *= 4;
            btnRT.sizeDelta = new Vector2(150, 150); // 你要的大小

            StageButtonText buttonOnStage = button.GetComponent<StageButtonText>();
            buttonOnStage.SetText((i+1).ToString());
            //buttonOnStage.enabled = false;
            buttons.Add(buttonOnStage);

            Button btn = buttonOnStage.GetComponent<Button>();
            int index = i + 1; // 必须这样存一份
            btn.onClick.AddListener(() => ClickOnStageButton(index));
            btn.enabled = false;

            button.transform.SetParent(parent1, false);
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
        canvas.gameObject.SetActive(true);
        canvas1.gameObject.SetActive(false);
        grayBackground.gameObject.SetActive(false);
        explainHowToPlayBackground.gameObject.SetActive(false);
        sousa_text.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        Scroll.gameObject.SetActive(false);
    }

    public void ClickOnHowToPlayButton()
    {
        Camera.main.gameObject.GetComponent<CameraDrag>().SetIsActing(true);

        grayBackground.gameObject.SetActive(true);
        explainHowToPlayBackground.gameObject.SetActive(true);
        sousa_text.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        Scroll.gameObject.SetActive(true);
    }

    public void ClickOnCloseButton()
    {
        Camera.main.gameObject.GetComponent<CameraDrag>().SetIsActing(false);

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

    public void SelectLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene", LoadSceneMode.Single);
    }

    public void ClickOnStageButton(int _levelNum)
    {
        canvas.gameObject.SetActive(false);
        canvas1.gameObject.SetActive(true);
        Camera.main.gameObject.GetComponent<CameraDrag>().SetIsActing(true);   
        stageNum = _levelNum;
        stageSelectBackground.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        stage_text.gameObject.SetActive(true);
        stage_text.text = "STAGE " + stageNum.ToString();
        GameManager.Instance.SetGameLevel((GameManager.GameLevel)(_levelNum));

        gameLevel = GameManager.Instance.GetGameLevel();
        targetHexMap = GameManager.Instance.GetLevelCsvPath();
        // maxTurn = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "B", 4);

        clearScore_text.gameObject.SetActive(true);
        clearScore_text.text = "Clear Score : " + CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 7).ToString();
        tryButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void ClickOnBackButton()
    {
        canvas.gameObject.SetActive(true);
        canvas1.gameObject.SetActive(false);

        Camera.main.gameObject.GetComponent<CameraDrag>().SetIsActing(false);

        stageSelectBackground.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        stage_text.gameObject.SetActive(false);
        clearScore_text.gameObject.SetActive(false);
        tryButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

    }
}
