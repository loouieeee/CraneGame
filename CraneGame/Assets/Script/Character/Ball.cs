using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{

    [SerializeField] private int score = 0;

    [SerializeField] private TMP_Text mytext;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetmyText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetmyText()
    {
        mytext.text = GetBallScore().ToString();
    }

    public void SetBallScore(int _score)
    {
        this.score = _score;
        SetmyText();
    }
    public int GetBallScore()
    {
        return score;
    }

    public void AddBallScore(int _score)
    {
        this.score += _score;
        SetmyText();
    }

    public void SubBallScore(int _score)
    {
        this.score -= _score;
        SetmyText();
    }
    public void MulBallScore(int _score)
    {
        this.score *= _score;
        SetmyText();
    }
    public void DivBallScore(int _score)
    {
        this.score /= _score;
        SetmyText();
    }

}
