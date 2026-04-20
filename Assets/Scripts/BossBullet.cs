using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Vector3 moveDir = Vector3.down;
    private float moveSpeed = 8f;
    private GameObject explosionPrefab;

    public void Init(Vector3 dir, float speed, GameObject explosionObj)
    {
        moveDir = dir.normalized;
        moveDir.z = 0f;
        moveSpeed = speed;
        explosionPrefab = explosionObj;
    }

    private void Update()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (explosionPrefab != null)
            {
                GameObject explosionObj = Instantiate(explosionPrefab);
                explosionObj.transform.position = collision.transform.position;
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}