using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public StageConfig currentConfig;
    [SerializeField] private bool[] isStageClear = new bool[20];
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentConfig = null;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        Application.targetFrameRate = 60;
    }

    public enum GameLevel
    {
        Level_1=1, Level_2, Level_3,
        Level_4, Level_5,
        Level_6, Level_7,
        Level_8, Level_9,
        Level_10, Level_11,
        Level_12, Level_13,
        Level_14, Level_15, Level_16,
        Level_17, Level_18, Level_19,
        Level_20, Level_21, Level_22
    }
    public GameLevel gameLevel;

    
    public void SetGameLevel(GameLevel targetGameLevel)
    {
        gameLevel = targetGameLevel;
    }

    //public int GetGameLevel()
    //{
    //    switch (gameLevel)
    //    {
    //        case GameLevel.Level_1:
    //            return 1;
    //            case GameLevel.Level_2:
    //            return 2;
    //            case GameLevel.Level_3:
    //            return 3;
    //            case GameLevel.Level_4:
    //            return 4;
    //            case GameLevel.Level_5:
    //            return 5;
    //            case GameLevel.Level_6:
    //            return 6;
    //            case GameLevel.Level_7:
    //            return 7;
    //            case GameLevel.Level_8:
    //            return 8;
    //            case GameLevel.Level_9:
    //            return 9;
    //            case GameLevel.Level_10:
    //            return 10;
    //            case GameLevel.Level_11:
    //            return 11;
    //            case GameLevel.Level_12: return 12;
    //            case GameLevel.Level_13: return 13;
    //            case GameLevel.Level_14: return 14;
    //            case GameLevel.Level_15: return 15;
    //            case GameLevel.Level_16: return 16;
    //            case GameLevel.Level_17: return 17;
    //            case GameLevel.Level_18: return 18;
    //            case GameLevel.Level_19: return 19;
    //            case GameLevel.Level_20: return 20;
    //            default: return 0;
    //    }

    //    //return gameLevel;
    //}

    //public string GetLevelCsvPath()
    //{
    //    switch (gameLevel)
    //    {
    //        case GameLevel.Level_1:
    //            return "Csves/Level_1";
    //        case GameLevel.Level_2:
    //            return "Csves/Level_2";
    //        case GameLevel.Level_3:
    //            return "Csves/Level_3";
    //        case GameLevel.Level_4:
    //            return "Csves/Level_4";
    //        case GameLevel.Level_5:
    //            return "Csves/Level_5";
    //        case GameLevel.Level_6:
    //            return "Csves/Level_6";
    //        case GameLevel.Level_7:
    //            return "Csves/Level_7";
    //        case GameLevel.Level_8:
    //            return "Csves/Level_8";
    //        case GameLevel.Level_9:
    //            return "Csves/Level_9";
    //        case GameLevel.Level_10:
    //            return "Csves/Level_10";
    //        case GameLevel.Level_11:
    //            return "Csves/Level_11";
    //        case GameLevel.Level_12:
    //            return "Csves/Level_12";
    //        case GameLevel.Level_13:
    //            return "Csves/Level_13";
    //        case GameLevel.Level_14:
    //            return "Csves/Level_14";
    //        case GameLevel.Level_15:
    //            return "Csves/Level_15";
    //        case GameLevel.Level_16:
    //            return "Csves/Level_16";
    //        case GameLevel.Level_17:
    //            return "Csves/Level_17";
    //        case GameLevel.Level_18:
    //            return "Csves/Level_18";
    //        case GameLevel.Level_19:
    //            return "Csves/Level_19";
    //        case GameLevel.Level_20:
    //            return "Csves/Level_20";
    //    }

    //    //return "Csves/Map_Normal";


    //    return null;
    //}

    // ------------------------
    // ✅ 简化版 GetGameLevel
    // ------------------------
    public int GetGameLevel()
    {
        return (int)gameLevel;
    }

    // ------------------------
    // ✅ 简化版 GetLevelCsvPath
    // ------------------------
    public string GetLevelCsvPath()
    {
        return $"Csves/Level_{(int)gameLevel}";
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
