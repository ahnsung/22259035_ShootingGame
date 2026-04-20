using System.Collections;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public MonsterManager manager;
    public GameObject prefabsExplosion;

    [Header("HP / Score")]
    public int maxHp = 30;
    public int scoreValue = 10;
    private int currentHp;

    [Header("Fixed Position")]
    public float fixedY = 4.2f;
    public float minX = -7f;
    public float maxX = 7f;

    [Header("Bullet")]
    public GameObject bossBulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;
    public float bulletDelay = 0.75f;
    private float bulletTimer;

    [Header("Dash Pattern")]
    public float dashCooldown = 4f;
    public float retreatDistance = 1.4f;
    public float retreatSpeed = 6f;
    public float chargeSpeed = 17f;
    public float chargeTime = 0.9f;
    public float postDashDelay = 0.5f;
    public float returnSpeed = 8f;

    private float dashTimer;
    private bool isDead = false;
    private bool isPatternRunning = false;

    private Vector3 idlePosition;

    private void Start()
    {
        currentHp = maxHp;

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }

        Vector3 pos = transform.position;
        pos.y = fixedY;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = 0f;

        transform.position = pos;
        idlePosition = pos;
    }

    private void Update()
    {
        if (isDead)
            return;

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
            else
                return;
        }

        if (isPatternRunning)
            return;

        // 평소에는 제자리 고정
        transform.position = idlePosition;

        HandleFire();

        dashTimer += Time.deltaTime;
        if (dashTimer >= dashCooldown)
        {
            StartCoroutine(DashPattern());
        }
    }

    private void HandleFire()
    {
        if (bossBulletPrefab == null || firePoint == null || target == null)
            return;

        bulletTimer += Time.deltaTime;

        if (bulletTimer >= bulletDelay)
        {
            bulletTimer = 0f;

            GameObject bulletObj = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);
            BossBullet bullet = bulletObj.GetComponent<BossBullet>();

            if (bullet != null)
            {
                Vector3 dir = (target.position - firePoint.position).normalized;
                dir.z = 0f;
                bullet.Init(dir, bulletSpeed, prefabsExplosion);
            }
        }
    }

    private IEnumerator DashPattern()
    {
        isPatternRunning = true;
        dashTimer = 0f;
        bulletTimer = 0f;

        // 돌진 직전 플레이어 위치를 저장
        Vector3 savedPlayerPos = target.position;
        savedPlayerPos.z = 0f;

        // 1. 뒤로 빠지는 예고 모션
        Vector3 retreatDir = (transform.position - savedPlayerPos).normalized;
        retreatDir.z = 0f;

        if (retreatDir.sqrMagnitude < 0.01f)
            retreatDir = Vector3.up;

        Vector3 retreatTarget = transform.position + retreatDir * retreatDistance;
        retreatTarget.z = 0f;
        retreatTarget.x = Mathf.Clamp(retreatTarget.x, minX - 1f, maxX + 1f);
        retreatTarget.y = Mathf.Clamp(retreatTarget.y, fixedY, fixedY + 1.8f);

        while (Vector3.Distance(transform.position, retreatTarget) > 0.03f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                retreatTarget,
                retreatSpeed * Time.deltaTime
            );
            yield return null;
        }

        // 2. 저장한 플레이어 위치를 향해 직선 돌진
        Vector3 chargeDir = (savedPlayerPos - transform.position).normalized;
        chargeDir.z = 0f;

        if (chargeDir.sqrMagnitude < 0.01f)
            chargeDir = Vector3.down;

        float elapsed = 0f;

        while (elapsed < chargeTime)
        {
            transform.position += chargeDir * chargeSpeed * Time.deltaTime;

            Vector3 pos = transform.position;
            pos.z = 0f;
            pos.x = Mathf.Clamp(pos.x, minX - 2f, maxX + 2f);
            pos.y = Mathf.Clamp(pos.y, -5f, fixedY + 2f);
            transform.position = pos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3. 후딜
        yield return new WaitForSeconds(postDashDelay);

        // 4. 원래 고정 자리로 복귀
        while (Vector3.Distance(transform.position, idlePosition) > 0.03f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                idlePosition,
                returnSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = idlePosition;
        isPatternRunning = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isDead)
            return;

        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);

            currentHp--;

            if (currentHp <= 0)
            {
                Die();
            }
        }
        else if (collision.CompareTag("Player"))
        {
            if (prefabsExplosion != null)
            {
                GameObject explosionObj = Instantiate(prefabsExplosion);
                explosionObj.transform.position = collision.transform.position;
            }

            Destroy(collision.gameObject);
        }
    }

    private void Die()
    {
        isDead = true;

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
            scoreManager.AddScore(scoreValue);

        if (prefabsExplosion != null)
        {
            GameObject explosionObj = Instantiate(prefabsExplosion);
            explosionObj.transform.position = transform.position;
        }

        if (manager != null)
            manager.NotifyBossDead();

        Destroy(gameObject);
    }
}