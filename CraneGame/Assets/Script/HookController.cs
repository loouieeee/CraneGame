using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class HookController : Character, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //public static HookController instance;
    [SerializeField] private Renderer rend;

    [SerializeField] private float xInput;
    [SerializeField] private float zInput;

    [SerializeField] private GameObject hook1;
    [SerializeField] private GameObject hook2;

    [SerializeField] private Vector3 defAngle = new Vector3(0, 0, -10);
    [SerializeField] private Vector3 defAngle2 = new Vector3(0, 0, 10);
    [SerializeField] private Vector3 targetAngle = new Vector3(0, 0, -40);
    [SerializeField] private Vector3 targetAngle2 = new Vector3(0, 0, 40);
    [SerializeField] private float xLimit;
    [SerializeField] private float zLimit;

    [SerializeField] private float SphereColliderRadius = 1;
    [SerializeField] private Vector3 SphereCenter;

    [SerializeField] private bool isStop = false;
    [SerializeField] private bool isCatch = false;
    [SerializeField] private bool isReachResultLine = false;
    [SerializeField] private bool isReact = false;
    private Vector2 startPos;
    private Vector2 inputDir;

    public List<Ball> caughtBalls = new List<Ball>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE //pc输入
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

#elif UNITY_ANDROID || UNITY_IOS //手机
 if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                inputDir = Vector2.zero;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector2 delta = touch.position - startPos;
                inputDir = delta.normalized;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                inputDir = Vector2.zero;
            }

            xInput = inputDir.x;
            zInput = inputDir.y;
        }
        else
        {
            inputDir = Vector2.zero;
        }

#endif
        if (!isStop && SceneManager.Instance.GetIsGameStart())
        {
            if (transform.position.x > xLimit && xInput > 0 || transform.position.x < -xLimit && xInput < 0)
            {
                SetZeroVelocityXZ();
                xInput = 0;
            }
            else if (transform.position.z > zLimit && zInput > 0 || transform.position.z < -zLimit && zInput < 0)
            {
                SetZeroVelocityXZ();
                zInput = 0;
            }

            else
                rb.linearVelocity = new Vector3(xInput * xMoveSpeed, -yMoveSpeed, zInput * zMoveSpeed);
        }

        HookMovement();
        HookReturnToDef();
        ChangeColor();

    }

    private void SetZeroVelocityXZ()
    {
        rb.linearVelocity = new Vector3(0, -yMoveSpeed, 0);
    }
    private void ChangeColor()
    {
        if (transform.position.x > 0)
            rend.material.color = Color.blue;
        else if (transform.position.x < 0)
            rend.material.color = Color.red;
    }

    public void TakeDamage()
    {
        StartCoroutine(DisableColliderWithShake());
    }

    IEnumerator DisableColliderWithShake()
    {
        collider.enabled = false;
        yield return StartCoroutine(GetComponent<ShakeEffect>().Shake(0.2f, 0.7f));
        yield return new WaitForSeconds(0.7f);
        collider.enabled = true;
    }

    private void HookMovement()
    {
        if (isStop)
        {
            Quaternion targetRotation = Quaternion.Euler(targetAngle);
            Quaternion targetRotation2 = Quaternion.Euler(targetAngle2);

            //hook1.transform.rotation = Quaternion.Euler(targetAngle);
            hook1.transform.rotation = Quaternion.Lerp(hook1.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            //hook2.transform.rotation = Quaternion.Euler(targetAngle2);
            hook2.transform.rotation = Quaternion.Lerp(hook2.transform.rotation, targetRotation2, Time.deltaTime * rotateSpeed);

        }


    }

    public void HookReturnToDef()
    {
        if (isReachResultLine)
        {
            Quaternion targetRotation = Quaternion.Euler(defAngle);
            Quaternion targetRotation2 = Quaternion.Euler(defAngle2);

            hook1.transform.rotation = Quaternion.Lerp(hook1.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            hook2.transform.rotation = Quaternion.Lerp(hook2.transform.rotation, targetRotation2, Time.deltaTime * rotateSpeed);

            Debug.Log("reachResultLine");
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("stop") && !isStop)
        {
            Debug.Log("stop");
            isStop = true;
            SetZeroVelocity();

            if (!isCatch)
            {
                SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
                sphere.radius = SphereColliderRadius;
                sphere.center = SphereCenter;
                sphere.isTrigger = true;
                isCatch = true;
            }

            StartCoroutine(GrabBalls(2f));

        }

        if (other.gameObject.CompareTag("ball"))
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                AddCaughtBalls(ball);
            }
        }

        if (other.gameObject.CompareTag("resultLine"))
        {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
            SetZeroVelocity();
            RemoveCaughtBalls();
            isReachResultLine = true;

            StartCoroutine(ShowResultScreen(3f));
        }
        #region fix
        if (other.gameObject.CompareTag("LeftPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetLeftPanelCode();
            int panelValue = panel.GetLeftPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);

            //switch (panelCode)
            //{
            //    case 1: panel.AddBallScore(panel.GetLeftPanelValue()); break;
            //    case 2: panel.SubBallScore(panel.GetLeftPanelValue()); break;
            //    case 3: panel.MulBallScore(panel.GetLeftPanelValue()); break;
            //    case 4: panel.DivBallScore(panel.GetLeftPanelValue()); break;
            //    case 5: panel.CreateBall(panel.GetLeftPanelValue()); break;
            //    case 6: panel.SubBall(panel.GetLeftPanelValue()); break;
            //    case 7: panel.MulBall(panel.GetLeftPanelValue()); break;
            //    case 8: panel.DivBall(panel.GetLeftPanelValue()); break;

            //    default: break;
            //}
            //panel.CreateBall(panel.GetLeftPanelValue());
            Debug.Log("111");
        }
        if (other.gameObject.CompareTag("RightPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetRightPanelCode();
            int panelValue = panel.GetRightPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);

            //switch (panelCode)
            //{
            //    case 1: panel.AddBallScore(panel.GetRightPanelValue()); break;
            //    case 2: panel.SubBallScore(panel.GetRightPanelValue()); break;
            //    case 3: panel.MulBallScore(panel.GetRightPanelValue()); break;
            //    case 4: panel.DivBallScore(panel.GetRightPanelValue()); break;
            //    case 5: panel.CreateBall(panel.GetRightPanelValue()); break;
            //    case 6: panel.SubBall(panel.GetRightPanelValue()); break;
            //    case 7: panel.MulBall(panel.GetRightPanelValue()); break;
            //    case 8: panel.DivBall(panel.GetRightPanelValue()); break;

            //    default: break;
            //}

            Debug.Log("222");
        }

        if (other.gameObject.CompareTag("upperLeftPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetUpperLeftPanelCode();
            int panelValue = panel.GetUpperLeftPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);

            //switch (panelCode)
            //{
            //    case 1: panel.AddBallScore(panel.GetUpperLeftPanelValue()); break;
            //    case 2: panel.SubBallScore(panel.GetUpperLeftPanelValue()); break;
            //    case 3: panel.MulBallScore(panel.GetUpperLeftPanelValue()); break;
            //    case 4: panel.DivBallScore(panel.GetUpperLeftPanelValue()); break;
            //    case 5: panel.CreateBall(panel.GetUpperLeftPanelValue()); break;
            //    case 6: panel.SubBall(panel.GetUpperLeftPanelValue()); break;
            //    case 7: panel.MulBall(panel.GetUpperLeftPanelValue()); break;
            //    case 8: panel.DivBall(panel.GetUpperLeftPanelValue()); break;

            //    default: break;
            //}

        }

        if (other.gameObject.CompareTag("upperRightPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetUpperRightPanelCode();
            int panelValue = panel.GetUpperRightPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);
        }
        if (other.gameObject.CompareTag("lowerLeftPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetLowerLeftPanelCode();
            int panelValue = panel.GetLowerLeftPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);
        }
        if (other.gameObject.CompareTag("lowerRightPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetLowerRightPanelCode();
            int panelValue = panel.GetLowerRightPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);
        }
        #endregion
        //PanelManager panel = other.GetComponentInParent<PanelManager>();
        //if (panel != null)
        //{
        //    Debug.Log("111");
        //    string tag = other.tag;
        //    int code = 0, value = 0;
        //    switch (tag)
        //    {
        //        case "LeftPanel": code = panel.GetLeftPanelCode(); value = panel.GetLeftPanelValue(); break;
        //        case "RightPanel": code = panel.GetRightPanelCode(); value = panel.GetRightPanelValue(); break;
        //        case "upperLeftPanel": code = panel.GetUpperLeftPanelCode(); value = panel.GetUpperLeftPanelValue(); break;
        //        case "upperRightPanel": code = panel.GetUpperRightPanelCode(); value = panel.GetUpperRightPanelValue(); break;
        //        case "lowerLeftPanel": code = panel.GetLowerLeftPanelCode(); value = panel.GetLowerLeftPanelValue(); break;
        //        case "lowerRightPanel": code = panel.GetLowerRightPanelCode(); value = panel.GetLowerRightPanelValue(); break;
        //        default: return;
        //    }
        //    ExecutePanelAction(panel, code, value);
        //}

        if (other.gameObject.CompareTag("SpikeBall"))
        {
            Gimmick_SpikeBall spikeBall = other.gameObject.GetComponent<Gimmick_SpikeBall>();
            AddSpikeBall(spikeBall);
        }

        if (other.gameObject.CompareTag("SlideStick"))
        {
            Gimmick_SlideStick slideStick = other.gameObject.GetComponent<Gimmick_SlideStick>();
            slideStick.SubBallScore();
            TakeDamage();
        }

        if (other.gameObject.CompareTag("Sickle"))
        {
            Gimmick_Sickle sickle = other.gameObject.GetComponent<Gimmick_Sickle>();
            sickle.SubFinalScore();
            TakeDamage();
        }
    }

    private void ExecutePanelAction(PanelManager panel, int code, int value)
    {
        switch (code)
        {
            case 1: panel.AddBallScore(value); break;
            case 2: panel.SubBallScore(value); break;
            case 3: panel.MulBallScore(value); break;
            case 4: panel.DivBallScore(value); break;
            case 5: panel.CreateBall(value); break;
            case 6: panel.SubBall(value); break;
            case 7: panel.MulBall(value); break;
            case 8: panel.DivBall(value); break;
            default: break;
        }

        Debug.Log($"Panel code: {code}, value: {value}, tag: {panel.gameObject.tag}");
    }
    public void AddSpikeBall(Gimmick_SpikeBall spikeBall)
    {
        Rigidbody rb = spikeBall.GetComponent<Rigidbody>();
        Collider collider = rb.GetComponent<Collider>();
        collider.enabled = false;
        // 让球跟随玩家
        spikeBall.transform.SetParent(this.transform);
        rb.isKinematic = true;
    }

    IEnumerator ShowResultScreen(float delay)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.Instance.SetIsMeasuring(true);

        yield return new WaitForSeconds(delay);
        SceneManager.Instance.ShowResultScreen();
    }

    IEnumerator GrabBalls(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("延迟 " + delay + " 秒后执行动作！");

        rb.linearVelocity = new Vector3(0,30,0);
        //ZeroVelocity();
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
        yield return new WaitForSeconds(0.5f);
        rb.linearVelocity = new Vector3(30,0,0);

        yield return new WaitForSeconds(1f);
        collider.enabled = true;
        //yield return new WaitForSeconds(4f);
        //ZeroVelocity();
    }

    public void AddCaughtBalls(Ball ball)
    {
        collider.enabled = false;
        if (!caughtBalls.Contains(ball))
        {
            caughtBalls.Add(ball);
            Debug.Log("addBall" + ball.name);

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Collider collider = rb.GetComponent<Collider>();
            collider.enabled = false;
            // 让球跟随玩家
            ball.transform.SetParent(this.transform);
            rb.isKinematic = true;
            // 可选：把球的位置对齐到玩家前方
            //ball.transform.localPosition = new Vector3(0, 1, 1);

            SceneManager.Instance.AddScore(ball.GetBallScore());
        }
    }

    public void RemoveCaughtBalls()
    {
        //if (caughtBalls.Contains(ball))
        //{
        //    caughtBalls.Remove(ball);
        //    Debug.Log("removeBall");

        //    Rigidbody rb = ball.GetComponent<Rigidbody>();

        //    ball.transform.SetParent(null);
        //    rb.isKinematic = false;

        //}

        foreach (Ball ball in caughtBalls)
        {
            if (ball != null)
            {
                // 1. 脱离父物体
                ball.transform.SetParent(null);

                // 2. 还原物理
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }

                Debug.Log("释放球: " + ball.name);
            }
        }

        // 3. 清空列表
        caughtBalls.Clear();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 处理按下
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 处理抬起
    }
}
