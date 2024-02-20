/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float attackRange;
    public float chaseRange;
    public float chaseSpeed;
    
    public float maxHP;
    public float currentHP;
    public float healthLevel = 10;

    public Animator _enemyanimator;
    public NavMeshAgent navMeshAgent;
    public CapsuleCollider _capsuleCollider;
    private EnemyState currentState;
    private SOSkill skillName;
    
    //드랍 아이템
    public GameObject[] itemPrefab;

    private void Awake()
    {
        _enemyanimator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        maxHP = SetMaxHealthFromHealthLevel();
        currentHP = maxHP;
    }

    private void Start()
    {
        currentState = new IdleState();
        currentState.EnterState(this);
        currentHP = maxHP;
        navMeshAgent.autoBraking = false;
        //GotoNextPoint();
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void ChangeState(EnemyState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    
    private float SetMaxHealthFromHealthLevel()
    {
        maxHP = healthLevel * 10;
        return maxHP;
    }
    
    public void EnemyTakeDamage(float damage)
    {
        currentHP = currentHP - damage;
        
        if (currentHP <= 0)
        {
            currentHP = 0;
        }
    }

    public void DropItem()
    {
        int itemindex = Random.Range(0, itemPrefab.Length);
        var itemGo = Instantiate<GameObject>(this.itemPrefab[itemindex]);
        itemGo.transform.position = this.gameObject.transform.position;
        itemGo.SetActive(true);
        
        Destroy(gameObject, 6f);
    }
    
}*/