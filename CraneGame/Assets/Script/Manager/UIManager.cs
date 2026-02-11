using TMPro;
using UnityEngine;
using System.Collections;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text scoreText;
    public TMP_Text panelText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }

    //public IEnumerator SpawnPanelText(string _text,Color _color,Color _underlayColor)
    //{
    //    panelText.gameObject.SetActive(true);

    //    Material mat = panelText.fontMaterial;
    //    mat.EnableKeyword("UNDERLAY_ON");
    //    mat.SetColor("_UnderlayColor", _underlayColor);


    //    var tmp = panelText.GetComponent<TMP_Text>();
    //    tmp.text = _text;
    //    tmp.color = _color;

    //    //Vector3 screenPos = new Vector3(0.5f, -0.1f, 10.0f);
    //    //Vector3 worldPos = Camera.main.ViewportToWorldPoint(screenPos);

    //    //panelText.transform.position = worldPos;
    //    panelText.transform.localScale = Vector3.zero;

    //    float duration1 = 0.33f;
    //    float timer = 0;

    //    Vector3 targetScale = Vector3.one * 1.2f;

    //    while (timer < duration1)
    //    {
    //        timer += Time.deltaTime;
    //        float t = timer / duration1;
    //        t = Mathf.SmoothStep(0, 1, t);

    //        panelText.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
    //        yield return null;
    //    }

    //    float duration2 = 0.16f;
    //    timer = 0;
    //    Vector3 startScale = panelText.transform.localScale;

    //    while (timer < duration2)
    //    {
    //        timer += Time.deltaTime;
    //        float t = timer / duration2;

    //        panelText.transform.localScale = Vector3.Lerp(startScale, Vector3.one, t);
    //        yield return null;
    //    }

    //    panelText.transform.localScale = Vector3.one;

    //    yield return new WaitForSeconds(1.5f);

    //    panelText.gameObject.SetActive(false);

    //}

    public IEnumerator SpawnPanelText(string _text, Color _color, Color _underlayColor)
    {
        panelText.gameObject.SetActive(true);

        // --- 1. 修改材质部分 ---
        // 获取材质实例
        Material mat = panelText.fontMaterial;

        // 开启 Underlay 功能
        mat.EnableKeyword("UNDERLAY_ON");

        // 设置 Underlay 颜色
        mat.SetColor("_UnderlayColor", _underlayColor);

        // 【关键】设置扩张度 (0.5f ~ 1.0f 比较合适)，否则可能看不见
        mat.SetFloat("_UnderlayDilate", 0.8f);

        // 【建议】设置柔和度为 0 (硬边)，如果是阴影可以设大一点
        mat.SetFloat("_UnderlaySoftness", 0f);

        // 【建议】把偏移归零，让描边在正中间
        mat.SetFloat("_UnderlayOffsetX", 0f);
        mat.SetFloat("_UnderlayOffsetY", 0f);

        // --- 2. 修改文字内容 ---
        // panelText 本身就是 TMP_Text 类型，直接用，不用 GetComponent
        panelText.text = _text;
        panelText.color = _color;

        // 【关键】防止描边太粗导致文字边缘被切掉
        panelText.UpdateMeshPadding();


        // --- 3. 动画部分 (保持你原来的逻辑) ---
        panelText.transform.localScale = Vector3.zero;

        float duration1 = 0.33f;
        float timer = 0;
        Vector3 targetScale = Vector3.one * 1.2f;

        while (timer < duration1)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, timer / duration1);
            panelText.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }

        float duration2 = 0.16f;
        timer = 0;
        Vector3 startScale = panelText.transform.localScale;

        while (timer < duration2)
        {
            timer += Time.deltaTime;
            float t = timer / duration2;
            panelText.transform.localScale = Vector3.Lerp(startScale, Vector3.one, t);
            yield return null;
        }

        panelText.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(1.5f);

        panelText.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
