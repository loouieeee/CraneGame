using UnityEngine.UI;
using UnityEngine;

public class FreeScrollRect : ScrollRect
{
    [SerializeField] private float minY = -2300f;
    [SerializeField] private float maxY = -1000f;

    protected override void LateUpdate()
    {
        base.LateUpdate();
        Vector2 pos = content.anchoredPosition;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        content.anchoredPosition = pos;
    }
}
