using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI nowScoreUI;
    public TextMeshProUGUI bestScoreUI;

    public int nowScore;
    public int bestScore;

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        nowScoreUI.text = "Now Score : " + nowScore;
        bestScoreUI.text = "Best Score : " + bestScore;
    }

    public void AddScore(int amount)
    {
        nowScore += amount;

        if (nowScore > bestScore)
        {
            bestScore = nowScore;
            PlayerPrefs.SetInt("BestScore", bestScore);

            nowScoreUI.text = "Now Score : " + nowScore;
            bestScoreUI.text = "Best Score : " + bestScore;
        }
    }
}