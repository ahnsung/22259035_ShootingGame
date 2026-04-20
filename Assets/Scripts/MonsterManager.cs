using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [Header("Normal Monster")]
    public GameObject prefabMonster;
    public Transform player;

    public float minTime = 1f;
    public float maxTime = 2f;

    private float nowTime;
    private float createTime;

    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    public int firstBossScore = 20;     // 첫 보스 등장 점수
    public int bossScoreInterval = 20;  // 다음 보스 등장 간격

    private int nextBossScore;
    private bool bossAlive = false;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        createTime = Random.Range(minTime, maxTime);
        nextBossScore = firstBossScore;
    }

    private void Update()
    {
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();

        // 보스가 없고, 점수가 기준 이상이면 보스 소환
        if (!bossAlive && scoreManager != null && scoreManager.nowScore >= nextBossScore)
        {
            SpawnBoss();
            nextBossScore += bossScoreInterval;
            return;
        }

        // 보스 살아있는 동안 일반 몬스터 생성 금지
        if (bossAlive)
            return;

        nowTime += Time.deltaTime;

        if (nowTime > createTime)
        {
            SpawnMonster();
            createTime = Random.Range(minTime, maxTime);
            nowTime = 0f;
        }
    }

    private void SpawnMonster()
    {
        if (prefabMonster == null)
            return;

        GameObject monster = Instantiate(prefabMonster, transform.position, Quaternion.identity);

        Monster monsterScript = monster.GetComponent<Monster>();
        if (monsterScript != null && player != null)
            monsterScript.target = player.gameObject;
    }

    private void SpawnBoss()
    {
        if (bossPrefab == null)
            return;

        bossAlive = true;

        // 보스 등장 직전에 필드 일반 몬스터 정리
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject m in monsters)
        {
            Destroy(m);
        }

        Vector3 spawnPos = transform.position;
        if (bossSpawnPoint != null)
            spawnPos = bossSpawnPoint.position;

        GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        BossMonster bossScript = bossObj.GetComponent<BossMonster>();
        if (bossScript != null)
        {
            bossScript.manager = this;

            if (player != null)
                bossScript.target = player;
        }

        nowTime = 0f;
    }

    public void NotifyBossDead()
    {
        bossAlive = false;
        nowTime = 0f;
        createTime = Random.Range(minTime, maxTime);
    }
}