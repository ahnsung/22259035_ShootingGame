using System.Collections;
using UnityEngine;

public class ChargeMonster : MonoBehaviour
{
    public float waitTime = 2.0f;
    public float retreatDistance = 1.2f;
    public float retreatSpeed = 4.0f;
    public float dashSpeed = 10.0f;
    public float dashTime = 2.0f;

    public GameObject target;
    public GameObject prefabsExplosion;

    private bool isDashing = false;
    private Vector3 dashDirection = Vector3.down;

    private void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj;
        }

        StartCoroutine(ChargePattern());
    }

    private void Update()
    {
        if (isDashing)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
        }
    }

    private IEnumerator ChargePattern()
    {
        yield return new WaitForSeconds(waitTime);

        Vector3 targetPos;

        if (target != null)
            targetPos = target.transform.position;
        else
            targetPos = transform.position + Vector3.down * 5f;

        targetPos.z = 0f;

        Vector3 retreatDir = (transform.position - targetPos).normalized;
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

            yield return null;
        }

        dashDirection = (targetPos - transform.position).normalized;
        dashDirection.z = 0f;

        if (dashDirection.sqrMagnitude < 0.01f)
            dashDirection = Vector3.down;

        isDashing = true;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
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
                explosionObj.transform.position = collision.transform.position;
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}