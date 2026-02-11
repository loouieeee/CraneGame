using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using static UnityEditor.PlayerSettings;
using System.Collections;
using UnityEngine.Sprites;
//using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using UnityEngine.Splines;
using Unity.VisualScripting;
using static GameManager;
using NUnit;

public class IngameManager : MonoBehaviour
{
    public static IngameManager Instance; // シングルトンのインスタンス
    //[SerializeField] private GameObject enemyPrefab; // 敵プレハブ（未使用）

    [SerializeField] private GameObject Crane;
    [SerializeField] private GameObject anonPerfab;

    [Header("BallInfo")]
    [SerializeField] private GameObject ballPerfab; // ボールのプレハブ
    [SerializeField] private int ballToCreate; // 生成するボールの数
    public int minBallScore;
    public int maxBallScore;

    [Header("Panelinfo")]
    [SerializeField] private int panelPos_Y;
    [SerializeField] private float panelOffset; // パネルのオフセット
    [SerializeField] private GameObject panelobj;
    [SerializeField] private PanelManager panel; // パネルマネージャー
    [SerializeField] private int panelType; // パネルの種類
    [SerializeField] private int numOfPanel; // パネルの数
    [SerializeField] private int leftPanelCode; // 左側パネルのコード
    [SerializeField] private int rightPanelCode; // 右側パネルのコード
    [SerializeField] private int leftValue; // 左パネルの値
    [SerializeField] private int rightValue; // 右パネルの値
    [SerializeField] private int upperLeftPanelCode; // 左上パネルコード
    [SerializeField] private int upperRightPanelCode; // 右上パネルコード
    [SerializeField] private int lowerLeftPanelCode; // 左下パネルコード
    [SerializeField] private int lowerRightPanelCode; // 右下パネルコード
    [SerializeField] private int upperLeftPanelValue; // 左上パネル値
    [SerializeField] private int upperRightPanelValue; // 右上パネル値
    [SerializeField] private int lowerLeftPanelValue; // 左下パネル値
    [SerializeField] private int lowerRightPanelValue; // 右下パネル値

    [SerializeField] private Vector3 pos; // パネル配置位置

    [Header("GimmickInfo")]
    [SerializeField] private int gimmickPos_Y;
    [SerializeField] private float gimmickOffset; // ギミックのオフセット
    [SerializeField] private int numOfGimmick; // ギミックの数
    //[SerializeField] private Gimmick_Sickle rotarySickle; // 回転鎌ギミック
    //[SerializeField] private Gimmick_SlideStick slideStick; // スライドスティックギミック
    //[SerializeField] private Gimmick_SpikeBall spikeBall; // スパイクボールギミック
    //[SerializeField] private GameObject spikeBallPrefab;
    //[SerializeField] private GameObject slideStickPrefab;
    //[SerializeField] private GameObject rotarySicklePrefab;
    [SerializeField] private GameObject spikeBallPerfab; // スパイクボールプレハブ
    [SerializeField] private GameObject slideStickPerfab; // スライドスティックプレハブ
    [SerializeField] private GameObject rotarySicklePerfab; // 回転鎌プレハブ
    [SerializeField] private int gimmickType; // ギミックの種類
    [SerializeField] private int gimmickPosType; // ギミック配置タイプ
    [SerializeField] private float moveSpeed; // ギミック移動速度
    [SerializeField] private int subFinalScoreValue; // サブ最終スコア値
    [SerializeField] private int subBallScoreValue; // サブボールスコア値
    [SerializeField] private bool isRotate; // 回転するか
    [SerializeField] private bool isClockwise; // 時計回りか
    [SerializeField] private int rotateSpeed; // 回転速度
    [SerializeField] private int spikeBallPositionCode; // スパイクボール位置コード
    [SerializeField] private float spikeBallSize; // スパイクボールサイズ
    [SerializeField] private int slideStickPositionCode; // スライドスティック位置コード
    [SerializeField] private int slideSitckMoveTime; // スライドスティック移動時間
    [SerializeField] private Transform gimmickPos1; // ギミック位置1
    [SerializeField] private Transform gimmickPos2; // ギミック位置2
    [SerializeField] private Transform gimmickPos3; // ギミック位置3
    [SerializeField] private Transform gimmickPos4; // ギミック位置4
    [SerializeField] private Transform gimmickPos5; // ギミック位置5

    [Header("UIInfo")]
    [SerializeField] private GameObject resultUI; // リザルトUI
    [SerializeField] public GameObject joystick;
    [SerializeField] private GameObject joystickUI;
    [SerializeField] private Image resultBackground; // リザルト背景
    [SerializeField] private Sprite[] resultBackSR;
    [SerializeField] private TMP_Text result_text; // リザルトテキスト
    [SerializeField] private TMP_Text clearscore_text; // クリアスコアテキスト
    [SerializeField] private TMP_Text score_text; // スコアテキスト
    [SerializeField] private TMP_Text jiyu_text; // 自由テキスト
    [SerializeField] private Button nextStageButton; // 次のステージボタン
    [SerializeField] private Button tryAgainButton; // 再挑戦ボタン
    [SerializeField] private Button titleSelectButton; // タイトル選択ボタン
    [SerializeField] private TMP_Text ingameClearScore_text; // ゲーム内クリアスコア
    [SerializeField] private Image blackCover; // 黒カバー
    [SerializeField] private SpriteRenderer[] sr_StartCountDown; // スタートカウントダウン用スプライト
    public SpriteRenderer blackCoverSpriteRenderer; // 黒カバースプライトレンダラー
    [SerializeField] private TMP_Text countDown_text1; // カウントダウンテキスト1
    [SerializeField] private TMP_Text countDown_text2; // カウントダウンテキスト2
    [SerializeField] private TMP_Text countDown_text3; // カウントダウンテキスト3
    [SerializeField] private TMP_Text countDown_textGO; // GOテキスト
    [SerializeField] private Button pauseButton;

    public SpriteRenderer background;
    public Vector3 offset;

    [Header("MeasureInfo")]
    [SerializeField] private GameObject measurePerfab; // 測定プレハブ
    [SerializeField] private GameObject liangtongPerfab;
    [SerializeField] private Transform measurePerfabPosition; // 測定プレハブ配置位置
    [SerializeField] private bool isMeasuring = false; // 測定中か
    [SerializeField] private FloatingText floatingText; // 浮遊テキスト
    [SerializeField] private int fTRandPosXmin; // 浮遊テキストX最小値
    [SerializeField] private int fTRandPosXmax; // 浮遊テキストX最大値
    [SerializeField] private int fTPosY; // 浮遊テキストY位置
    [SerializeField] private int fTRandPosZmin; // 浮遊テキストZ位置
    [SerializeField] private int fTRandPosZmax;

    [Header("GameInfo")]
    [SerializeField] int hookMoveSpeed_y;
    private bool isClear = false; // クリアしたか
    private int gameLevel; // ゲームレベル
    private string targetHexMap; // 対象のヘックスマップ
    [SerializeField] private int clearScore; // クリアスコア
    [SerializeField] private int score; // スコア
    public bool gameStart; // ゲーム開始フラグ
    [SerializeField] private TMP_Text clearOrNot_Text; // クリア表示テキスト
    [SerializeField] private GameObject UkezaraPrefab; // 受皿プレハブ
    [SerializeField] private int ukezaraPos;
    [SerializeField] private GameObject stopLine;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvas1;
    [SerializeField] private TMP_Text stage_text;
    [SerializeField] private TMP_Text clearScore_text;
    [SerializeField] private Button tryButton;
    public List<string> ballScores; // ボールのスコアリスト
    private int csvOffset = 4;
    [SerializeField] private bool isReachingStopLine;
    [SerializeField] private GameObject ball_zhuangshiPrefab;


    public GameObject effectPrefab;
    public SpriteRenderer lockOnMark;
    public GameObject laser;
    private LineRenderer laserLR;
    public List<Vector3> effectPos;
    public int currentList;

    [Header("SE Info")]
    [SerializeField] private bool isSEPlayed = false;

    AudioManager audio;
    /// <summary>
    /// シングルトンの初期化処理。
    /// Instance が null であればこのインスタンスを設定し、
    /// 既に存在する場合は破棄します。
    /// </summary>
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        effectPos = new List<Vector3>();
        currentList = 0;
        GameInit();
        CraneInit();

    }

    /// <summary>
    /// ゲーム開始時の初期化処理。
    /// UI更新、ゲーム初期化、カウントダウン開始を行います。
    /// </summary>
    void Start()
    {
        Debug.Log("chongqile");
        UIManager.Instance.UpdateScoreText(score);
        //GameInit();
        //StartCoroutine(StartingCover());
        MeasuringCylinderInit();
        SetJoyStickUI(true);
        CreateBall(ballToCreate);
        CreateBallZhuangshi();

        effectPrefab.transform.position = new Vector3(0, effectPos[currentList].y, 0);
        lockOnMark.transform.position = new Vector3(0, effectPos[currentList].y, 0);
        laserLR = laser.GetComponent<LineRenderer>();
        // ParticleSystem eff = effectPrefab.GetComponent<ParticleSystem>();
        //GameObject eff = Instantiate(effectPrefab,new Vector3(0,effectPos[currentList].y,0),Quaternion.identity);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) { gameStart = true; SetJoyStickUI(false); }
    }
    private void CraneInit()
    {
        HookController crane = Crane.GetComponent<HookController>();
        crane.SetXMoveSpeed((float)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 16));
    }

    public void SetEffectPos(Transform _pos)
    {
        if (currentList < 0 || currentList >= effectPos.Count)
        {
            Debug.LogError($"currentList 越界: {currentList}, effectPos.Count = {effectPos.Count}");
            return;
        }
        Vector3 start = Crane.transform.position + new Vector3(0,5,0);
        Vector3 end = new Vector3(_pos.position.x, effectPos[currentList].y, _pos.position.z);
        effectPrefab.transform.position =
            new Vector3(_pos.position.x, effectPos[currentList].y + 2, _pos.position.z);
        lockOnMark.transform.position = new Vector3(_pos.position.x, effectPos[currentList].y + 2.5f, _pos.position.z);
        laserLR.SetPosition(0, start);
        laserLR.SetPosition(1, end);
    }

    public void closeLockOnMark()
    {
        effectPrefab.gameObject.SetActive(false);
        lockOnMark.gameObject.SetActive(false);
        laser.gameObject.SetActive(false);
    }
    public void SetJoyStickUI(bool _active)
    {
        joystickUI.gameObject.SetActive(_active);
    }
    public void PlusCurrentList()
    {
        currentList++;
    }
    private void LateUpdate()
    {
        Vector3 camPos = Camera.main.transform.position;

        float minY = ukezaraPos + 23;

        if (camPos.y < minY)
        {
            camPos.y = minY;
        }

        Camera.main.transform.position = camPos;

        background.transform.position = camPos + offset;

    }

    /// <summary>
    /// ゲーム開始時のカウントダウン演出処理。
    /// 黒カバーとカウントダウンテキストを順番に表示し、
    /// 最後にゲーム開始フラグを true に設定します。
    /// </summary>
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

        gameStart = true;
    }

    /// <summary>
    /// 現在のスコアを取得します。
    /// </summary>
    /// <returns>現在のスコア</returns>
    public int GetScore() => score;

    /// <summary>
    /// クリアスコアを取得します。
    /// </summary>
    /// <returns>クリアスコア</returns>
    public int GetClearScore() => clearScore;

    /// <summary>
    /// ゲーム開始フラグを取得します。
    /// </summary>
    /// <returns>ゲームが開始されているか</returns>
    public bool GetIsGameStart() => gameStart;

    /// <summary>
    /// ゲームクリア状態を取得します。
    /// </summary>
    /// <returns>ゲームがクリアされているか</returns>
    public bool GetIsClear() => isClear;

    public void ClickOnTryButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene", LoadSceneMode.Single);
    }
    /// <summary>
    /// 次のステージへ進むボタン押下時の処理。
    /// ゲームレベルを設定し、InGameScene をロードします。
    /// </summary>
    public void ClickOnToNextStage()
    {
        canvas.gameObject.SetActive(false);
        canvas1.gameObject.SetActive(true);
        GameManager.Instance.SetGameLevel((GameManager.GameLevel)(gameLevel+1));
        targetHexMap = GameManager.Instance.GetLevelCsvPath();
        gameLevel = GameManager.Instance.GetGameLevel();
        stage_text.text = "STAGE " + gameLevel.ToString();
        clearScore_text.text = "Clear Score : " + CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 7).ToString();


    }

    /// <summary>
    /// リトライボタン押下時の処理。
    /// InGameScene を再ロードします。
    /// </summary>
    public void ClickOnTryAgainButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// ステージ選択に戻るボタン押下時の処理。
    /// StageSelectScene をロードします。
    /// </summary>
    public void ClickOnBackToStageSelectButton()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
    }

    /// <summary>
    /// リザルト画面を表示する処理。
    /// スコアに応じてクリアかゲームオーバーを判定し、
    /// 各UIを更新して表示します。
    /// </summary>
    public void ShowResultScreen()
    {
        pauseButton.gameObject.SetActive(false);

        if (score >= clearScore)
        {
            isClear = true;
            resultBackground.sprite = resultBackSR[0];
            result_text.text = "CLEAR!!";
            clearscore_text.text = "CLEAR SCORE : " + clearScore.ToString() + "m";
            score_text.text = "SCORE : " + score.ToString() + "m";
            jiyu_text.text = "GOODJOB!!";
            audio.PlaySE(audio.gameClearSE);
            nextStageButton.gameObject.SetActive(true);
            GameManager.Instance.SetIsStageClear(gameLevel, isClear);
        }
        else
        {
            resultBackground.sprite = resultBackSR[1];
            result_text.text = "GAME OVER";
            clearscore_text.text = "CLEAR SCORE : " + clearScore.ToString() + "m";
            score_text.text = "SCORE : " + score.ToString() + "m";
            jiyu_text.text = "TRYAGAIN!";
            audio.PlaySE(audio.gameOverSE);
            tryAgainButton.gameObject.SetActive(true);
        }

        resultBackground.gameObject.SetActive(true);
        result_text.gameObject.SetActive(true);
        clearscore_text.gameObject.SetActive(true);
        score_text.gameObject.SetActive(true);
        jiyu_text.gameObject.SetActive(true);
        titleSelectButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// ゲームの初期化処理。
    /// スコアリセット、UI初期化、CSVからのマップ情報読み込み、
    /// ボール・パネル・ギミック生成を行います。
    /// </summary>
    public void GameInit()
    {
        Debug.Log("chongqile");
        score = 0;
        isClear = false;
        Init_UI();

        targetHexMap = GameManager.Instance.GetLevelCsvPath();
        gameLevel = GameManager.Instance.GetGameLevel();
        Debug.Log("gamelevel = " + gameLevel);

        panelOffset = CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 10);
        gimmickOffset = CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 12);
        hookMoveSpeed_y = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 14);
        ukezaraPos = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "V", 3);

        ballToCreate = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 18);
        minBallScore = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 20);
        maxBallScore = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 22);

        CreatePanel();
        CreateGimmick();
        //liangtongInit();
        clearScore = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 7);
        ingameClearScore_text.text = "ClearScore : " + clearScore;

        audio = AudioManager.Instance;

        //AudioManager.Instance.StopBGM();
        //AudioManager.Instance.PlayBGM(AudioManager.Instance.InGameBGM);
    }

    private void MeasuringCylinderInit()
    {
        measurePerfab.transform.position = new Vector3(81.5f, ukezaraPos + 10, -55f);
        measurePerfab.transform.rotation = Quaternion.Euler(18, 0, 0);
        liangtongPerfab.transform.position = new Vector3(81.5f, ukezaraPos - 7, -55f);
        liangtongPerfab.transform.rotation = Quaternion.Euler(20.2f, 0, 0);
        stopLine.transform.position = new Vector3(0, ukezaraPos - 3, 0);
    }

    /// <summary>
    /// Gimmickを生成する
    /// </summary>
    private void CreateGimmick()
    {
        numOfGimmick = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 5);
        for (int i = 0; i < numOfGimmick; i++)
        {
            gimmickType = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "H", 5 + csvOffset * i);
            gimmickPos_Y = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "T", 5 + csvOffset * i);
            if (gimmickType == 1)//spikeball
            {
                gimmickPosType = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "N", 5 + csvOffset * i);
                //pos = gimmickPos5.position - new Vector3(0, i * 40, 0);
                switch (gimmickPosType)
                {
                    case 1: pos = gimmickPos1.position + new Vector3(0, gimmickPos_Y, 0); break;
                    case 2: pos = gimmickPos2.position + new Vector3(0, gimmickPos_Y, 0); break;
                    case 3: pos = gimmickPos3.position + new Vector3(0, gimmickPos_Y, 0); break;
                    case 4: pos = gimmickPos4.position + new Vector3(0, gimmickPos_Y, 0); break;
                    default: break;
                }
                spikeBallSize = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "O", 5 + csvOffset * i);
                Gimmick_SpikeBall newSpikeBall = Instantiate(spikeBallPerfab, pos, Quaternion.identity).GetComponent<Gimmick_SpikeBall>();
                newSpikeBall.SetSize(spikeBallSize);
            }
            else if (gimmickType == 2)//rotarysickle
            {
                pos = gimmickPos5.position + new Vector3(0, gimmickPos_Y, 0);      //i * gimmickOffset + 2 * gimmickOffset
                Gimmick_Sickle newRotarySickle = Instantiate(rotarySicklePerfab, pos, Quaternion.identity).GetComponent< Gimmick_Sickle>();
                newRotarySickle.SetIsRotate((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "K", 5 + csvOffset * i));
                newRotarySickle.SetIsRotateClockwise((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "L", 5 + csvOffset * i));
                newRotarySickle.SetRotateSpeed((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "M", 5 + csvOffset * i));
                newRotarySickle.SetSubFinalScore((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "I", 5 + csvOffset * i));
            }

            else if (gimmickType == 3)//slidestick
            {
                gimmickPosType = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "P", 5 + csvOffset * i);

                Vector3 basePos;
                Quaternion rot;

                switch (gimmickPosType)
                {
                    case 1: basePos = gimmickPos2.position; rot = Quaternion.Euler(0, 0, 90); break;
                    case 2: basePos = gimmickPos4.position; rot = Quaternion.Euler(0, 0, 90); break;
                    case 3: basePos = gimmickPos4.position; rot = Quaternion.Euler(0, 90, 90); break;
                    case 4: basePos = gimmickPos3.position; rot = Quaternion.Euler(0, 90, 90); break;
                    case 5: basePos = gimmickPos3.position; rot = Quaternion.Euler(0, 180, 90); break;
                    case 6: basePos = gimmickPos1.position; rot = Quaternion.Euler(0, 180, 90); break;
                    case 7: basePos = gimmickPos1.position; rot = Quaternion.Euler(0, 270, 90); break;
                    case 8: basePos = gimmickPos2.position; rot = Quaternion.Euler(0, 270, 90); break;
                    default:
                        Debug.LogWarning("Unexpected gimmickPosType: " + gimmickPosType);
                        return;
                }

                Vector3 pos = basePos + new Vector3(0, gimmickPos_Y, 0);

                Gimmick_SlideStick newSlideStick = Instantiate(slideStickPerfab, pos, Quaternion.identity).GetComponent<Gimmick_SlideStick>();
                newSlideStick.transform.rotation = rot;

                newSlideStick.SetXMoveSpeed((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "R", 5 + csvOffset * i));
                newSlideStick.SetMoveTime((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "Q", 5 + csvOffset * i));
                newSlideStick.SetSubBallScore((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "J", 5 + csvOffset * i));

                newSlideStick.gameObject.SetActive(true);
            }
        }
    }
    /// <summary>
    /// ボールを生成する
    /// </summary>
    public void CreateBall(int _ballToCreate)
    {
        for (int i = 0; i < _ballToCreate; i++)
        {
            int randPosX = Random.Range(-25, 25);
            int randPosZ = Random.Range(-25, 25);
            pos = new Vector3(randPosX, ukezaraPos+5, randPosZ);
            //GameObject newBall = Instantiate(ballPerfab, pos, Quaternion.identity);
            GameObject newBall = PoolManager.Instance.GetObject("ball", pos, Quaternion.identity);
            //GameObject newAnon = Instantiate(anonPerfab,pos, Quaternion.identity);
            //newAnon.transform.localScale = new Vector3(5, 5, 5);
            int randScore = Random.Range(minBallScore, maxBallScore);
            newBall.GetComponent<Ball>().SetBallScore(randScore);
            //Debug.Log("生成ball！");
        }
    }

    public void CreateBallZhuangshi()
    {
        for (int i = 0; i < 300; i++)
        {
            int randPosX = Random.Range(-25, 25);
            int randPosZ = Random.Range(-25, 25);
            int randPosY = Random.Range(5, 10);

            pos = new Vector3(randPosX, ukezaraPos - randPosY, randPosZ);
            //if()
            //GameObject newBall = Instantiate(ballPerfab, pos, Quaternion.identity);
            GameObject newBall = PoolManager.Instance.GetObject("ball", pos, Quaternion.identity);

            //newBall.GetComponent<Rigidbody>().useGravity = false;

            //GameObject newAnon = Instantiate(anonPerfab,pos, Quaternion.identity);
            //newAnon.transform.localScale = new Vector3(5, 5, 5);
            int randScore = Random.Range(minBallScore, maxBallScore);
            newBall.GetComponent<Ball>().SetBallScore(randScore);
            //Debug.Log("生成ball！");

            //Renderer render = newBall.GetComponent<Renderer>();
            //render.material.color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        }
    }
    /// <summary>
    /// UIの初期化
    /// </summary>
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
        clearOrNot_Text.gameObject.SetActive(false);
    }
    /// <summary>
    /// パネルを生成する
    /// </summary>
    private void CreatePanel()
    {
        numOfPanel = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "E", 3);
        for (int i = 0; i < numOfPanel; i++)
        {
            panelType = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "H", csvOffset * i + 3);
            panelPos_Y = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "T", csvOffset * i + 3);
            Vector3 pos = new Vector3(0, panelPos_Y, 0);
            PanelManager newPanel = Instantiate(panelobj, pos, Quaternion.identity).GetComponent<PanelManager>();

            effectPos.Add(pos);

            newPanel.SetPanelPartsType(panelType);
            if (panelType == 2)
            {
                leftPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "I", csvOffset * i + 3);
                leftValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "J", csvOffset * i + 3);
                rightPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "K", csvOffset * i + 3);
                rightValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "L", csvOffset * i + 3);


                newPanel.SetPanelPartsTwo(leftPanelCode, leftValue, rightPanelCode, rightValue);
            }

            else if (panelType == 4)
            {
                upperLeftPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "I", csvOffset * i + 3);
                upperLeftPanelValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "J", csvOffset * i + 3);
                upperRightPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "K", csvOffset * i + 3);
                upperRightPanelValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "L", csvOffset * i + 3);

                lowerLeftPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "M", csvOffset * i + 3);
                lowerLeftPanelValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "N", csvOffset * i + 3);
                lowerRightPanelCode = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "O", csvOffset * i + 3);
                lowerRightPanelValue = (int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "P", csvOffset * i + 3);

                newPanel.SetPanelPartsFour(upperLeftPanelCode, upperLeftPanelValue, upperRightPanelCode, upperRightPanelValue,
                    lowerLeftPanelCode, lowerLeftPanelValue, lowerRightPanelCode, lowerRightPanelValue);
            }

            newPanel.SetIsRotate((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "Q", csvOffset * i + 3));
            newPanel.SetIsRotateClockwise((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "R", csvOffset * i + 3));
            newPanel.SetRotateSpeed((int)CSVReader.instance.ReadTargetCellIndex(targetHexMap, "S", csvOffset * i + 3));

        }
        Vector3 ukezarapos = new Vector3(0, ukezaraPos, 0);
        Vector3 ukezararota = new Vector3(90, 0, 0);
        effectPos.Add(ukezarapos);
        GameObject newUkezara =  Instantiate(UkezaraPrefab, ukezarapos, Quaternion.Euler(ukezararota));
    }

    public void SetClearOrNotText()
    {
        clearOrNot_Text.gameObject.SetActive(true);
        //if (score > clearScore)
        //{
            clearOrNot_Text.text = "Finish!";

        audio.PlaySE(audio.gameFinishSE);
        //}
        //else
        //{
        //    clearOrNot_Text.text = "GAME OVER";
        //}
    }
    public void SetClearOrNotTextOff()
    {
        clearOrNot_Text.gameObject.SetActive(false);
        ingameClearScore_text.gameObject.SetActive(false);
    }
    public bool GetIsReachingStopLine() => isReachingStopLine;
    public void SetIsReachingStopLine(bool _isReachingStopLine)
    {
        isReachingStopLine = _isReachingStopLine;
    }
    public bool GetIsMeasuring() => isMeasuring;

    public void SetIsMeasuring(bool _isMeasuring) {  isMeasuring = _isMeasuring; }
    public int GetLeftPanelCode() => leftPanelCode;
    public int GetRightPanelCode() => rightPanelCode;
    public int GetLeftValue() => leftValue;
    public int GetRightValue() => rightValue;
    public void AddScore(int amount)
    {
        score += amount;
        string display = amount > 0 ? $"+{amount}" : amount.ToString();
        ballScores.Add(display);
        // 可以在这里通知 UI 更新
        UIManager.Instance.UpdateScoreText(score);
    }

    public void SubScore(int amount)
    {
        score -= amount;
        UIManager.Instance.UpdateScoreText(score);
    }

    public IEnumerator SpawnFloatingText(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        for(int i = 0; i < ballScores.Count; i++)
        {
            Vector3 flotingTextPos = measurePerfabPosition.position + new Vector3(Random.Range(fTRandPosXmin, fTRandPosXmax), fTPosY + i * 1f, Random.Range(fTRandPosZmin,fTRandPosZmax));
            FloatingText newFloatingText = Instantiate(floatingText, flotingTextPos, Quaternion.identity);
            newFloatingText.SetFloatingText(ballScores[i].ToString());
            yield return new WaitForSeconds(0.2f);
        }
    }
    public int GetHookMoveSpeed() => hookMoveSpeed_y;

    public Vector3 GetLiangtongPosition()
    {
        if (liangtongPerfab != null)
            return liangtongPerfab.transform.position;

        Debug.LogError("liangtongPerfab is null!");
        return Vector3.zero;
    }

    public float GetLiangtongZ()
    {
        return liangtongPerfab.transform.position.z;
    }

    public int GetukezaraPos() => ukezaraPos;
}

