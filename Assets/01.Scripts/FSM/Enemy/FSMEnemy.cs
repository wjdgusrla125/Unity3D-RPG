using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMEnemy : FSMBase
{
    public EnemyHealth EHealth;
    public GameObject[] itemPrefab;
    
    protected GameObject _player;
    protected FSMPlayer _fsmplayer;
    protected PlayerStats _stats;
    protected NavMeshAgent _navmesh;

    public Collider[] Colliders;

    protected override void Awake()
    {
        base.Awake();

        _player = GameManager.Instance.Player;
        _fsmplayer = _player.GetComponent<FSMPlayer>();
        _stats = _player.GetComponent<PlayerStats>();
        EHealth = this.GetComponent<EnemyHealth>();

        _navmesh = GetComponent<NavMeshAgent>();
        _navmesh.speed = State.MS_RunSpeed;

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


    public void TakeDamage()
    {
        if(IsDead()) return;

        float Damage;
        
        //스킬별 데미지 작성
        if (_fsmplayer.IsBaseAttack())
        {
            Damage = _stats.Agility.value.ModifiedValue * 3;
            EHealth.Takedamage((int)Damage);
        }
        else if (_fsmplayer.IsDoubleAttack())
        {
            Damage = _stats.Agility.value.ModifiedValue * 7;
            EHealth.Takedamage((int)Damage);
        }
        else if (_fsmplayer.IsSpinAttack())
        {
            Damage = _stats.Agility.value.ModifiedValue * 15;
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
            _fsmplayer.TakeDamage(State.MS_AttackDamage - _stats.Defence.value.ModifiedValue);
        }
        
        switch (CHState)
        {
            case CH_STATE.MS_ATTACK:
                _fsmplayer.TakeDamage(State.MS_AttackDamage);
                break;
            case CH_STATE.MS_COMBOATTACK:
                _fsmplayer.TakeDamage(State.MS_AttackDamage + 10);
                break;
            case CH_STATE.MS_SPINATTACK:
                _fsmplayer.TakeDamage(State.MS_AttackDamage + 15);
                break;
            case CH_STATE.MS_HEAVYATTACK:
                _fsmplayer.TakeDamage(State.MS_AttackDamage + 20);
                break;
        }
    }

    protected bool DetectPlayer()
    {
        return CompareDistanse(transform.position, _player.transform.position, State.MS_ChaseRange);
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
        itemGo.transform.position = this.gameObject.transform.position;
        itemGo.SetActive(true);
        
        Destroy(gameObject, 6f);
    }
    
    private bool IsMSCombo(){return (CH_STATE.MS_COMBOATTACK == CHState);}
    private bool IsMSSpin(){return (CH_STATE.MS_SPINATTACK == CHState);}
    private bool IsMSHeavy(){return (CH_STATE.MS_HEAVYATTACK == CHState);}
    public bool IsDead() { return (CH_STATE.MS_DEAD == CHState); }

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

        } while (!isNewState);
    }

    protected virtual IEnumerator MS_CHASE()
    {
        do
        {
            _navmesh.destination = _player.transform.position;
            yield return null;
            
            if(IsDead()) break;
            
            if (CompareDistanse(transform.position, _player.transform.position, State.MS_attackRange)) // 공격 사거리 범위에 들어올 때
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
            
        } while (!isNewState);
    }

    protected virtual IEnumerator MS_ATTACK()
    {
        do
        {
            yield return null;
            
            if(IsDead()) break;

            if (!CompareDistanse(transform.position, _player.transform.position, State.MS_attackRange)) //공격 사거리 범위 벗어날 때
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
            
        } while (!isNewState);
    }

    protected virtual IEnumerator MS_DEAD()
    {
        for (int i=0; i<Colliders.Length; ++i) Colliders[i].enabled = false;
        _charactercontroller.enabled = false;
        DropItem();
        
        yield return null;
    }
}