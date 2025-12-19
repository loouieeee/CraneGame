using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class jiliangbiao : Character
{
    [SerializeField] private Transform parent;
    [SerializeField] private int score;
    [SerializeField] private int clearScore;
    [SerializeField] private int jiceng;
    //public float moveDistance = 5f; // 想要移动的距离
    public float moveSpeed = 2f;    // 移动速度
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float yMoveDis;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float interval = 0.5f;  // 每隔多少秒激活一个
    private Coroutine activateRoutine;
    protected override void Start()
    {
        base.Start();
        //rb.freezeRotation = true;
        for (int i = 0; i < parent.childCount; i++)
            parent.GetChild(i).gameObject.SetActive(false);
        Debug.Log("子物体数量：" + transform.childCount);
    }

    bool isPlayed = false;

    // Update is called once per frame
    void Update()
    {
        if (IngameManager.Instance.GetIsMeasuring()&&!isPlayed)
        {
            isPlayed = true;
            //StartCoroutine(MoveToTarget());
            SetMeasureActive();
        }
    }

    //private IEnumerator MoveToTarget()
    //{
    //    score = SceneManager.Instance.GetScore();
    //    clearScore = SceneManager.Instance.GetClearScore();

    //    float percentage = (float)score / clearScore;

    //    if (percentage >= 1) percentage = 1;
    //    else if(percentage < 0) percentage = 0;

    //    float moveAmount = percentage * yMoveDis; // 临时偏移量
    //    targetPos = rb.position + new Vector3(0, moveAmount, 0);

    //    //targetPos = rb.position + new Vector3(0, yMoveDis, 0);

    //    while ((rb.position - targetPos).sqrMagnitude > 0.001f)
    //    {
    //        rb.MovePosition(Vector3.MoveTowards(rb.position, targetPos, moveSpeed * Time.deltaTime));
    //        yield return null;
    //    }

    //    rb.MovePosition(targetPos); // 精确到目标位置
    //    SceneManager.Instance.SetIsMeasuring(false);


    //}

    private IEnumerator MoveToTarget()
    {
        // 获取分数比例
        score = IngameManager.Instance.GetScore();
        clearScore = IngameManager.Instance.GetClearScore();

        float percentage = Mathf.Clamp01((float)score / clearScore); // 0~1限制

        float moveAmount = percentage * yMoveDis; // 偏移量
        Vector3 targetPos = rb.position + new Vector3(0, moveAmount, 0);

        // 精度阈值（平方距离）
        float threshold = 0.0001f;

        // 循环移动直到接近目标
        while ((rb.position - targetPos).sqrMagnitude > threshold)
        {
            // 使用 FixedDeltaTime 确保物理同步
            Vector3 newPos = Vector3.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            // 等待下一个 FixedUpdate，物理系统同步
            yield return new WaitForFixedUpdate();
        }

        // 精确设置最终位置
        rb.MovePosition(targetPos);

        // 移动完成后通知场景管理器
        IngameManager.Instance.SetIsMeasuring(false);
    }



    public void SetMeasureActive()
    {
        score = IngameManager.Instance.GetScore();
        clearScore = IngameManager.Instance.GetClearScore();

        float percentage = Mathf.Clamp01((float)score / clearScore);
        int numToActive = (int)(jiceng * percentage);

        Debug.Log(numToActive);

        // 如果有上一次的协程在跑，先停止它
        if (activateRoutine != null)
            StopCoroutine(activateRoutine);

        // 启动新的激活协程
        activateRoutine = StartCoroutine(ActivateChildren(numToActive));

    }

    private IEnumerator ActivateChildren(int count)
    {
        //// 先全部关闭
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).gameObject.SetActive(false);
        //}
        //Debug.Log("进来了, 子物体数量: " + parent.childCount);

        // 按顺序逐个开启
        for (int i = 0; i < count && i < transform.childCount; i++)
        {
            yield return new WaitForSeconds(interval);
            transform.GetChild(i).gameObject.SetActive(true);
            //Debug.Log("激活");
        }

        IngameManager.Instance.SetIsMeasuring(false);

    }

}
