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
        for (int i = 0; i < SpawnPoint.Length; i++)
        {
            GameObject t_object = Instantiate(monster, this.gameObject.transform);
            M_queue.Enqueue(t_object);
            t_object.SetActive(false);
        }
        
        StartCoroutine(MonsterSpawn());
    }

    public void InsertQueue(GameObject p_object)
    {
        M_queue.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue()
    {
        GameObject t_object = M_queue.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }

    public IEnumerator MonsterSpawn()
    {
        while (true)
        {
            if (M_queue.Count != 0)
            {
                for (int i = 0; i < SpawnPoint.Length; i++)
                {
                    GameObject t_object = GetQueue();
                    t_object.transform.position = SpawnPoint[i].position;
                }
            }
            
            yield return new WaitForSeconds(1f);
        }
    }
}

