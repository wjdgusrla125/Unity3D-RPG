using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public PoolManager _poolManager;

    
    private void Start()
    {
        StartCoroutine(Monstersp());
    }

    IEnumerator Monstersp()
    {
        while (true)
        {
            yield return null;

            GameObject t_object = _poolManager.GetQueue();
        }
    }
}
