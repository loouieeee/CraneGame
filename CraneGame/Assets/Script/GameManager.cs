using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public StageConfig currentConfig;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentConfig = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
