using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBoss : FSMEnemy
{
    private bool DoDefence = false;

    protected override IEnumerator MS_IDLE()
    {
        do
        {
            yield return null;
            
            if(IsDead()) break;

            if (this.EHealth.currentHP <= this.EHealth.MaxHP / 4 && DoDefence == false)//보스 피가 1/4로 내려갔을 때
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_DEFENCE);
                break;
            }
            
            if (DetectPlayer()) //플레이어와 적 사이의 거리가 MS_ChaseRange보다 작은게 참일 때
            {
                _navmesh.isStopped = false;
                SetState(CH_STATE.MS_CHASE);
                break;
            }

        } while (!isNewState);
    }

    protected override IEnumerator MS_CHASE()
    {
        int AttackPattern = Random.Range(0, 3);
        
        do
        {
            _navmesh.destination = _player.transform.position;
            
            yield return null;
            
            if(IsDead()) break;

            if (this.EHealth.currentHP <= this.EHealth.MaxHP / 4 && DoDefence == false)//보스 피가 1/4로 내려갔을 때
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_DEFENCE);
                break;
            }
            
            if (CompareDistanse(this.transform.position, _player.transform.position, State.MS_attackRange))
            {
                _navmesh.isStopped = true;
                switch (AttackPattern)
                {
                    case 0:
                        SetState(CH_STATE.MS_COMBOATTACK);
                        break;
                    case 1:
                        SetState(CH_STATE.MS_SPINATTACK);
                        break;
                    case 2:
                        SetState(CH_STATE.MS_HEAVYATTACK);
                        break;
                }
                
                if (!DetectPlayer())
                {
                    _navmesh.isStopped = true;
                    SetState(CH_STATE.MS_IDLE);
                    break;
                }
            }
            
        } while (!isNewState);
    }
    
    protected virtual IEnumerator MS_COMBOATTACK()
    {
        int AttackPattern;
        do
        {
            AttackPattern = Random.Range(0, 3);
        } while (AttackPattern == 0);
        
        do
        {
            yield return new WaitForSeconds(2.0f);
            
            if(IsDead()) break;

            if (!CompareDistanse(this.transform.position, _player.transform.position, State.MS_attackRange)) //공격 사거리 범위 벗어날 때
            {
                _navmesh.isStopped = false;
                SetState(CH_STATE.MS_CHASE);
                break;
            }
            else if (this.EHealth.currentHP <= this.EHealth.MaxHP / 4 && DoDefence == false)//보스 피가 1/4로 내려갔을 때
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_DEFENCE);
                break;
            }
            else 
            {
                switch (AttackPattern)
                {
                    case 0:
                        SetState(CH_STATE.MS_COMBOATTACK);
                        break;
                    case 1:
                        SetState(CH_STATE.MS_SPINATTACK);
                        break;
                    case 2:
                        SetState(CH_STATE.MS_HEAVYATTACK);
                        break;
                }
            }

            if (!DetectPlayer())
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_IDLE);
                break;
            }
        } while (!isNewState);
    }
    
    protected virtual IEnumerator MS_SPINATTACK()
    {
        int AttackPattern;
        do
        {
            AttackPattern = Random.Range(0, 3);
        } while (AttackPattern == 1);
        do
        {
            yield return new WaitForSeconds(2.0f);
            
            if(IsDead()) break;
            
            

            if (!CompareDistanse(this.transform.position, _player.transform.position, State.MS_attackRange)) //공격 사거리 범위 벗어날 때
            {
                _navmesh.isStopped = false;
                SetState(CH_STATE.MS_CHASE);
                break;
            }
            else if (this.EHealth.currentHP <= this.EHealth.MaxHP / 4 && DoDefence == false)//보스 피가 1/4로 내려갔을 때
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_DEFENCE);
                break;
            }
            else 
            {
                switch (AttackPattern)
                {
                    case 0:
                        SetState(CH_STATE.MS_COMBOATTACK);
                        break;
                    case 1:
                        SetState(CH_STATE.MS_SPINATTACK);
                        break;
                    case 2:
                        SetState(CH_STATE.MS_HEAVYATTACK);
                        break;
                }
            }

            if (!DetectPlayer())
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_IDLE);
                break;
            }
        } while (!isNewState);
    }
    
    protected virtual IEnumerator MS_HEAVYATTACK()
    {
        int AttackPattern;
        do
        {
            AttackPattern = Random.Range(0, 3);
        } while (AttackPattern == 2);
        do
        {
            yield return new WaitForSeconds(2.0f);
            
            if(IsDead()) break;

            if (!CompareDistanse(this.transform.position, _player.transform.position, State.MS_attackRange)) //공격 사거리 범위 벗어날 때
            {
                _navmesh.isStopped = false;
                SetState(CH_STATE.MS_CHASE);
                break;
            }
            else if (this.EHealth.currentHP <= this.EHealth.MaxHP / 4 && DoDefence == false)//보스 피가 1/4로 내려갔을 때
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_DEFENCE);
                break;
            }
            else 
            {
                switch (AttackPattern)
                {
                    case 0:
                        SetState(CH_STATE.MS_COMBOATTACK);
                        break;
                    case 1:
                        SetState(CH_STATE.MS_SPINATTACK);
                        break;
                    case 2:
                        SetState(CH_STATE.MS_HEAVYATTACK);
                        break;
                }
            }

            if (!DetectPlayer())
            {
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_IDLE);
                break;
            }
        } while (!isNewState);
    }
    
    protected virtual IEnumerator MS_DEFENCE()
    {
        DoDefence = true;
        do
        {
            if(IsDead()) break;

            this.EHealth.RecoveryHP();

            if (this.EHealth.IsHalfHealth())
            {
                yield return new WaitForSeconds(3f);
                _navmesh.isStopped = true;
                SetState(CH_STATE.MS_IDLE);
                break;
            }
        } while (!isNewState);
    }
    
}
