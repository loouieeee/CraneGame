using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class ScrollViewScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{

    private float movespeed;
    private int pageCount = 1;
    Transform content;
    private ScrollRect Rect;

    private float pageIndex;
    private bool isDrag = false;

    private List<float> PageList = new List<float> { 0 };
    private float targetPos = 0;
    private float nowindex = 0;

    private float beginDragPos;
    private float endDragPos;
    private float sensitivity = 0.5f;

    public Transform pointPos;
    public GameObject pointPrefab;
    public List<Image> renderers;
    public Transform scroll;
    void Awake()
    {
        Rect = this.GetComponent<ScrollRect>();
        InitPageList();
        movespeed = 10;
    }


    void InitPageList()
    {
        content = transform.Find("Viewport/Content");
        pageIndex = (content.childCount / pageCount) - 1;
        if (content != null && content.childCount != 0)
        {
            for (float i = 1; i <= pageIndex; i++)
            {
                PageList.Add((i / pageIndex));
            }
        }

        for(int i = 0; i < content.childCount; i++)
        {
            var gm = Instantiate(pointPrefab, pointPos.position + new Vector3(i * 30, 0, 0), Quaternion.identity, scroll);
            renderers.Add(gm.GetComponent<Image>());
        }
        renderers[0].color = Color.white;
        renderers[1].color = Color.gray;
    }

    void Update()
    {
        if (!isDrag)
        {
            Rect.horizontalNormalizedPosition = Mathf.Lerp(Rect.horizontalNormalizedPosition, targetPos, Time.deltaTime * movespeed);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        beginDragPos = Rect.horizontalNormalizedPosition;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        endDragPos = Rect.horizontalNormalizedPosition;
        endDragPos = endDragPos > beginDragPos ? endDragPos + sensitivity : endDragPos - sensitivity;
        float offset = endDragPos - beginDragPos;
        if (offset < 0)
        {
            nowindex = Mathf.Clamp(nowindex - 1, 0, pageIndex);
            targetPos = PageList[Convert.ToInt32(nowindex)];
            renderers[0].color = Color.white;
            renderers[1].color= Color.gray;
        }
        else if (offset > 0)
        {
            nowindex = Mathf.Clamp(nowindex + 1, 0, pageIndex);
            targetPos = PageList[Convert.ToInt32(nowindex)];
            renderers[0].color = Color.gray;
            renderers[1].color = Color.white;
        }
    }
}
