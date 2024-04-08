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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.gameObject.transform.position,0.5f);
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
