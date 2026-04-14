using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject target;

    public float spd = 5.0f;
    Vector3 direct = Vector3.down;

    public GameObject prefabsExplosion;
   
    private void Start()
    {
        int rndNum = Random.Range(0, 10);

        if(rndNum % 3 == 0)
        {

            direct = target.transform.position - transform.position;
            direct.Normalize();
        }
    }

    private void Update()
    {
        transform.position = transform.position + direct * spd * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject explisionObj = Instantiate(prefabsExplosion);
        explisionObj.transform.position = transform.position;


        Destroy(collision.gameObject);

        Destroy(gameObject);
    }
}
