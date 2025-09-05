using Unity.VisualScripting;
using UnityEngine;

public class PanelManager : MonoBehaviour
{


    public GameObject ballPerfab;

    public int numOfBall;

    public Vector3 pos;


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
        // 在触发物体的位置生成
        for (int i = 0; i < numOfBall; i++)
        {
            Instantiate(ballPerfab, pos, Quaternion.identity);

            Debug.Log("生成了一个新物体！");
        }
        
    }
}
