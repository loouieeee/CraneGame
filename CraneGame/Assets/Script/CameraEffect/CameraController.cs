using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()   // 用 LateUpdate 确保摄像机在角色更新之后再动
    {
        // 目标位置 = 玩家位置 + 偏移量
        Vector3 desiredPosition = player.position + offset;

        if (!IngameManager.Instance.GetIsReachingStopLine())
            desiredPosition.x = transform.position.x;
        else
            desiredPosition = player.position + offset;

        

        // 平滑插值（防止生硬抖动）
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 设置摄像机位置
        transform.position = smoothedPosition;

        // 保持看向玩家（可选）
        //transform.LookAt(player);
    }
}
