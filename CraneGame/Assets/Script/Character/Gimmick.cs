using Unity.VisualScripting;
using UnityEngine;

public class Gimmick : Character
{
    [SerializeField] protected int subFinalScoreAmount;
    [SerializeField] protected int subBallScoreAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubFinalScore()
    {
        IngameManager.Instance.SubScore(subFinalScoreAmount);
    }

    public void SubBallScore()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
        foreach (GameObject ballObj in balls)
        {
            Ball ball = ballObj.GetComponent<Ball>();
            if (ball != null)
            {
                ball.SubBallScore(subBallScoreAmount);
            }
        }
    }

    public void SetSubFinalScore(int _subFinalScoreAmount)
    {
        subFinalScoreAmount = _subFinalScoreAmount;
    }

    public void SetSubBallScore(int _subBallScoreAmount)
    {
        subBallScoreAmount = _subBallScoreAmount;
    }
}
