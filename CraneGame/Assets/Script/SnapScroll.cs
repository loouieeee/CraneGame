using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class SnapScrollAuto : MonoBehaviour, IEndDragHandler, IPointerUpHandler
{
    public RectTransform content;        // 可留空，会自动使用 ScrollRect.content
    public float snapSpeed = 10f;
    private ScrollRect scrollRect;
    private int itemCount;
    private float itemWidth;
    private Vector2 targetPos;
    private bool isSnapping = false;

    IEnumerator Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        if (content == null) content = scrollRect.content;

        // 强制刷新布局，等待一帧以确保 LayoutGroup / ContentSizeFitter 完成布局
        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        itemCount = content.childCount;

        if (itemCount >= 2)
        {
            RectTransform a = content.GetChild(0).GetComponent<RectTransform>();
            RectTransform b = content.GetChild(1).GetComponent<RectTransform>();
            itemWidth = Mathf.Abs(b.anchoredPosition.x - a.anchoredPosition.x);

            // 备用方案：如果 anchoredPosition 差为 0，则用 rect.width + spacing（若有 HorizontalLayoutGroup）
            if (Mathf.Approximately(itemWidth, 0f))
            {
                float spacing = 0f;
                var h = content.GetComponent<HorizontalLayoutGroup>();
                if (h != null) spacing = h.spacing;
                itemWidth = a.rect.width + spacing;
            }
        }
        else if (itemCount == 1)
        {
            itemWidth = content.GetChild(0).GetComponent<RectTransform>().rect.width;
        }
    }

    void Update()
    {
        if (isSnapping)
        {
            content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, targetPos, Time.deltaTime * snapSpeed);
            if ((content.anchoredPosition - targetPos).sqrMagnitude < 0.04f)
            {
                content.anchoredPosition = targetPos;
                isSnapping = false;
                if (scrollRect != null) scrollRect.velocity = Vector2.zero;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TrySnap();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TrySnap();
    }

    private void TrySnap()
    {
        if (content == null || itemCount == 0 || itemWidth <= 0f)
        {
            Debug.LogWarning("SnapScrollAuto: 无效状态（content/childCount/itemWidth）. childCount=" + (content != null ? content.childCount.ToString() : "null") + " itemWidth=" + itemWidth);
            return;
        }

        float posX = content.anchoredPosition.x;
        int index = Mathf.RoundToInt(-posX / itemWidth);
        index = Mathf.Clamp(index, 0, itemCount - 1);
        targetPos = new Vector2(-index * itemWidth, content.anchoredPosition.y);
        isSnapping = true;
    }
}
