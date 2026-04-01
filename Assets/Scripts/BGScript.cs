using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScript : MonoBehaviour
{
    public Material mBG;

    public float spd = .2f;

    private void Update()
    {
        Vector2 dir = Vector2.up;
        mBG.mainTextureOffset += dir * spd * Time.deltaTime;
    }
}
