using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{

    [SerializeField] private int score = 0;

    [SerializeField] private TMP_Text mytext;

    private Renderer render;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetmyText();
        render = GetComponent<Renderer>();
        SetColor();
        //render.material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < IngameManager.Instance.GetukezaraPos() -200)
        {
            PoolManager.Instance.ReturnObject("ball", gameObject);
        }
    }

    public void SetColor()
    {
        if (score < 20)
            render.material.color = Color.blue;
        else if (score >= 20 && score < 40)
            render.material.color = Color.green;
        else if (score >= 40 && score < 70)
            render.material.color = Color.yellow;
        else if (score >= 70)
            render.material.color = Color.red;
        else
            render.material.color = Color.white;
        
    }
    private void SetmyText()
    {
        mytext.text = GetBallScore().ToString();
    }

    public void SetBallScore(int _score)
    {
        this.score = _score;
        SetmyText();
        //SetColor();
    }
    public int GetBallScore()
    {
        return score;
    }

    public void AddBallScore(int _score)
    {
        this.score += _score;
        SetColor();
        SetmyText();
    }

    public void SubBallScore(int _score)
    {
        this.score -= _score;
        SetColor();
        SetmyText();
    }
    public void MulBallScore(int _score)
    {
        this.score *= _score;
        SetColor();
        SetmyText();
    }
    public void DivBallScore(int _score)
    {
        this.score /= _score;
        SetColor();
        SetmyText();
    }


}
