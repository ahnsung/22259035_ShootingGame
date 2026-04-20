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

    [Header("Boss Spawn")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    public int firstBossScore = 20;
    public int bossScoreInterval = 20;

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

        if (Input.GetKeyDown(KeyCode.V))
        {
            DebugSpawnBoss();
        }

        if (!bossAlive && scoreManager != null && scoreManager.nowScore >= nextBossScore)
        {
            SpawnBoss();
            nextBossScore += bossScoreInterval;
            return;
        }

        if (bossAlive)
            return;

        nowTime += Time.deltaTime;

        if (nowTime >= createTime)
        {
            SpawnMonster();
            nowTime = 0f;
            createTime = Random.Range(minTime, maxTime);
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
        {
            Debug.LogWarning("Boss Prefab is not assigned.");
            return;
        }

        bossAlive = true;

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            Destroy(monster);
        }

        Vector3 spawnPos = transform.position;
        if (bossSpawnPoint != null)
            spawnPos = bossSpawnPoint.position;

        GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        BossMonster boss = bossObj.GetComponent<BossMonster>();
        if (boss != null)
        {
            boss.manager = this;

            if (player != null)
                boss.target = player;
        }
        else
        {
            Debug.LogWarning("BossMonster component is missing on boss prefab.");
        }

        nowTime = 0f;
    }

    public void NotifyBossDead()
    {
        bossAlive = false;
        nowTime = 0f;
        createTime = Random.Range(minTime, maxTime);
    }

    public void DebugSpawnBoss()
    {
        if (bossAlive)
            return;

        SpawnBoss();
    }
}