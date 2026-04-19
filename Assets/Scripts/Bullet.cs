using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float spd = 5f;

    private void Update()
    {
        transform.Translate(Vector3.up * spd * Time.deltaTime);
    }
}
