using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PanelSub : MonoBehaviour
{

    public int numOfBall;

    private int deletedBall = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");

        foreach (GameObject ball in balls)
        {
            // 如果不是自己也不是被碰到的目标，就销毁
            if (ball != gameObject && ball != other.gameObject)
            {
                Destroy(ball);
                deletedBall++;

                if (deletedBall >= numOfBall)
                    return;

            }
        }

    }
}
