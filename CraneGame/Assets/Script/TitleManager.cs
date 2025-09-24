using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{

    [SerializeField] private Button cridetButton;
    [SerializeField] private Button tapToStartButton;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text seisakuText;
    [SerializeField] private TMP_Text introduceText;
    [SerializeField] private Button closeButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init_TitleScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init_TitleScene()
    {
        titleText.gameObject.SetActive(true);
        tapToStartButton.gameObject.SetActive(true);
        cridetButton.gameObject.SetActive(true);

        seisakuText.gameObject.SetActive(false);
        introduceText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);

    }
    public void ClickOnCridetButton()
    {
        titleText.gameObject.SetActive(false);
        tapToStartButton.gameObject.SetActive(false);
        cridetButton.gameObject.SetActive(false);

        seisakuText.gameObject.SetActive(true);
        introduceText.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
    }

    public void ClickOnCloseButton()
    {
        titleText.gameObject.SetActive(true);
        tapToStartButton.gameObject.SetActive(true);
        cridetButton.gameObject.SetActive(true);

        seisakuText.gameObject.SetActive(false);
        introduceText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
    }


    public void LoadStageSelectScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
    }

}
