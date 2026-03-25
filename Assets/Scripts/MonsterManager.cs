using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject prefabMonster;

    float nowTime;
    float minTime = 1f;
    float maxTime = 5f;
    public float createTime = 1f;

    private void Start()
    {
        createTime = Random.Range(minTime, maxTime);
    }

    private void Update()
    {
        nowTime = nowTime + Time.deltaTime;

        if(nowTime > createTime)
        {
            GameObject monster = Instantiate(prefabMonster);
            monster.transform.position = transform.position;

            createTime = Random.Range(minTime, maxTime);
            nowTime = 0; // 누적 시간 초기화
        }
    }
}
