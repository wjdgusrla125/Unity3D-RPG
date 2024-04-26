using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMEnemy : FSMBase
{
    //Pubilc Field
    public EnemyHealth EHealth;
    public GameObject[] itemPrefab;
    public Collider[] Colliders;
    
    //Private Field
    protected bool isAlive  = false;
    
    //Protected Field
    protected GameObject _player;
    protected FSMPlayer _fsmplayer;
    protected PlayerStats _stats;
    protected NavMeshAgent _navmesh;
    
    //프로퍼티
    public bool IsDead() { return (CH_STATE.MS_DEAD == CHState); }
    
    //Unity 콜백
    protected override void Awake()
    {
        base.Awake();

        _player = GameManager.Instance.Player;
        _fsmplayer = _player.GetComponent<FSMPlayer>();
        _stats = _player.GetComponent<PlayerStats>();
        EHealth = this.GetComponent<EnemyHealth>();

        _navmesh = GetComponent<NavMeshAgent>();
        _navmesh.speed = characterState.MS_RunSpeed;

        for (int i = 0; i < Colliders.Length; ++i)
        {
            Colliders[i].enabled = false;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EHealth.HP = EHealth.MaxHP;

        SetState(CH_STATE.MS_IDLE);
    }
    
    //일반 메소드
    public void TakeDamage()
    {
        if(IsDead()) return;

        float Damage;
        
        //스킬별 데미지 작성
        if (_fsmplayer.IsBaseAttack())
        {
            Damage = _stats.Offence.value.ModifiedValue * 3;
            EHealth.Takedamage((int)Damage);
        }
        else if (_fsmplayer.IsDoubleAttack())
        {
            Damage = _stats.Offence.value.ModifiedValue * 7;
            EHealth.Takedamage((int)Damage);
        }
        else if (_fsmplayer.IsSpinAttack())
        {
            Damage = _stats.Offence.value.ModifiedValue * 15;
            EHealth.Takedamage((int)Damage);
        }
        
        if (CHState == CH_STATE.MS_DEFENCE)
        {
            Damage = 0;
            EHealth.RecoveryHP();
        }
        if (EHealth.IsDeath())
        {
            SetState(CH_STATE.MS_DEAD);
        }
    }

    public void OnMSAttack()
    {
        if (_fsmplayer.CHState == CH_STATE.BLOCK)
        {
            _fsmplayer.TakeDamage(characterState.MS_AttackDamage - _stats.Defence.value.ModifiedValue);
        }
        
        switch (CHState)
        {
            case CH_STATE.MS_ATTACK:
                _fsmplayer.TakeDamage(characterState.MS_AttackDamage);
                break;
            case CH_STATE.MS_COMBOATTACK:
                _fsmplayer.TakeDamage(characterState.MS_AttackDamage + 10);
                break;
            case CH_STATE.MS_SPINATTACK:
                _fsmplayer.TakeDamage(characterState.MS_AttackDamage + 15);
                break;
            case CH_STATE.MS_HEAVYATTACK:
                _fsmplayer.TakeDamage(characterState.MS_AttackDamage + 20);
                break;
        }
    }

    protected bool DetectPlayer()
    {
        return CompareDistanse(transform.position, _player.transform.position, characterState.MS_ChaseRange);
    }

    public bool CompareDistanse(Vector3 a, Vector3 b, float distance)
    {
        if (Vector3.Distance(a, b) <= distance)
            return true;

        return false;
    }
    
    public void DropItem()
    {
        int itemindex = Random.Range(0, itemPrefab.Length);
        var itemGo = Instantiate<GameObject>(this.itemPrefab[itemindex]);
        itemGo.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        itemGo.SetActive(true);

        Destroy(gameObject, 6f);
        
        _fsmplayer.gold.ApplyChange(100);
    }

    //코루틴
    protected virtual IEnumerator MS_IDLE()
    {
        do
        {
            yield return null;
            
            if(IsDead()) break;

            if (DetectPlayer()) //플레이어와 적 사이의 거리가 MS_ChaseRange보다 작은게 참일 때
            {
                _navmesh.isStopped = false;
                SetState(CH_STATE.MS_CHASE);
                break;
            }

        } while (!IsNewState);
    }

    protected virtual IEnumerator MS_CHASE()
    {
        do
        {
            _navmesh.destination = _player.transform.position;
            yield return null;
            
            if(IsDead()) break;
            
            if (CompareDistanse(transform.position, _player.transform.position, characterState.MS_attackRange)) // 공격 사거리 범위에 들어올 때
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_ATTACK);
                break;
            }

            if (!DetectPlayer())
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_IDLE);
                break;
            }
            
        } while (!IsNewState);
    }

    protected virtual IEnumerator MS_ATTACK()
    {
        do
        {
            yield return null;
            
            if(IsDead()) break;

            if (!CompareDistanse(transform.position, _player.transform.position, characterState.MS_attackRange)) //공격 사거리 범위 벗어날 때
            {
                _navmesh.isStopped = false;
                SetState(CH_STATE.MS_CHASE);
                break;
            }

            if (!DetectPlayer())
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_IDLE);
                break;
            }
            
        } while (!IsNewState);
    }

    protected virtual IEnumerator MS_DEAD()
    {
        if (isAlive == false)
        {
            isAlive = true;
            for (int i=0; i<Colliders.Length; ++i) Colliders[i].enabled = false;
            characterController.enabled = false;
            DropItem();
        }
        
        yield return null;
    }
}