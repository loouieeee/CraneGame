using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PanelManager : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private GameObject ballPerfab;

    //[SerializeField] private int numOfBallToCreate;
    //[SerializeField] private int numberOfBallToDelete;

    [SerializeField] private Vector3 pos;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private bool isBurn = false;
    [SerializeField] private bool isRotate;
    [SerializeField] private bool isRotateClockwise;

    [SerializeField] private GameObject panelPartsTwo;
    [SerializeField] private GameObject panelPartsFour;
 
    [SerializeField] private BoxCollider panelCollier_Left;
    [SerializeField] private BoxCollider panelCollier_Right;

    [SerializeField] private int panelPartsType;

    [Header("PanelPartsTwo")]
    [SerializeField] private int LeftPanelCode;
    [SerializeField] private int RightPanelCode;
    [SerializeField] private int LeftPanelValue;
    [SerializeField] private int RightPanelValue;
    [SerializeField] private TMP_Text text_left;
    [SerializeField] private TMP_Text text_right;

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

    private int value;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init_Text();
        mainCamera = Camera.main;
    }

    private void Init_Text()
    {
        #region fix
        //if (panelPartsType == 2)
        //{
        //    panelPartsTwo.gameObject.SetActive(true);
        //    text_left.text = "";
        //    text_right.text = "";

        //    if (LeftPanelCode == 1) { text_left.text = "s+"; }
        //    else if (LeftPanelCode == 2) { text_left.text = "s-"; }
        //    else if (LeftPanelCode == 3) { text_left.text = "s*"; }
        //    else if (LeftPanelCode == 4) { text_left.text = "s/"; }
        //    else if (LeftPanelCode == 5) { text_left.text = "b+"; }
        //    else if (LeftPanelCode == 6) { text_left.text = "b-"; }
        //    else if (LeftPanelCode == 7) { text_left.text = "b*"; }
        //    else if (LeftPanelCode == 8) { text_left.text = "b/"; }
        //    text_left.text += LeftPanelValue.ToString();

        //    if (RightPanelCode == 1) { text_right.text = "s+"; }
        //    else if (RightPanelCode == 2) { text_right.text = "s-"; }
        //    else if (RightPanelCode == 3) { text_right.text = "s*"; }
        //    else if (RightPanelCode == 4) { text_right.text = "s/"; }
        //    else if (RightPanelCode == 5) { text_right.text = "b+"; }
        //    else if (RightPanelCode == 6) { text_right.text = "b-"; }
        //    else if (RightPanelCode == 7) { text_right.text = "b*"; }
        //    else if (RightPanelCode == 8) { text_right.text = "b/"; }
        //    text_right.text += RightPanelValue.ToString();
        //}

        //else if(panelPartsType == 4)
        //{
        //    panelPartsFour.gameObject.SetActive(true);
        //    text_upperLeft.text = "";
        //    text_upperRight.text = "";
        //    text_lowerLeft.text = "";
        //    text_lowerRight.text = "";

        //    if (upperLeftPanelCode == 1) { text_upperLeft.text = "s+"; }
        //    else if (upperLeftPanelCode == 2) { text_upperLeft.text = "s-"; }
        //    else if (upperLeftPanelCode == 3) { text_upperLeft.text = "s*"; }
        //    else if (upperLeftPanelCode == 4) { text_upperLeft.text = "s/"; }
        //    else if (upperLeftPanelCode == 5) { text_upperLeft.text = "b+"; }
        //    else if (upperLeftPanelCode == 6) { text_upperLeft.text = "b-"; }
        //    else if (upperLeftPanelCode == 7) { text_upperLeft.text = "b*"; }
        //    else if (upperLeftPanelCode == 8) { text_upperLeft.text = "b/"; }
        //    text_upperLeft.text += upperLeftPanelValue.ToString();

        //    if (upperRightPanelCode == 1) { text_upperRight.text = "s+"; }
        //    else if (upperRightPanelCode == 2) { text_upperRight.text = "s-"; }
        //    else if (upperRightPanelCode == 3) { text_upperRight.text = "s*"; }
        //    else if (upperRightPanelCode == 4) { text_upperRight.text = "s/"; }
        //    else if (upperRightPanelCode == 5) { text_upperRight.text = "b+"; }
        //    else if (upperRightPanelCode == 6) { text_upperRight.text = "b-"; }
        //    else if (upperRightPanelCode == 7) { text_upperRight.text = "b*"; }
        //    else if (upperRightPanelCode == 8) { text_upperRight.text = "b/"; }
        //    text_upperRight.text += upperRightPanelValue.ToString();

        //    if (lowerLeftPanelCode == 1) { text_lowerLeft.text = "s+"; }
        //    else if (lowerLeftPanelCode == 2) { text_lowerLeft.text = "s-"; }
        //    else if (lowerLeftPanelCode == 3) { text_lowerLeft.text = "s*"; }
        //    else if (lowerLeftPanelCode == 4) { text_lowerLeft.text = "s/"; }
        //    else if (lowerLeftPanelCode == 5) { text_lowerLeft.text = "b+"; }
        //    else if (lowerLeftPanelCode == 6) { text_lowerLeft.text = "b-"; }
        //    else if (lowerLeftPanelCode == 7) { text_lowerLeft.text = "b*"; }
        //    else if (lowerLeftPanelCode == 8) { text_lowerLeft.text = "b/"; }
        //    text_lowerLeft.text += lowerLeftPanelValue.ToString();

        //    if (lowerRightPanelCode == 1) { text_lowerRight.text = "s+"; }
        //    else if (lowerRightPanelCode == 2) { text_lowerRight.text = "s-"; }
        //    else if (lowerRightPanelCode == 3) { text_lowerRight.text = "s*"; }
        //    else if (lowerRightPanelCode == 4) { text_lowerRight.text = "s/"; }
        //    else if (lowerRightPanelCode == 5) { text_lowerRight.text = "b+"; }
        //    else if (lowerRightPanelCode == 6) { text_lowerRight.text = "b-"; }
        //    else if (lowerRightPanelCode == 7) { text_lowerRight.text = "b*"; }
        //    else if (lowerRightPanelCode == 8) { text_lowerRight.text = "b/"; }
        //    text_lowerRight.text += lowerRightPanelValue.ToString();
        //}
        #endregion
        if (panelPartsType == 2)
        {
            panelPartsTwo.SetActive(true);
            text_left.text = GetPanelCodeText(LeftPanelCode, LeftPanelValue);
            text_right.text = GetPanelCodeText(RightPanelCode, RightPanelValue);
        }
        else if (panelPartsType == 4)
        {
            panelPartsFour.SetActive(true);
            text_upperLeft.text = GetPanelCodeText(upperLeftPanelCode, upperLeftPanelValue);
            text_upperRight.text = GetPanelCodeText(upperRightPanelCode, upperRightPanelValue);
            text_lowerLeft.text = GetPanelCodeText(lowerLeftPanelCode, lowerLeftPanelValue);
            text_lowerRight.text = GetPanelCodeText(lowerRightPanelCode, lowerRightPanelValue);
        }


    }

    // Update is called once per frame
    void Update()
    {
        RotateMovement();

        SetTextRotate();
    }

    private void SetTextRotate()
    {
        text_left.transform.rotation = Quaternion.identity;
        text_right.transform.rotation = Quaternion.identity;
        text_lowerLeft.transform.rotation = Quaternion.identity;
        text_lowerRight.transform.rotation = Quaternion.identity;
        text_upperLeft.transform.rotation = Quaternion.identity;
        text_upperRight.transform.rotation = Quaternion.identity;
    }

    private string GetPanelCodeText(int code, int value)
    {
        string prefix = code switch
        {
            1 => "s+",
            2 => "s-",
            3 => "s*",
            4 => "s/",
            5 => "b+",
            6 => "b-",
            7 => "b*",
            8 => "b/",
            _ => ""
        };
        return prefix + value.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other == panelCollier_Left && LeftPanelCode == 1) { }//ball score add
        //if (other == panelCollier_Left && LeftPanelCode== 2) { }//ball score sub
        //if (other == panelCollier_Left && LeftPanelCode == 3) { }//ball score mul
        //if (other == panelCollier_Left && LeftPanelCode == 4) { }//ball score div
        //if (other == panelCollier_Left && LeftPanelCode == 5) { CreateBall(SceneManager.Instance.GetLeftValue()); }//ball num add
        //if (other == panelCollier_Left && LeftPanelCode == 6) { }//ball num sub
        //if (other == panelCollier_Left && LeftPanelCode == 7) { }//ball num mul
        //if (other == panelCollier_Left && LeftPanelCode == 8) { }//ball num div
        //if (other == panelCollier_Right && SceneManager.Instance.GetRightPanelCode()== 1) { }//ball score add
        //if (other == panelCollier_Right && SceneManager.Instance.GetRightPanelCode()== 2) { }//ball score sub
        //if (other == panelCollier_Right && SceneManager.Instance.GetRightPanelCode()== 3) { }//ball score mul
        //if (other == panelCollier_Right && SceneManager.Instance.GetRightPanelCode()== 4) { }//ball score div
        //if (other == panelCollier_Right && SceneManager.Instance.GetRightPanelCode()== 5) { }//ball num add
        //if (other == panelCollier_Right && SceneManager.Instance.GetRightPanelCode()== 6) { }//ball num sub
        //if (other == panelCollier_Right && SceneManager.Instance.GetRightPanelCode()== 7) { }//ball num mul
        //if (other == panelCollier_Right && SceneManager.Instance.GetRightPanelCode()== 8) { }//ball num div

        //if(other.CompareTag("LeftPanel")) { Debug.Log("111"); CreateBall(10); }
        //if(other.CompareTag("RightPanel")) { Debug.Log("222"); CreateBall(23); }
        if (other.CompareTag("hook"))
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach(Collider collider in colliders)
            {
                collider.enabled = false;
            }

            Collider col = GetComponent<Collider>();
            col.enabled = false;
            //enabled = false;   // 开启触发器模式
        }

    }
    public int GetPanelPartsType() { return panelPartsType; }
    public void SetPanelPartsType(int code) { this.panelPartsType = code; }
    //2parts
    public int GetLeftPanelCode() { return LeftPanelCode; }
    public int GetRightPanelCode() {  return RightPanelCode; }
    public int GetLeftPanelValue() {  return LeftPanelValue; }
    public int GetRightPanelValue() { return RightPanelValue; }
    public void SetLeftPanelCode(int code) { this.LeftPanelCode = code; }
    public void SetRightPanelCode(int code) { this.RightPanelCode = code; }
    public void SetLeftPanelValue(int value) {  this.LeftPanelValue = value; }
    public void SetRightPanelValue(int value) { this.RightPanelValue = value; }
    //4parts
    public int GetUpperLeftPanelCode() { return upperLeftPanelCode; }
    public int GetUpperRightPanelCode() { return upperRightPanelCode; }
    public int GetLowerLeftPanelCode() { return  lowerLeftPanelCode; }
    public int GetLowerRightPanelCode() { return lowerRightPanelCode; }
    public int GetUpperLeftPanelValue() { return upperLeftPanelValue; }
    public int GetUpperRightPanelValue() {  return upperRightPanelValue; }
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

    public void RotateMovement()
    {
        int dir = isRotateClockwise ? 1 : -1;
        if (isRotate)
        {
            transform.Rotate(0, dir * rotateSpeed * Time.deltaTime, 0);
        }
    }

    public void SetIsRotateClockwise(int value)
    {
        isRotateClockwise = (value != 0);
    }
    public void SetIsRotate(int value)
    {
        isRotate = (value != 0); // 非0视为true
    }


    public void CreateBall(int numOfBallToCreate)
    {
        if (!isBurn)
        {

            // 在触发物体的位置生成
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

    public void SubBall(int numberOfBallToDelete)
    {
        int deletedBall = 0;
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ball in balls)
        {
            // 如果不是自己也不是被碰到的目标，就销毁
            //if (ball != gameObject && ball != other.gameObject)
            //{
                Destroy(ball);
                deletedBall++;

                if (deletedBall >= numberOfBallToDelete)
                    break;

            //}
        }
    }

    public void MulBall(int numOfBallToMul)
    {
        if (!isBurn)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
            int num = balls.Length;
            int temp = num * numOfBallToMul;

            // 在触发物体的位置生成
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
    
    public void DivBall(int numOfBallToDiv)
    {
        int deletedBall = 0;
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        int num = balls.Length;
        int temp = num / numOfBallToDiv;

        foreach (GameObject ball in balls)
        {
            // 如果不是自己也不是被碰到的目标，就销毁
            //if (ball != gameObject && ball != other.gameObject)
            //{
            Destroy(ball);
            deletedBall++;

            if (deletedBall >= temp)
                break;

            //}
        }
    }

    public void AddBallScore(int scoreToAdd)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ballObj in balls)
        {
            // 如果不是自己也不是被碰到的目标，就销毁
            //if (ball != gameObject && ball != other.gameObject)
            //{

            //}

            Ball ball = ballObj.GetComponent<Ball>();
            if (ball != null)
            {
                ball.AddBallScore(scoreToAdd);
            }
        }

    }

    public void SubBallScore(int scoreToSub) 
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ballObj in balls)
        {
            // 如果不是自己也不是被碰到的目标，就销毁
            //if (ball != gameObject && ball != other.gameObject)
            //{

            //}

            Ball ball = ballObj.GetComponent<Ball>();
            if (ball != null)
            {
                ball.SubBallScore(scoreToSub);
            }
        }
    }

    public void MulBallScore(int scoreToMul)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ballObj in balls)
        {
            // 如果不是自己也不是被碰到的目标，就销毁
            //if (ball != gameObject && ball != other.gameObject)
            //{

            //}

            Ball ball = ballObj.GetComponent<Ball>();
            if (ball != null)
            {
                ball.MulBallScore(scoreToMul);
            }
        }
    }

    public void DivBallScore(int scoreToDiv)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ballObj in balls)
        {
            // 如果不是自己也不是被碰到的目标，就销毁
            //if (ball != gameObject && ball != other.gameObject)
            //{

            //}

            Ball ball = ballObj.GetComponent<Ball>();
            if (ball != null)
            {
                ball.DivBallScore(scoreToDiv);
            }
        }
    }
}
