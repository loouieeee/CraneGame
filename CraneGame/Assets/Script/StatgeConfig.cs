using UnityEngine;

[CreateAssetMenu(fileName = "StageConfig", menuName = "Game/StageConfig")]
public class StageConfig : ScriptableObject
{
    [Header("关卡名字")]
    public string stageName;

    [Header("敌人数量")]
    public int enemyCount;

    [Header("敌人血量倍率")]
    public float enemyHealthMultiplier;

    [Header("敌人生成间隔")]
    public float spawnInterval;

    [Header("item1")]
    public GameObject item1Prefab;

    [Header("item2")]
    public GameObject item2Prefab;

    [Header("iteam3")]
    public GameObject item3Prefab;

    [Header("position1")]
    public Vector3 pos1;
    
    [Header("position2")]
    public Vector3 pos2;

    [Header("position3")]
    public Vector3 pos3;
}
