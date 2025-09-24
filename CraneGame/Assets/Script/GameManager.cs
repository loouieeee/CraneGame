using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public StageConfig currentConfig;
    [SerializeField] private bool[] isStageClear = new bool[6];
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

    public enum GameLevel
    {
        Level_1, Level_2, Level_3,
        Level_4, Level_5,
        Level_6, Level_7,
        Level_8, Level_9,
        Level_10
    }
    public GameLevel gameLevel;

    
    public void SetGameLevel(GameLevel targetGameLevel)
    {
        gameLevel = targetGameLevel;
    }

    public int GetGameLevel()
    {
        switch (gameLevel)
        {
            case GameLevel.Level_1:
                return 1;
                case GameLevel.Level_2:
                return 2;
                case GameLevel.Level_3:
                return 3;
                case GameLevel.Level_4:
                return 4;
                case GameLevel.Level_5:
                return 5;
                case GameLevel.Level_6:
                return 6;
                case GameLevel.Level_7:
                return 7;
                case GameLevel.Level_8:
                return 8;
                case GameLevel.Level_9:
                return 9;
                case GameLevel.Level_10:
                return 10;
                default: return 0;
        }

        //return gameLevel;
    }

    public string GetLevelCsvPath()
    {
        //switch (GetGameLevel())
        //{
        //    case GameLevel.Level_1:

        //        return "A";

        //    case GameLevel.Level_2:

        //        return "Csves/Map_Normal";

        //    case GameLevel.Level_3:

        //        return "C";
        //}

        return "Csves/Map_Normal";


        //return null;
    }

    public void SetIsStageClear(int stageNum, bool isClear)
    {
        if (stageNum < 1 || stageNum > isStageClear.Length)
        {
            Debug.LogWarning("无效的关卡编号：" + stageNum);
            return;
        }

        isStageClear[stageNum - 1] = isClear; // -1 因为数组是从 0 开始
        //StageSelector.Instance.SetIsStageClear(stageNum, isClear);
    }

    public bool GetIsStageClear(int stageNum)
    {
        if (stageNum < 1 || stageNum > isStageClear.Length)
            return false;

        return isStageClear[stageNum - 1];
    }
}
