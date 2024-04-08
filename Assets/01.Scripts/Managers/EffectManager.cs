using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public GameObject[] particleEffects;
    public Queue<GameObject> ParticleQueue = new Queue<GameObject>();
    public Transform[] SpawnPoint;
    
    private void Start()
    {
        // 초기에 몬스터를 큐에 추가
        for (int i = 0; i < SpawnPoint.Length; i++)
        {
            GameObject[] obj = Instantiate(particleEffects, SpawnPoint[i].position, Quaternion.identity, transform);
            obj.SetActive(false);
            ParticleQueue.Enqueue(obj);
        }

        //StartCoroutine(MonsterSpawn());
    }
}
