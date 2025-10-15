using UnityEngine;

public class CloseCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hook"))
        {
            // 获取最上层父物体（或者根对象）
            var root = transform.root;
            Collider[] colliders = root.GetComponentsInChildren<Collider>();

            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
        }
    }
}
