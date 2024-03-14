using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Monstersp());
    }

    IEnumerator Monstersp()
    {
        while (true)
        {
            yield return null;

            GameObject t_object = PoolManager.Instance.GetQueue();
        }
    }
}
