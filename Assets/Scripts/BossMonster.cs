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
    public float fixedX = 0f;

    [Header("Rotation")]
    public Vector3 fixedRotation = new Vector3(0f, 0f, 0f);

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

        idlePosition = new Vector3(fixedX, fixedY, 0f);
        transform.position = idlePosition;
        FixDirectionDown();
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

        transform.position = idlePosition;
        FixDirectionDown();

        HandleFire();

        dashTimer += Time.deltaTime;
        if (dashTimer >= dashCooldown)
        {
            StartCoroutine(DashPattern());
        }
    }

    private void FixDirectionDown()
    {
        transform.rotation = Quaternion.Euler(fixedRotation);
    }

    private void HandleFire()
    {
        if (bossBulletPrefab == null)
        {
            Debug.LogWarning("Boss bullet prefab is not assigned.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint is not assigned.");
            return;
        }

        if (target == null)
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
            else
            {
                Debug.LogWarning("BossBullet component is missing on boss bullet prefab.");
            }
        }
    }

    private IEnumerator DashPattern()
    {
        isPatternRunning = true;
        dashTimer = 0f;
        bulletTimer = 0f;

        Vector3 savedPlayerPos = target.position;
        savedPlayerPos.z = 0f;

        Vector3 retreatDir = (transform.position - savedPlayerPos).normalized;
        retreatDir.z = 0f;

        if (retreatDir.sqrMagnitude < 0.01f)
            retreatDir = Vector3.up;

        Vector3 retreatTarget = transform.position + retreatDir * retreatDistance;
        retreatTarget.z = 0f;

        while (Vector3.Distance(transform.position, retreatTarget) > 0.03f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                retreatTarget,
                retreatSpeed * Time.deltaTime
            );

            FixDirectionDown();
            yield return null;
        }

        Vector3 chargeDir = (savedPlayerPos - transform.position).normalized;
        chargeDir.z = 0f;

        if (chargeDir.sqrMagnitude < 0.01f)
            chargeDir = Vector3.down;

        float elapsed = 0f;

        while (elapsed < chargeTime)
        {
            transform.position += chargeDir * chargeSpeed * Time.deltaTime;
            FixDirectionDown();

            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(postDashDelay);

        while (Vector3.Distance(transform.position, idlePosition) > 0.03f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                idlePosition,
                returnSpeed * Time.deltaTime
            );

            FixDirectionDown();
            yield return null;
        }

        transform.position = idlePosition;
        FixDirectionDown();
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