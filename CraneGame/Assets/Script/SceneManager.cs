using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;
using System.Collections;
using UnityEngine.Sprites;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    //[SerializeField] private GameObject enemyPrefab;
    [Header("BallInfo")]
    [SerializeField] private GameObject ballPerfab;
    [SerializeField] private int ballToCreate;

    [Header("Panelinfo")]
    [SerializeField] private PanelManager panel;
    [SerializeField] private int leftPanelCode;
    [SerializeField] private int rightPanelCode;
    [SerializeField] private int leftValue;
    [SerializeField] private int rightValue;
    [SerializeField] private Vector3 pos;//= new Vector3(0,-150,0);



    [Header("UIInfo")]
    [SerializeField] private Image resultBackground;
    [SerializeField] private TMP_Text result_text;
    [SerializeField] private TMP_Text clearscore_text;
    [SerializeField] private TMP_Text score_text;
    [SerializeField] private TMP_Text jiyu_text;
    [SerializeField] private Button nextStageButton;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button titleSelectButton;
    [SerializeField] private TMP_Text ingameClearScore_text;
    [SerializeField] private Image blackCover;
    [SerializeField] private SpriteRenderer[] sr_StartCountDown;
    public SpriteRenderer blackCoverSpriteRenderer;
    [SerializeField] private TMP_Text countDown_text1;
    [SerializeField] private TMP_Text countDown_text2;
    [SerializeField] private TMP_Text countDown_text3;
    [SerializeField] private TMP_Text countDown_textGO;

    [SerializeField] private GameObject jiliangbiao;
    [SerializeField] private bool isMeasuring = false;
    private bool isClear = false;
    private  int gameLevel;
    private string targetHexMap;
    [SerializeField] private int clearScore;
    [SerializeField] private int score;
    public bool gameStart;
    //public TextMeshPro mytext;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        

        //blackCoverSpriteRenderer = blackCover.transform.GetChild(0).GetComponent<SpriteRenderer>();

        Debug.Log("chongqile");

        UIManager.Instance.UpdateScoreText(score);

        GameInit();
        StartCoroutine(StartingCover());

    }


    IEnumerator StartingCover()
    {
        blackCover.gameObject.SetActive(true);
        countDown_text3.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        countDown_text3.gameObject.SetActive(false);
        countDown_text2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        countDown_text2.gameObject.SetActive(false);
        countDown_text1.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        countDown_text1.gameObject.SetActive(false);
        countDown_textGO.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        blackCover.gameObject.SetActive(false);
        countDown_textGO.gameObject.SetActive(false);

        ////blackCoverSpriteRenderer.sprite = sr_StartCountDown[0].sprite;
        //yield return new WaitForSeconds(1f);
        ////blackCoverSpriteRenderer.sprite = sr_StartCountDown[1].sprite;
        //yield return new WaitForSeconds(1f);
        ////blackCoverSpriteRenderer.sprite = sr_StartCountDown[2].sprite;
        //yield return new WaitForSeconds(1f);
        ////blackCoverSpriteRenderer.sprite = sr_StartCountDown[3].sprite;
        //yield return new WaitForSeconds(0.5f);
        //blackCover.gameObject.SetActive(false);
        ////blackCoverSpriteRenderer.gameObject.SetActive(false);

        gameStart = true;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetClearScore()
    {
        return clearScore;
    }

    public bool GetIsGameStart()
    {
        return gameStart;
    }
    public bool GetIsClear()
    {
        return isClear;
    }

    public void ClickOnToNextStage()
    {
        GameManager.Instance.SetGameLevel((GameManager.GameLevel)(gameLevel));//这里里面做了加一减一所以直接带入
        //UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");

        UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene", LoadSceneMode.Single);
    }
    public void ClickOnTryAgainButton()
    {
        //GameManager.Instance.SetGameLevel(GameManager.GameLevel.Level_1);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene", LoadSceneMode.Single);
    }

    public void ClickOnBackToStageSelectButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
    }

    public void ShowResultScreen()
    {
        if(score>= clearScore)
        {
            isClear = true;
            result_text.text = "CLEAR!!";
            clearscore_text.text = "CLEAR SCORE : " + clearScore.ToString() + "m";
            score_text.text = "SCORE : " + score.ToString() + "m";
            jiyu_text.text = "GOODJOB!!";
            nextStageButton.gameObject.SetActive(true);
            //StageSelector.Instance.SetIsStageClear(1, isClear);
            GameManager.Instance.SetIsStageClear(gameLevel, isClear);

        }

        else
        {
            result_text.text = "GAME OVER";
            clearscore_text.text = "CLEAR SCORE : " + clearScore.ToString() + "m";
            score_text.text = "SCORE : " + score.ToString() + "m";
            jiyu_text.text = "TRYAGAIN!";
            tryAgainButton.gameObject.SetActive(true);
        }

        resultBackground.gameObject.SetActive(true);
        result_text.gameObject.SetActive(true);
        clearscore_text.gameObject.SetActive(true);
        score_text.gameObject.SetActive(true);
        jiyu_text.gameObject.SetActive(true);
        titleSelectButton.gameObject.SetActive(true);
    }

    public void GameInit()
    {
        Debug.Log("chongqile");
        score = 0;
        isClear = false;
        resultBackground.gameObject.SetActive(false);
        result_text.gameObject.SetActive(false);
        clearscore_text.gameObject.SetActive(false);
        score_text.gameObject.SetActive(false);
        jiyu_text.gameObject.SetActive(false);
        titleSelectButton.gameObject.SetActive(false);
        tryAgainButton.gameObject.SetActive(false);
        nextStageButton.gameObject.SetActive(false);

        targetHexMap = GameManager.Instance.GetLevelCsvPath();
        // maxTurn = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "B", 4);

         gameLevel = GameManager.Instance.GetGameLevel();
        Debug.Log("gamelevel = "+gameLevel);

        for(int i = 0; i < ballToCreate; i++)
        {
            int randPosX = Random.Range(-25, 25);
            int randPosZ = Random.Range(-25, 25);
            pos = new Vector3 (randPosX, -150, randPosZ);
            GameObject newBall = Instantiate(ballPerfab, pos, Quaternion.identity);

            int randScore = Random.Range(1, 11);
            newBall.GetComponent<Ball>().SetBallScore(randScore);
            Debug.Log("生成了一个新物体！");
        }


        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = new Vector3(0, (-i * 40)-40 , 0);
            string note1 = ((char)('C' + i * 4)).ToString();
            string note2 = ((char)('E' + i * 4)).ToString();
            string note3 = ((char)('D' + i * 4)).ToString();
            string note4 = ((char)('F' + i * 4)).ToString();

            PanelManager newPanel= Instantiate(panel, pos, Quaternion.identity);
            leftPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, note1, 2+gameLevel);
            rightPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap,note2, 2+gameLevel);
            leftValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap,note3, 2+gameLevel);
            rightValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, note4, 2+ gameLevel);

            newPanel.SetLeftPanelCode(leftPanelCode);
            newPanel.SetRightPanelCode(rightPanelCode);
            newPanel.SetLeftPanelValue(leftValue);
            newPanel.SetRightPanelValue(rightValue);

            //Collider col = panel.GetComponent<Collider>();
            //col.isTrigger = true;   // 开启触发器模式
        }

        clearScore = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "U", 2 + gameLevel);
        ingameClearScore_text.text = "ClearScore : " + clearScore;


    }
    public bool GetIsMeasuring() {  return isMeasuring; }

    public void SetIsMeasuring(bool _isMeasuring) {  isMeasuring = _isMeasuring; }
    public int GetLeftPanelCode() { return leftPanelCode; }
    public int GetRightPanelCode() {  return rightPanelCode;}
    public int GetLeftValue() {  return leftValue; }
    public int GetRightValue() {  return rightValue; }
    public void AddScore(int amount)
    {
        score += amount;
        // 可以在这里通知 UI 更新
        UIManager.Instance.UpdateScoreText(score);
    }

    public void SubScore(int amount)
    {
        score -= amount;
        UIManager.Instance.UpdateScoreText(score);
    }
    //else
    //{
    // Debug.LogWarning("没有配置，无法初始化关卡！");
    // }
    //}

}


