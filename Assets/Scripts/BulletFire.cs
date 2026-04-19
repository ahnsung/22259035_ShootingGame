using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    public GameObject bulletObject;
    public GameObject bulletFireObject;
    // Start is called before the first frame update

    // Update is called once per frame

    public float delay = 3f;
    public float det = 0f;
    void Update()
    {
        det = det +  Time.deltaTime;
        if(det > delay)
        {
            GameObject bullet = Instantiate(bulletObject);
            bullet.transform.position = bulletFireObject.transform.position;
            det = 0f;
        }
    }
}