using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// パネルの制御を行うクラス
/// Panelの種類（2分割 or 4分割）に応じてテキスト・挙動・回転などを管理します。
/// </summary>
public class PanelManager : MonoBehaviour
{
    // カメラ参照
    private Camera mainCamera;

    // ボールのプレハブ
    [SerializeField] private GameObject ballPerfab;

    // 生成位置や回転制御用
    [SerializeField] private Vector3 pos;
    [SerializeField] private float rotateSpeed;

    // 一度だけ実行される処理の制御フラグ
    [SerializeField] private bool isBurn = false;

    // 回転するかどうか
    [SerializeField] private bool isRotate;
    // 時計回りに回転するかどうか
    [SerializeField] private bool isRotateClockwise;

    // Panelの構成（2分割 or 4分割）
    [SerializeField] private GameObject panelPartsTwo;
    [SerializeField] private GameObject panelPartsFour;

    // 左右の判定用コライダー
    [SerializeField] private BoxCollider panelCollier_Left;
    [SerializeField] private BoxCollider panelCollier_Right;

    // 2分割(=2) or 4分割(=4)
    [SerializeField] private int panelPartsType;

    [SerializeField] private SpriteRenderer[] sr;
    [SerializeField] private Sprite[] image;

    // ==================== 2分割パネル用設定 ====================
    [Header("PanelPartsTwo")]
    [SerializeField] private int LeftPanelCode;
    [SerializeField] private int RightPanelCode;
    [SerializeField] private int LeftPanelValue;
    [SerializeField] private int RightPanelValue;

    [SerializeField] private TMP_Text text_left;
    [SerializeField] private TMP_Text text_right;

    public GameObject panelParts2obj;

    // ==================== 4分割パネル用設定 ====================
    [Header("PanelPartsFour")]
    [SerializeField] private int upperLeftPanelCode;
    [SerializeField] private int upperRightPanelCode;
    [SerializeField] private int lowerLeftPanelCode;
    [SerializeField] private int lowerRightPanelCode;

    [SerializeField] private int upperLeftPanelValue;
    [SerializeField] private int upperRightPanelValue;
    [SerializeField] private int lowerLeftPanelValue;
    [SerializeField] private int lowerRightPanelValue;

    [SerializeField] private TMP_Text text_upperLeft;
    [SerializeField] private TMP_Text text_upperRight;
    [SerializeField] private TMP_Text text_lowerLeft;
    [SerializeField] private TMP_Text text_lowerRight;

    public GameObject panelParts4obj;

    // 共通の値格納用
    private int value;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        Init_Text();
        mainCamera = Camera.main;
    }

    /// <summary>
    /// パネルテキスト初期化処理
    /// 各PanelCodeに対応する文字列を表示します。
    /// </summary>
    private void Init_Text()
    {
        if (panelPartsType == 2)
        {
            panelPartsTwo.SetActive(true);
            text_left.text = GetPanelCodeText(LeftPanelCode, LeftPanelValue,0);
            text_right.text = GetPanelCodeText(RightPanelCode, RightPanelValue,1);
            panelParts2obj.gameObject.SetActive(true);
        }
        else if (panelPartsType == 4)
        {
            panelPartsFour.SetActive(true);
            text_upperLeft.text = GetPanelCodeText(upperLeftPanelCode, upperLeftPanelValue,2);
            text_upperRight.text = GetPanelCodeText(upperRightPanelCode, upperRightPanelValue,3);
            text_lowerLeft.text = GetPanelCodeText(lowerLeftPanelCode, lowerLeftPanelValue,4);
            text_lowerRight.text = GetPanelCodeText(lowerRightPanelCode, lowerRightPanelValue,5);
            panelParts4obj.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 毎フレーム更新処理
    /// </summary>
    void Update()
    {
        RotateMovement();
        SetTextRotate();
    }

    /// <summary>
    /// テキストの回転をリセットし、読みやすく保つ
    /// </summary>
    private void SetTextRotate()
    {

        text_left.transform.rotation = Quaternion.identity;
        text_right.transform.rotation = Quaternion.identity;
        text_lowerLeft.transform.rotation = Quaternion.identity;
        //text_lowerLeft.transform.forward = Camera.main.transform.forward;
        text_lowerRight.transform.rotation = Quaternion.identity;
        //text_lowerRight.transform.forward = Camera.main.transform.forward;
        text_upperLeft.transform.rotation = Quaternion.identity;
        //text_upperLeft.transform.forward = Camera.main.transform.forward;
        text_upperRight.transform.rotation = Quaternion.identity;
        //text_upperRight.transform.forward = Camera.main.transform.forward;

        ////sr.transform.rotation = Quaternion.identity;
        foreach (SpriteRenderer sprite in sr)
        {
            sprite.transform.forward = Camera.main.transform.forward;

        }
    }

    private void LateUpdate()
    {
        sr[0].transform.position = text_left.transform.position + new Vector3(-8, 9, 0);
        sr[1].transform.position = text_right.transform.position + new Vector3(-8, 9, 0);
        sr[2].transform.position = text_upperLeft.transform.position + new Vector3(-11, 9, 0);
        sr[3].transform.position = text_upperRight.transform.position + new Vector3(-11, 9, 0);
        sr[4].transform.position = text_lowerLeft.transform.position + new Vector3(-11, 9, 0);
        sr[5].transform.position = text_lowerRight.transform.position + new Vector3(-11, 9, 0);
       
    }

    /// <summary>
    /// パネルコードと値を合成して文字列を返す
    /// </summary>
    private string GetPanelCodeText(int code, int value, int srIndex)
    {
        //string prefix = code switch
        //{
        //    //1 => "s+",
        //    //2 => "s-",
        //    //3 => "s*",
        //    //4 => "s/",
        //    //5 => "b+",
        //    //6 => "b-",
        //    //7 => "b*",
        //    //8 => "b/",
        //    //_ => ""


        //};
        //return prefix + value.ToString();

        if (code >= 1 && code <= image.Length)
            sr[srIndex].sprite = image[code - 1];

        return value.ToString();
    }

    /// <summary>
    /// 他のColliderと接触した際の処理
    /// hookタグのオブジェクトと接触すると自身のColliderを無効化する。
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hook"))
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }

            Collider col = GetComponent<Collider>();
            col.enabled = false;
        }
    }

    // ==================== Getter / Setter 系 ====================
    public int GetPanelPartsType() { return panelPartsType; }
    public void SetPanelPartsType(int code) { this.panelPartsType = code; }

    // 2分割パネル設定

    public void SetPanelPartsTwo(int _leftCode,int _leftValue,int _rightCode,int _rightValue)
    {
        this.LeftPanelCode = _leftCode;
        this.RightPanelCode = _rightCode;
        this.LeftPanelValue = _leftValue;
        this.RightPanelValue = _rightValue;
    }
    public int GetLeftPanelCode() { return LeftPanelCode; }
    public int GetRightPanelCode() { return RightPanelCode; }
    public int GetLeftPanelValue() { return LeftPanelValue; }
    public int GetRightPanelValue() { return RightPanelValue; }
    public void SetLeftPanelCode(int code) { this.LeftPanelCode = code; }
    public void SetRightPanelCode(int code) { this.RightPanelCode = code; }
    public void SetLeftPanelValue(int value) { this.LeftPanelValue = value; }
    public void SetRightPanelValue(int value) { this.RightPanelValue = value; }

    // 4分割パネル設定
    public void SetPanelPartsFour(int _upperLeftCode,int _upperLeftValue,int _upperRightCode,int _upperRightValue,
        int _lowerLeftCode,int _lowerLeftValue,int _lowerRightCode, int _lowerRightValue)
    {
        this.upperLeftPanelCode = _upperLeftCode;
        this.upperLeftPanelValue = _upperLeftValue;
        this.upperRightPanelCode = _upperRightCode;
        this.upperRightPanelValue = _upperRightValue;
        this.lowerLeftPanelCode = _lowerLeftCode;
        this.lowerLeftPanelValue = _lowerLeftValue;
        this.lowerRightPanelCode = _lowerRightCode;
        this.lowerRightPanelValue = _lowerRightValue;
    }

    public int GetUpperLeftPanelCode() { return upperLeftPanelCode; }
    public int GetUpperRightPanelCode() { return upperRightPanelCode; }
    public int GetLowerLeftPanelCode() { return lowerLeftPanelCode; }
    public int GetLowerRightPanelCode() { return lowerRightPanelCode; }
    public int GetUpperLeftPanelValue() { return upperLeftPanelValue; }
    public int GetUpperRightPanelValue() { return upperRightPanelValue; }
    public int GetLowerLeftPanelValue() { return lowerLeftPanelValue; }
    public int GetLowerRightPanelValue() { return lowerRightPanelValue; }
    public void SetUpperLeftPanelCode(int code) { this.upperLeftPanelCode = code; }
    public void SetUpperRightPanelCode(int code) { this.upperRightPanelCode = code; }
    public void SetLowerLeftPanelCode(int code) { this.lowerLeftPanelCode = code; }
    public void SetLowerRightPanelCode(int code) { this.lowerRightPanelCode = code; }
    public void SetUpperLeftPanelValue(int value) { this.upperLeftPanelValue = value; }
    public void SetUpperRightPanelValue(int value) { this.upperRightPanelValue = value; }
    public void SetLowerLeftPanelValue(int value) { this.lowerLeftPanelValue = value; }
    public void SetLowerRightPanelValue(int value) { this.lowerRightPanelValue = value; }

    public void SetRotateSpeed(int value) { this.rotateSpeed = value; }

    /// <summary>
    /// パネルの回転処理
    /// isRotate=trueなら指定方向に回転
    /// </summary>
    public void RotateMovement()
    {
        int dir = isRotateClockwise ? 1 : -1;
        if (isRotate)
        {
            transform.Rotate(0, dir * rotateSpeed * Time.deltaTime, 0);
        }

        //SetTextRotate();
    }

    /// <summary>
    /// 回転方向の設定（0=false, 非0=true）
    /// </summary>
    public void SetIsRotateClockwise(int value)
    {
        isRotateClockwise = (value != 0);
    }

    /// <summary>
    /// 回転するかどうかの設定（0=false, 非0=true）
    /// </summary>
    public void SetIsRotate(int value)
    {
        isRotate = (value != 0);
    }

    // ==================== Ball関連の処理 ====================

    /// <summary>
    /// 指定数のボールを生成する
    /// </summary>
    public void CreateBall(int numOfBallToCreate)
    {
        if (!isBurn)
        {
            for (int i = 0; i < numOfBallToCreate; i++)
            {
                int randPosX = Random.Range(-25, 25);
                int randPosZ = Random.Range(-25, 25);
                pos = new Vector3(randPosX, -150, randPosZ);
                GameObject newBall = Instantiate(ballPerfab, pos, Quaternion.identity);

                int randScore = Random.Range(1, 11);
                newBall.GetComponent<Ball>().SetBallScore(randScore);
                Debug.Log("生成了一个新物体！");
            }
            isBurn = true;
        }
    }

    /// <summary>
    /// 指定数のボールを削除する
    /// </summary>
    public void SubBall(int numberOfBallToDelete)
    {
        int deletedBall = 0;
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
            deletedBall++;
            if (deletedBall >= numberOfBallToDelete)
                break;
        }
    }

    /// <summary>
    /// ボール数を指定倍率で増加させる
    /// </summary>
    public void MulBall(int numOfBallToMul)
    {
        if (!isBurn)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
            int num = balls.Length;
            int temp = num * numOfBallToMul;

            for (int i = 0; i < temp; i++)
            {
                GameObject newBall = Instantiate(ballPerfab, pos, Quaternion.identity);
                int randScore = Random.Range(1, 11);
                newBall.GetComponent<Ball>().SetBallScore(randScore);
                Debug.Log("生成了一个新物体！");
            }
            isBurn = true;
        }
    }

    /// <summary>
    /// ボール数を指定値で割り、余分を削除
    /// </summary>
    public void DivBall(int numOfBallToDiv)
    {
        int deletedBall = 0;
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        int num = balls.Length;
        int temp = num / numOfBallToDiv;

        foreach (GameObject ball in balls)
        {
            Destroy(ball);
            deletedBall++;
            if (deletedBall >= temp)
                break;
        }
    }

    /// <summary>
    /// 全てのボールのスコアに加算
    /// </summary>
    public void AddBallScore(int scoreToAdd)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ballObj in balls)
        {
            Ball ball = ballObj.GetComponent<Ball>();
            if (ball != null)
            {
                ball.AddBallScore(scoreToAdd);
            }
        }
    }

    /// <summary>
    /// 全てのボールのスコアを減算
    /// </summary>
    public void SubBallScore(int scoreToSub)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ballObj in balls)
        {
            Ball ball = ballObj.GetComponent<Ball>();
            if (ball != null)
            {
                ball.SubBallScore(scoreToSub);
            }
        }
    }

    /// <summary>
    /// 全てのボールのスコアを指定倍率で乗算
    /// </summary>
    public void MulBallScore(int scoreToMul)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ballObj in balls)
        {
            Ball ball = ballObj.GetComponent<Ball>();
            if (ball != null)
            {
                ball.MulBallScore(scoreToMul);
            }
        }
    }

    /// <summary>
    /// 全てのボールのスコアを指定値で除算
    /// </summary>
    public void DivBallScore(int scoreToDiv)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ballObj in balls)
        {
            Ball ball = ballObj.GetComponent<Ball>();
            if (ball != null)
            {
                ball.DivBallScore(scoreToDiv);
            }
        }
    }
}
