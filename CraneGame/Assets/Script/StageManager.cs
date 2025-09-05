using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    public GameObject hook;

    void Start()
    {
        //if (GameManager.Instance.currentConfig != null)
        //{
            var cfg = GameManager.Instance.currentConfig;
            for (int i = 0; i < cfg.enemyCount; i++)
            {
                Vector3 pos = new Vector3(i * 200, 0, 0);

                float hp = 100 * GameManager.Instance.currentConfig.enemyHealthMultiplier;
                Debug.Log($"生成敌人 {i + 1}, 血量: {hp}");

                Instantiate(enemyPrefab, pos, Quaternion.identity);
            }

            Instantiate(cfg.item1Prefab, cfg.pos1, Quaternion.identity);
            Instantiate(cfg.item2Prefab,cfg.pos2, Quaternion.identity);
            Instantiate(cfg.item3Prefab,cfg.pos3, Quaternion.identity);


        }
        //else
        //{
           // Debug.LogWarning("没有配置，无法初始化关卡！");
       // }
    //}





}


