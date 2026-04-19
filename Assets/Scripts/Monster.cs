using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Monster : MonoBehaviour
{
    public float spd = 5.0f;
    public GameObject target;
    public GameObject prefabsExplosion;

    Vector3 direct = Vector3.down;
    public TextMeshProUGUI boomCountUI;
    public int boomCnt = 3;
    private void Start()
    {
        int rndNum = Random.Range(0, 10);

        if (rndNum % 3 == 0)
        {

            direct = target.transform.position - transform.position;
            direct.Normalize();
        }
}

    private void Update()
    {
        transform.position = transform.position + direct * spd * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            GameObject gameManager = GameObject.Find("GameManager");
            ScoreManager scoreManager = gameManager.GetComponent<ScoreManager>();

            scoreManager.nowScore++;
            scoreManager.nowScoreUI.text = "Now Score : " + scoreManager.nowScore;

            if (scoreManager.nowScore > scoreManager.bestScore)
            {
                scoreManager.bestScore = scoreManager.nowScore;
                scoreManager.bestScoreUI.text = "Best Score : " + scoreManager.bestScore;

                PlayerPrefs.SetInt("BestScore", scoreManager.bestScore);
            }

            GameObject explisionObj = Instantiate(prefabsExplosion);
            explisionObj.transform.position = transform.position;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            GameObject explisionObj = Instantiate(prefabsExplosion);
            explisionObj.transform.position = transform.position;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}

