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

        if (nowScoreUI != null)
            nowScoreUI.text = "Now Score : " + nowScore;

        if (bestScoreUI != null)
            bestScoreUI.text = "Best Score : " + bestScore;
    }

    public void AddScore(int amount)
    {
        nowScore += amount;

        if (nowScoreUI != null)
            nowScoreUI.text = "Now Score : " + nowScore;

        if (nowScore > bestScore)
        {
            bestScore = nowScore;

            if (bestScoreUI != null)
                bestScoreUI.text = "Best Score : " + bestScore;

            PlayerPrefs.SetInt("BestScore", bestScore);
        }
    }
}