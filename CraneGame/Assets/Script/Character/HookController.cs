using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class HookController : Character, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    // フック本体の描画用レンダラー

    // X軸の入力値（キーボードまたはタッチ）
    [SerializeField] private float xInput;
    // Z軸の入力値（キーボードまたはタッチ）
    [SerializeField] private float zInput;

    public float zOffsetToLiangtong;
    // 左側のフックオブジェクト
    [SerializeField] private GameObject hook1;
    // 右側のフックオブジェクト
    [SerializeField] private GameObject hook2;

    // 左フックの初期角度
    [SerializeField] private Vector3 armDefAngle_left;
    // 右フックの初期角度
    [SerializeField] private Vector3 armDefAngle_right;
    [SerializeField] private Transform armDefPos_left;
    [SerializeField] private Transform armDefPos_right;
    // 左フックのターゲット角度（停止時に開く角度）
    [SerializeField] private Vector3 armTargetAngle_left = new Vector3(0, 0, -40);
    // 右フックのターゲット角度（停止時に開く角度）
    [SerializeField] private Vector3 armTargetAngle_right = new Vector3(0, 0, 40);
    [SerializeField] private Vector3 armTargetPos_left;
    [SerializeField] private Vector3 armTargetPos_right;

    // X方向の移動制限距離
    [SerializeField] private float xLimit;
    // Z方向の移動制限距離
    [SerializeField] private float zLimit;

    // 一時的に追加するSphereColliderの半径
    [SerializeField] private float SphereColliderRadius = 1;
    // SphereColliderの中心オフセット
    [SerializeField] private Vector3 SphereCenter;

    // 移動停止中かどうか
    [SerializeField] private bool isStop = false;
    // ボールをキャッチしたかどうか
    [SerializeField] private bool isCatch = false;
    // リザルトラインに到達したかどうか
    [SerializeField] private bool isReachResultLine = false;
    // 反応中かどうか（未使用フラグ）
    [SerializeField] private bool isReact = false;

    [SerializeField] private bool isArmMoveDownSEPlayed = false;
    [SerializeField] private bool isArmMoveHorizontalSEPlayed = false;

    public bool isAnimPlayed;
    public bool isAnimPlayed1;
    public Animator anim;
    // タッチ開始位置
    private Vector2 startPos;
    // 入力方向ベクトル
    private Vector2 inputDir;

    // 捕まえたボールのリスト
    public List<Ball> caughtBalls = new List<Ball>();

    // 各ボールのスコア（文字列形式）
    public List<string> ballScores = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        yMoveSpeed = IngameManager.Instance.GetHookMoveSpeed();
        Debug.Log( yMoveSpeed);
        armDefAngle_left = hook1.transform.eulerAngles;
        armDefAngle_right = hook2.transform.eulerAngles;
        anim = GetComponent<Animator>();
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
        if (!isStop && IngameManager.Instance.GetIsGameStart())
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

            if(!isArmMoveDownSEPlayed)
            {
                //AudioManager.Instance.PlaySE(AudioManager.Instance.craneMoveDownSE, true);
                isArmMoveDownSEPlayed = true;
            }
        }

       // if((xInput != 0||zInput != 0) && !isArmMoveHorizontalSEPlayed) { AudioManager.Instance.PlaySE(AudioManager.Instance.craneMoveHorizontalSE, true); isArmMoveHorizontalSEPlayed = true; }
        
       // if(xInput == 0 && zInput == 0) { isArmMoveHorizontalSEPlayed = false; AudioManager.Instance.StopSE(AudioManager.Instance.craneMoveHorizontalSE); }
        HookMovement();
        HookReturnToDef();
        //ChangeColor();
        IngameManager.Instance.SetEffectPos(transform);
    }

    // XZ方向の速度をゼロにする（落下のみを残す）
    private void SetZeroVelocityXZ()
    {
        rb.linearVelocity = new Vector3(0, -yMoveSpeed, 0);
    }

    // X位置に応じてオブジェクトの色を変える
    private void ChangeColor()
    {
        if (transform.position.x > 0)
            rend.material.color = Color.blue;
        else if (transform.position.x < 0)
            rend.material.color = Color.red;
    }

    // ダメージを受けた際の処理（シェイク演出付き）
    public void TakeDamage()
    {
        StartCoroutine(DisableColliderWithShake());
    }

    // 一時的にコライダーを無効化し、揺れ効果を再生する
    IEnumerator DisableColliderWithShake()
    {
        collider.enabled = false;
        yield return StartCoroutine(GetComponent<ShakeEffect>().Shake(0.2f, 0.7f));
        yield return new WaitForSeconds(0.7f);
        collider.enabled = true;
    }

    // フックが停止中の動作（左右のフックを開く）
    private void HookMovement()
    {
        if (isStop && !isAnimPlayed)
        {
            isAnimPlayed = true;
            //AudioManager.Instance.StopSE();
            //AudioManager.Instance.PlaySE(AudioManager.Instance.craneCloseSE,true);

            //Quaternion targetRotation_left = Quaternion.Euler(armTargetAngle_left);
            //Quaternion targetRotation_right = Quaternion.Euler(armTargetAngle_right);

            //hook1.transform.localRotation = Quaternion.Lerp(hook1.transform.localRotation, targetRotation_left, Time.deltaTime * rotateSpeed);
            //hook2.transform.localRotation = Quaternion.Lerp(hook2.transform.localRotation, targetRotation_right, Time.deltaTime * rotateSpeed);

            anim.Play("CraneArmMove");
        }
    }

    // リザルトラインに到達した際、フックを初期角度に戻す
    public void HookReturnToDef()
    {
        if (isReachResultLine && !isAnimPlayed1)
        {
            isAnimPlayed1 = true;

            //AudioManager.Instance.StopSE();
            //AudioManager.Instance.PlaySE(AudioManager.Instance.craneOpenSE, true);

            //Quaternion targetRotation_left = Quaternion.Euler(armDefAngle_left);
            //Quaternion targetRotation_right = Quaternion.Euler(armDefAngle_right);

            //hook1.transform.localRotation = Quaternion.Lerp(hook1.transform.localRotation, targetRotation_left, Time.deltaTime * rotateSpeed);
            //hook2.transform.localRotation = Quaternion.Lerp(hook2.transform.localRotation, targetRotation_right, Time.deltaTime * rotateSpeed);

            anim.Play("CraneArmMove_Anti");

        }
    }

    // 衝突判定の処理
    private void OnTriggerEnter(Collider other)
    {
        // 「stop」タグに当たったときの処理
        if (other.gameObject.CompareTag("stop") && !isStop)
        {
            AudioManager.Instance.StopSE(AudioManager.Instance.craneMoveDownSE);

            Debug.Log("stop");
            isStop = true;
            SetZeroVelocity();
            IngameManager.Instance.closeLockOnMark();
            IngameManager.Instance.SetIsReachingStopLine(true);

            // まだキャッチしていない場合は一時的にSphereColliderを追加
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

        // 「ball」タグ：ボールをキャッチした場合
        if (other.gameObject.CompareTag("ball"))
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                AddCaughtBalls(ball);
            }
        }

        // 「resultLine」タグ：結果ラインに到達した場合
        if (other.gameObject.CompareTag("resultLine"))
        {
            AudioManager.Instance.StopSE(AudioManager.Instance.craneMoveDownSE);

            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
            SetZeroVelocity();
            RemoveCaughtBalls();
            isReachResultLine = true;

            StartCoroutine(IngameManager.Instance.SpawnFloatingText(1f));
            StartCoroutine(ShowResultScreen(1f));
        }

        #region パネル処理
        // 左パネルとの接触時
        if (other.gameObject.CompareTag("LeftPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetLeftPanelCode();
            int panelValue = panel.GetLeftPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);
            //Debug.Log("111");
        }
        // 右パネルとの接触時
        if (other.gameObject.CompareTag("RightPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetRightPanelCode();
            int panelValue = panel.GetRightPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);
            //Debug.Log("222");
        }
        // 左上パネルとの接触時
        if (other.gameObject.CompareTag("upperLeftPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetUpperLeftPanelCode();
            int panelValue = panel.GetUpperLeftPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);
        }
        // 右上パネルとの接触時
        if (other.gameObject.CompareTag("upperRightPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetUpperRightPanelCode();
            int panelValue = panel.GetUpperRightPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);
        }
        // 左下パネルとの接触時
        if (other.gameObject.CompareTag("lowerLeftPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetLowerLeftPanelCode();
            int panelValue = panel.GetLowerLeftPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);
        }
        // 右下パネルとの接触時
        if (other.gameObject.CompareTag("lowerRightPanel"))
        {
            PanelManager panel = other.GetComponentInParent<PanelManager>();
            int panelCode = panel.GetLowerRightPanelCode();
            int panelValue = panel.GetLowerRightPanelValue();
            ExecutePanelAction(panel, panelCode, panelValue);
        }
        #endregion

        // トゲ付きボールとの接触
        if (other.gameObject.CompareTag("SpikeBall"))
        {
            Gimmick_SpikeBall spikeBall = other.gameObject.GetComponent<Gimmick_SpikeBall>();
            AddSpikeBall(spikeBall);
        }

        // スライド棒との接触
        if (other.gameObject.CompareTag("SlideStick"))
        {
            Gimmick_SlideStick slideStick = other.gameObject.GetComponent<Gimmick_SlideStick>();
            slideStick.SubBallScore();
            TakeDamage();
        }

        // 鎌ギミックとの接触
        if (other.gameObject.CompareTag("Sickle"))
        {
            Gimmick_Sickle sickle = other.gameObject.GetComponent<Gimmick_Sickle>();
            sickle.SubFinalScore();
            TakeDamage();
        }
    }

    // パネルコードに応じて対応する処理を実行
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

        AudioManager.Instance.PlaySE(AudioManager.Instance.panelAreaEffectSE);

        IngameManager.Instance.PlusCurrentList();

        Handheld.Vibrate();
    }

    // トゲ付きボールをプレイヤーに付着させる
    public void AddSpikeBall(Gimmick_SpikeBall spikeBall)
    {
        Rigidbody rb = spikeBall.GetComponent<Rigidbody>();
        Collider collider = rb.GetComponent<Collider>();
        collider.enabled = false;
        spikeBall.transform.SetParent(this.transform);
        rb.isKinematic = true;
    }

    // リザルト画面を表示するまでの演出処理
    IEnumerator ShowResultScreen(float delay)
    {
        yield return new WaitForSeconds(1f);
        IngameManager.Instance.SetIsMeasuring(true);

        yield return new WaitForSeconds(3f);
        IngameManager.Instance.SetClearOrNotText();

        yield return new WaitForSeconds(delay);
        IngameManager.Instance.SetClearOrNotTextOff();
        IngameManager.Instance.ShowResultScreen();
    }

    // 一定時間後にボールを掴み上げて動作する処理
    IEnumerator GrabBalls(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("延迟 " + delay + " 秒后执行动作！");
        MoveBallstoCenter();
        rb.linearVelocity = new Vector3(0, 30, 0);

        //AudioManager.Instance.PlaySE(AudioManager.Instance.craneMoveDownSE, true);

        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
        yield return new WaitForSeconds(0.5f);
        rb.linearVelocity = Vector3.zero;
        //rb.linearVelocity = new Vector3(30, 0, 0);
        StartCoroutine(MoveHookToLiangtong(50f));
        //yield return new WaitForSeconds(1f);
        collider.enabled = true;

    }

    // 捕まえたボールをリストに追加し、スコアを加算
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
            ball.transform.SetParent(this.transform);
            rb.isKinematic = true;

            //movetocenter(ball);

            IngameManager.Instance.AddScore(ball.GetBallScore());
        }
    }

    private void MoveBallstoCenter()
    {
        Collider catcherCollider = GetComponent<Collider>();
        
        foreach(Ball ball in caughtBalls)
        {
            ball.transform.position = catcherCollider.bounds.center - new Vector3(0, 3, 0);
        }
    }

    // 捕まえた全てのボールを解放する
    public void RemoveCaughtBalls()
    {
        foreach (Ball ball in caughtBalls)
        {
            if (ball != null)
            {
                ball.transform.SetParent(null);
                float randXpos = UnityEngine.Random.Range(-6f, 6f);
                float randYpos = UnityEngine.Random.Range(-3f, 3f);
                float randZpos = UnityEngine.Random.Range(-6f, 6f);
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
                Debug.Log("释放球: " + ball.name);
                ball.transform.position += new Vector3(randXpos, randYpos, randZpos); 
            }
        }
        caughtBalls.Clear();
    }

    // ドラッグ操作時のイベント（未実装）
    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    // クリックまたはタップ開始時の処理
    public void OnPointerDown(PointerEventData eventData)
    {
        // 处理按下
    }

    // クリックまたはタップ終了時の処理
    public void OnPointerUp(PointerEventData eventData)
    {
        // 处理抬起
    }

    // 捕まえたボールのスコアリストを返す
    public List<string> GetBallScores()
    {
        return ballScores;
    }

    IEnumerator MoveHookToLiangtong(float speed)
    {
        Vector3 target = IngameManager.Instance.GetLiangtongPosition();
        Vector3 dis = new Vector3(target.x,target.y + 25, target.z + 52);
        // 保持当前 Z 偏移
        target.z = transform.position.z;

        while (Vector3.Distance(transform.position, dis) > 0.05f)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, dis, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = dis;
    }

}
