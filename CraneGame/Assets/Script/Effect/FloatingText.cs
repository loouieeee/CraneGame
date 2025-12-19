using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    public float moveUpSpeed = 0.5f;      // 向上漂浮的速度
    public float fadeDuration = 1.5f;     // 渐隐时间
    public float lifeTime = 2f;           // 总生命周期

    private TextMeshPro textMesh;
    private Color originalColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalColor = textMesh.color;
    }

    public void SetFloatingText(string text)
    {
        textMesh.text = text;
        StartCoroutine(FloatAndFade());
    }

    private IEnumerator FloatAndFade()
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            // 1️⃣ 向上漂浮
            transform.position = startPos + Vector3.up * (elapsed * moveUpSpeed);

            // 2️⃣ 渐隐透明
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        Destroy(gameObject, lifeTime - fadeDuration);
    }
}
