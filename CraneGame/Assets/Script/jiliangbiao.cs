using UnityEngine;
using System.Collections;
public class jiliangbiao : Character
{
    [SerializeField] private int score;
    [SerializeField] private int clearScore;
    //public float moveDistance = 5f; // 想要移动的距离
    public float moveSpeed = 2f;    // 移动速度
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float yMoveDis;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        rb.freezeRotation = true;

        int dsa = 1, da = 2;
        float asd;
        asd = (float)dsa / da;
        Debug.Log(asd);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.Instance.GetIsMeasuring())
        {
            StartCoroutine(MoveToTarget());
        }
    }

    private IEnumerator MoveToTarget()
    {
        score = SceneManager.Instance.GetScore();
        clearScore = SceneManager.Instance.GetClearScore();

        float percentage = (float)score / clearScore;
        
        if (percentage >= 1) percentage = 1;
        else if(percentage < 0) percentage = 0;

        float moveAmount = percentage * yMoveDis; // 临时偏移量
        targetPos = rb.position + new Vector3(0, moveAmount, 0);

        //targetPos = rb.position + new Vector3(0, yMoveDis, 0);

        while ((rb.position - targetPos).sqrMagnitude > 0.001f)
        {
            rb.MovePosition(Vector3.MoveTowards(rb.position, targetPos, moveSpeed * Time.deltaTime));
            yield return null;
        }

        rb.MovePosition(targetPos); // 精确到目标位置
        SceneManager.Instance.SetIsMeasuring(false);


    }

}
