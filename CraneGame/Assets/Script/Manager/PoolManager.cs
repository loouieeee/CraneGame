using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用对象池系统，可复用任意 GameObject（子弹、特效、敌人等）
/// 用法：
/// 1. 在场景中新建一个空物体，挂上本脚本。
/// 2. 在 Inspector 指定 prefab 和初始数量。
/// 3. 通过 UniversalObjectPool.Instance.GetObject("Bullet") 获取对象。
/// 4. 用完后调用 UniversalObjectPool.Instance.ReturnObject("Bullet", obj)。
/// </summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public string key;            //名
        public GameObject prefab;
        public int size = 10;         //数
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, Transform> parentDictionary;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        parentDictionary = new Dictionary<string, Transform>();

        foreach (Pool pool in pools)
        {
            // 自动创建父物体
            GameObject parent = new GameObject($"{pool.key}_Pool");
            parent.transform.SetParent(this.transform);  // 放在 PoolManager 下面
            parentDictionary.Add(pool.key, parent.transform);

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, parent.transform); // 自动挂在父物体下
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.key, objectPool);
        }
    }

    //public GameObject GetObject(string key, Vector3 position, Quaternion rotation)
    //{
    //    if (!poolDictionary.ContainsKey(key))
    //    {
    //        Debug.LogWarning($"Pool with key {key} doesn't exist!");
    //        return null;
    //    }

    //    //GameObject obj = poolDictionary[key].Count > 0 ?
    //    //    poolDictionary[key].Dequeue() :
    //    //    Instantiate(pools.Find(p => p.key == key).prefab);
    //    Pool poolConfig = pools.Find(p => p.key == key);

    //    GameObject obj = poolDictionary[key].Count > 0 ?
    //        poolDictionary[key].Dequeue() :
    //        Instantiate(poolConfig.prefab, poolConfig.parent);

    //    // 确保取出来的也在父节点下
    //    if (obj.transform.parent != poolConfig.parent)
    //        obj.transform.SetParent(poolConfig.parent);

    //    obj.transform.position = position;
    //    obj.transform.rotation = rotation;
    //    obj.SetActive(true);
    //    return obj;
    //}
    public GameObject GetObject(string key, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(key))
            return null;

        Transform parent = parentDictionary[key];

        GameObject obj = poolDictionary[key].Count > 0 ?
            poolDictionary[key].Dequeue() :
            Instantiate(pools.Find(p => p.key == key).prefab, parent);

        // 保证永远在正确父物体下面
        obj.transform.SetParent(parent);

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }


    public void ReturnObject(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Pool with key {key} doesn't exist!");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
    }
}

//🎯 二、使用方式示例

//1️⃣ 设置步骤
//	1.	在场景中新建一个空物体命名为 ObjectPoolManager。
//	2.	挂上 UniversalObjectPool.cs。
//	3.	在 Inspector 中添加多个池：
//	•	Key: "Bullet"，Prefab: 子弹预制体，Size: 50
//	•	Key: "Enemy"，Prefab: 敌人预制体，Size: 10
//	•	Key: "Explosion"，Prefab: 爆炸特效，Size: 20
//2️⃣ 从池中取对象
//    // 比如玩家开火
//GameObject bullet = UniversalObjectPool.Instance.GetObject("Bullet", firePoint.position, firePoint.rotation);
//bullet.GetComponent<Rigidbody>().velocity = firePoint.forward * 10f;

//3️⃣ 用完放回池
//    // 在子弹或特效脚本里
//void OnDisableOrExpire()
//{
//    UniversalObjectPool.Instance.ReturnObject("Bullet", gameObject);
//}