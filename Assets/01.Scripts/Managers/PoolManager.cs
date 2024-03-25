using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject monster;
    public Queue<GameObject> M_queue = new Queue<GameObject>();
    public Transform[] SpawnPoint;
    
    private void Start()
    {
        // 초기에 몬스터를 큐에 추가
        for (int i = 0; i < SpawnPoint.Length; i++)
        {
            GameObject obj = Instantiate(monster, SpawnPoint[i].position, Quaternion.identity, transform);
            obj.SetActive(false);
            M_queue.Enqueue(obj);
        }

        StartCoroutine(MonsterSpawn());
    }

    public GameObject GetQueue()
    {
        if (M_queue.Count > 0)
        {
            GameObject obj = M_queue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator MonsterSpawn()
    {
        while (true)
        {
            for (int i = 0; i < SpawnPoint.Length; i++)
            {
                GameObject obj = GetQueue();

                if (obj == null)
                {
                    break;
                }

                obj.transform.position = SpawnPoint[i].position;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}

