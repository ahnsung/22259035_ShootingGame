using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float spd = 5;

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");

        float v = Input.GetAxis("Vertical");

        Vector3 direct = new Vector3(h, v, 0);

        transform.position = transform.position + direct * spd  * Time.deltaTime;
    }
}
