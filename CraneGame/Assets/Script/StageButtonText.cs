using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonText : MonoBehaviour
{
    [SerializeField]private TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame  
    void Update()
    {
        
    }

    public void SetText(string _text)
    {
        text.text = "Stage " + _text;  
    }
}
