using UnityEngine;
using TMPro;

public class Boom : MonoBehaviour
{
    public TextMeshProUGUI boomCountUI;
    public int boomCnt = 3;

    private void Start()
    {
        boomCountUI.text = "Count : " + boomCnt;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseBoom();
        }
    }

    void UseBoom()
    {
        if (boomCnt <= 0) return;

        boomCnt--;
        boomCountUI.text = "Count : " + boomCnt;

        GameObject gameManager = GameObject.Find("GameManager");
        ScoreManager scoreManager = gameManager.GetComponent<ScoreManager>();

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject m in monsters)
        {
            // 점수 증가
            scoreManager.nowScore++;
            scoreManager.nowScoreUI.text = "Now Score : " + scoreManager.nowScore;

            if (scoreManager.nowScore > scoreManager.bestScore)
            {
                scoreManager.bestScore = scoreManager.nowScore;
                scoreManager.bestScoreUI.text = "Best Score : " + scoreManager.bestScore;
                PlayerPrefs.SetInt("BestScore", scoreManager.bestScore);
            }

            // 폭발 생성
            Monster monsterScript = m.GetComponent<Monster>();
            if (monsterScript != null)
            {
                GameObject explosion = Instantiate(monsterScript.prefabsExplosion);
                explosion.transform.position = m.transform.position;
            }

            Destroy(m);
        }
    }
}