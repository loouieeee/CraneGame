using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{

    [SerializeField] private Button cridetButton;
    [SerializeField] private Button tapToStartButton;
    //[SerializeField] private TMP_Text titleText;
    //[SerializeField] private TMP_Text seisakuText;
    //[SerializeField] private TMP_Text introduceText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image graybackground;
    //[SerializeField] private SpriteRenderer titleLogo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init_TitleScene();
        //AudioManager.Instance.PlayBGM(AudioManager.Instance.TitleBGM);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("点击屏幕！");
            AudioManager.Instance.PlaySE(AudioManager.Instance.clickSE);
        }
    }

    public void Init_TitleScene()
    {
        //titleText.gameObject.SetActive(true);
        tapToStartButton.gameObject.SetActive(true);
        cridetButton.gameObject.SetActive(true);

        //seisakuText.gameObject.SetActive(false);
        //introduceText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        graybackground.gameObject.SetActive(false);
        //AudioManager.Instance.PlayBGM(AudioManager.Instance.TitleBGM);
        AudioManager.Instance.PlayBGM(AudioManager.Instance.TitleBGM,true,1);
        //AudioManager.Instance.PlayBGMLoop(AudioManager.Instance.TitleBGM);

    }
    public void ClickOnCridetButton()
    {
        //titleLogo.gameObject.SetActive(false);
        //titleText.gameObject.SetActive(false);
        tapToStartButton.gameObject.SetActive(false);
        cridetButton.gameObject.SetActive(false);

        graybackground.gameObject.SetActive(true);
        //seisakuText.gameObject.SetActive(true);
        //introduceText.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
    }

    public void ClickOnCloseButton()
    {
        //titleLogo.gameObject.SetActive(true);
        //titleText.gameObject.SetActive(true);
        tapToStartButton.gameObject.SetActive(true);
        cridetButton.gameObject.SetActive(true);

        graybackground.gameObject.SetActive(false);
        //seisakuText.gameObject.SetActive(false);
        //introduceText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
    }


    public void LoadStageSelectScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
    }

}
