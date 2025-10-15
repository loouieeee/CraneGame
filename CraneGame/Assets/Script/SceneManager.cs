using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;
using System.Collections;
using UnityEngine.Sprites;
using UnityEditor.Experimental.GraphView;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    [SerializeField] private int gimmickOffset;
    [SerializeField] private int panelOffset;
    //[SerializeField] private GameObject enemyPrefab;
    [Header("BallInfo")]
    [SerializeField] private GameObject ballPerfab;
    [SerializeField] private int ballToCreate;

    [Header("Panelinfo")]
    [SerializeField] private PanelManager panel;
    [SerializeField] private int panelType;
    [SerializeField] private int numOfPanel;
    [SerializeField] private int leftPanelCode;
    [SerializeField] private int rightPanelCode;
    [SerializeField] private int leftValue;
    [SerializeField] private int rightValue;
    [SerializeField] private int upperLeftPanelCode;
    [SerializeField] private int upperRightPanelCode;
    [SerializeField] private int lowerLeftPanelCode;
    [SerializeField] private int lowerRightPanelCode;
    [SerializeField] private int upperLeftPanelValue;
    [SerializeField] private int upperRightPanelValue;
    [SerializeField] private int lowerLeftPanelValue;
    [SerializeField] private int lowerRightPanelValue;

    [SerializeField] private Vector3 pos;//= new Vector3(0,-150,0);

    [Header("GimmickInfo")]
    [SerializeField] private int numOfGimmick;
    [SerializeField] private Gimmick_Sickle rotarySickle;
    [SerializeField] private Gimmick_SlideStick slideStick;
    [SerializeField] private Gimmick_SpikeBall spikeBall;
    [SerializeField] private GameObject spikeBallPerfab;
    [SerializeField] private GameObject slideStickPerfab;
    [SerializeField] private GameObject rotarySicklePerfab;
    [SerializeField] private int gimmickType;
    [SerializeField] private int gimmickPosType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int subFinalScoreValue;
    [SerializeField] private int subBallScoreValue;
    [SerializeField] private bool isRotate;
    [SerializeField] private bool isClockwise;
    [SerializeField] private int rotateSpeed;
    [SerializeField] private int spikeBallPositionCode;
    [SerializeField] private float spikeBallSize;
    [SerializeField] private int slideStickPositionCode;
    [SerializeField] private int slideSitckMoveTime;
    [SerializeField] private Transform gimmickPos1;
    [SerializeField] private Transform gimmickPos2;
    [SerializeField] private Transform gimmickPos3;
    [SerializeField] private Transform gimmickPos4;
    [SerializeField] private Transform gimmickPos5;


    [Header("UIInfo")]
    [SerializeField] private GameObject resultUI;
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

    [Header("MeasureInfo")]
    [SerializeField] private GameObject measurePerfab;
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
        Init_UI();

        targetHexMap = GameManager.Instance.GetLevelCsvPath();
        // maxTurn = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "B", 4);

        gameLevel = GameManager.Instance.GetGameLevel();
        Debug.Log("gamelevel = " + gameLevel);

        CreateBall();

        CreatePanel();

        CreateGimmick();

        panelOffset = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 10);
        gimmickOffset = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 12);
        clearScore = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 7);
        ingameClearScore_text.text = "ClearScore : " + clearScore;
    }

    private void CreateGimmick()
    {
        numOfGimmick = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 5);
        for (int i = 0; i < numOfGimmick; i++)
        {
            gimmickType = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "H", 5 + 4 * i);
            if (gimmickType == 1)//spikeball
            {
                gimmickPosType = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "N", 5 + 4 * i);
                //pos = gimmickPos5.position - new Vector3(0, i * 40, 0);
                switch (gimmickPosType)
                {
                    case 1: pos = gimmickPos1.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0); break;
                    case 2: pos = gimmickPos2.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0); break;
                    case 3: pos = gimmickPos3.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0); break;
                    case 4: pos = gimmickPos4.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0); break;
                    default: break;
                }
                spikeBallSize = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "O", 5 + 4 * i);
                Gimmick_SpikeBall newSpikeBall = Instantiate(spikeBall, pos, Quaternion.identity);
            }
            else if (gimmickType == 2)//rotarysickle
            {
                pos = gimmickPos5.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0);
                Gimmick_Sickle newRotarySickle = Instantiate(rotarySickle, pos, Quaternion.identity);
                newRotarySickle.SetIsRotate((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "K", 5 + 4 * i));
                newRotarySickle.SetIsRotateClockwise((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "L", 5 + 4 * i));
                newRotarySickle.SetRotateSpeed((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "M", 5 + 4 * i));
                newRotarySickle.SetSubFinalScore((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "I", 5 + 4 * i));
            }

            else if (gimmickType == 3)//slidestick
            {
                gimmickPosType = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "P", 5 + 4 * i);
                //pos = gimmickPos5.position - new Vector3(0, i * 40, 0);
                Gimmick_SlideStick newSlideStick = null; ;
                switch (gimmickPosType)
                {
                    case 1:
                        {
                            pos = gimmickPos2.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0);
                            newSlideStick = Instantiate(slideStick, pos, Quaternion.Euler(0, 180, 90));
                            break;
                        }
                    case 2:
                        {
                            pos = gimmickPos4.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0);
                            newSlideStick = Instantiate(slideStick, pos, Quaternion.Euler(0, 180, 90));
                            break;
                        }
                    case 3:
                        {
                            pos = gimmickPos4.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0);
                            newSlideStick = Instantiate(slideStick, pos, Quaternion.Euler(0, 270, 90));
                            break;
                        }
                    case 4:
                        {
                            pos = gimmickPos3.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0);
                            newSlideStick = Instantiate(slideStick, pos, Quaternion.Euler(0, 270, 90));
                            break;

                        }
                    case 5:
                        {
                            pos = gimmickPos3.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0);
                            newSlideStick = Instantiate(slideStick, pos, Quaternion.Euler(0, 0, 90));
                            break;
                        }
                    case 6:
                        {
                            pos = gimmickPos1.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0);
                            newSlideStick = Instantiate(slideStick, pos, Quaternion.Euler(0, 0, 90));
                            break;
                        }
                    case 7:
                        {
                            pos = gimmickPos1.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0);
                            newSlideStick = Instantiate(slideStick, pos, Quaternion.Euler(0, 90, 90));
                            break;
                        }
                    case 8:
                        {
                            pos = gimmickPos2.position - new Vector3(0, i * gimmickOffset + 2 * gimmickOffset, 0);
                            newSlideStick = Instantiate(slideStick, pos, Quaternion.Euler(0, 90, 90));
                            break;
                        }
                    default:
                        Debug.LogWarning($"Unexpected gimmickPosType: {gimmickPosType}");
                        break;
                }
                if (newSlideStick != null)
                {
                    newSlideStick.SetXMoveSpeed((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "R", 5 + 4 * i));
                    newSlideStick.SetMoveTime((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "Q", 5 + 4 * i));
                    newSlideStick.SetSubBallScore((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "J", 5 + 4 * i));
                }
            }
        }
    }

    private void CreateBall()
    {
        for (int i = 0; i < ballToCreate; i++)
        {
            int randPosX = Random.Range(-25, 25);
            int randPosZ = Random.Range(-25, 25);
            pos = new Vector3(randPosX, -150, randPosZ);
            GameObject newBall = Instantiate(ballPerfab, pos, Quaternion.identity);

            int randScore = Random.Range(1, 11);
            newBall.GetComponent<Ball>().SetBallScore(randScore);
            Debug.Log("生成了一个新物体！");
        }
    }

    private void Init_UI()
    {
        resultBackground.gameObject.SetActive(false);
        result_text.gameObject.SetActive(false);
        clearscore_text.gameObject.SetActive(false);
        score_text.gameObject.SetActive(false);
        jiyu_text.gameObject.SetActive(false);
        titleSelectButton.gameObject.SetActive(false);
        tryAgainButton.gameObject.SetActive(false);
        nextStageButton.gameObject.SetActive(false);
    }

    private void CreatePanel()
    {
        numOfPanel = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 3);
        //numOfPanel = 3;
        for (int i = 0; i < numOfPanel; i++)
        {
            panelType = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "H", 4 * i + 3);
            Vector3 pos = new Vector3(0, -i * panelOffset - panelOffset, 0);
            PanelManager newPanel = Instantiate(panel, pos, Quaternion.identity);


            newPanel.SetPanelPartsType(panelType);
            if (panelType == 2)
            {
                //string note1 = ((char)('C' + i * 4)).ToString();
                //string note2 = ((char)('E' + i * 4)).ToString();xx    
                //string note3 = ((char)('D' + i * 4)).ToString();
                //string note4 = ((char)('F' + i * 4)).ToString();

                leftPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "I", 4 * i + 3);
                leftValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "J", 4 * i + 3);
                rightPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "K", 4 * i + 3);
                rightValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "L", 4 * i + 3);

                newPanel.SetLeftPanelCode(leftPanelCode);
                newPanel.SetRightPanelCode(rightPanelCode);
                newPanel.SetLeftPanelValue(leftValue);
                newPanel.SetRightPanelValue(rightValue);
            }

            else if (panelType == 4)
            {
                upperLeftPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "I", 4 * i + 3);
                upperLeftPanelValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "J", 4 * i + 3);
                upperRightPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "K", 4 * i + 3);
                upperRightPanelValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "L", 4 * i + 3);

                lowerLeftPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "M", 4 * i + 3);
                lowerLeftPanelValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "N", 4 * i + 3);
                lowerRightPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "O", 4 * i + 3);
                lowerRightPanelValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "P", 4 * i + 3);

                newPanel.SetUpperLeftPanelCode(upperLeftPanelCode);
                newPanel.SetUpperLeftPanelValue(upperLeftPanelValue);
                newPanel.SetUpperRightPanelCode(upperRightPanelCode);
                newPanel.SetUpperRightPanelValue(upperRightPanelValue);

                newPanel.SetLowerLeftPanelCode(lowerLeftPanelCode);
                newPanel.SetLowerLeftPanelValue(lowerLeftPanelValue);
                newPanel.SetLowerRightPanelCode(lowerRightPanelCode);
                newPanel.SetLowerRightPanelValue(lowerRightPanelValue);
            }

            newPanel.SetIsRotate((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "Q", 4 * i + 3));
            newPanel.SetIsRotateClockwise((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "R", 4 * i + 3));
            newPanel.SetRotateSpeed((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "S", 4 * i + 3));

            //Collider col = panel.GetComponent<Collider>();
            //col.isTrigger = true;   // 开启触发器模式
        }
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


