using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 100f;
    public float smoothTime = 0.3f;
    public float minY = -5f; // ✅ 下边界
    public float maxY = 5f;  // ✅ 上边界


    private Vector3 dragOrigin;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(0, -pos.y * dragSpeed, 0);

            targetPos = transform.position + move;

            // ✅ 限制 Y 范围
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

            dragOrigin = Input.mousePosition;
        }

        // 平滑移动（有惯性）
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
