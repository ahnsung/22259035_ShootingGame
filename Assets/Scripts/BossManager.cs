using System.Collections;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    [Header("HP")]
    public int maxHp = 30;
    private int currentHp;

    [Header("Move")]
    public float moveSpeed = 4f;
    public float moveXLimit = 8f;

    [Header("Dash")]
    public float dashCooldown = 4f;
    public float dashPrepareTime = 0.5f;
    public float dashSpeed = 18f;
    public float dashDuration = 0.8f;
    public float dashEndDelay = 0.4f;

    [Header("Fire")]
    public GameObject bossBulletPrefab;
    public Transform firePoint;
    public float bulletDelay = 0.5f;
    public float bulletSpeed = 9f;

    [Header("Etc")]
    public Transform target;
    public GameObject prefabsExplosion;
    public int scoreValue = 10;

    [HideInInspector] public MonsterManager manager;

    private float fireTimer;
    private float dashTimer;

    private bool isDashing = false;
    private bool isPatternRunning = false;
    private bool isDead = false;

    private Vector3 dashDirection = Vector3.down;

    private void Start()
    {
        currentHp = maxHp;

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }
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

        if (isDashing)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            return;
        }

        MoveNormally();

        if (!isPatternRunning)
        {
            HandleFire();

            dashTimer += Time.deltaTime;
            if (dashTimer >= dashCooldown)
            {
                StartCoroutine(DashPattern());
            }
        }
    }

    private void MoveNormally()
    {
        Vector3 pos = transform.position;

        float targetX = Mathf.Clamp(target.position.x, -moveXLimit, moveXLimit);
        pos.x = Mathf.MoveTowards(pos.x, targetX, moveSpeed * Time.deltaTime);

        transform.position = pos;
    }

    private void HandleFire()
    {
        if (bossBulletPrefab == null || firePoint == null)
            return;

        fireTimer += Time.deltaTime;

        if (fireTimer >= bulletDelay)
        {
            fireTimer = 0f;

            GameObject bullet = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);

            BossBullet bossBullet = bullet.GetComponent<BossBullet>();
            if (bossBullet != null)
            {
                Vector3 dir = (target.position - firePoint.position).normalized;
                dir.z = 0f;
                bossBullet.Init(dir, bulletSpeed, prefabsExplosion);
            }
        }
    }

    private IEnumerator DashPattern()
    {
        isPatternRunning = true;
        dashTimer = 0f;
        fireTimer = 0f;

        // 돌진 방향 미리 고정
        Vector3 dir = target.position - transform.position;
        dir.z = 0f;

        if (dir.sqrMagnitude < 0.01f)
            dir = Vector3.down;

        dashDirection = dir.normalized;

        // 살짝 텀을 준 뒤 돌진
        yield return new WaitForSeconds(dashPrepareTime);

        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashEndDelay);

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