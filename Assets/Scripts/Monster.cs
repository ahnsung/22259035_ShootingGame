using UnityEngine;

public class Monster : MonoBehaviour
{
    public float spd = 5.0f;
    public GameObject target;
    public GameObject prefabsExplosion;

    private Vector3 direct = Vector3.down;

    private void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj;
        }

        int rndNum = Random.Range(0, 10);

        // 가끔 플레이어를 향해 내려오게 하고,
        // 아니면 그냥 아래로 내려오게 해서 기존 느낌 유지
        if (rndNum % 3 == 0 && target != null)
        {
            direct = target.transform.position - transform.position;
            direct.z = 0f;
            direct.Normalize();
        }
    }

    private void Update()
    {
        transform.position += direct * spd * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
                scoreManager.AddScore(1);

            if (prefabsExplosion != null)
            {
                GameObject explosionObj = Instantiate(prefabsExplosion);
                explosionObj.transform.position = transform.position;
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            if (prefabsExplosion != null)
            {
                GameObject explosionObj = Instantiate(prefabsExplosion);
                explosionObj.transform.position = transform.position;
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}