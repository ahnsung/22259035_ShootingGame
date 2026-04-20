using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [Header("Monster Prefabs")]
    public GameObject prefabMonster;
    public GameObject prefabChargeMonster;

    [Header("Target")]
    public Transform player;

    [Header("Spawn Time")]
    public float minTime = 1f;
    public float maxTime = 2f;

    [Header("Spawn Chance")]
    [Range(0f, 1f)]
    public float chargeMonsterChance = 0.3f;

    private float nowTime;
    private float createTime;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        createTime = Random.Range(minTime, maxTime);
    }

    private void Update()
    {
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();

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
        GameObject selectedPrefab = null;

        float randomValue = Random.value;

        if (prefabChargeMonster != null && randomValue < chargeMonsterChance)
            selectedPrefab = prefabChargeMonster;
        else
            selectedPrefab = prefabMonster;

        if (selectedPrefab == null)
            return;

        GameObject monster = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        Monster normalMonster = monster.GetComponent<Monster>();
        if (normalMonster != null && player != null)
            normalMonster.target = player.gameObject;

        ChargeMonster chargeMonster = monster.GetComponent<ChargeMonster>();
        if (chargeMonster != null && player != null)
            chargeMonster.target = player.gameObject;
    }
}