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
        bestScoreUI.text = "Best Score : " + bestScore;
    }
}