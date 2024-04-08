using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FSMPlayer : FSMBase
{
    //pubilc Field
    [Header("FSMPlayer")]
    public FSMEnemy _enemy;
    public IntVariable gold;
    
    //Private Field
    private PlayerHealth _health;
    private PlayerMana _mana;
    private PlayerStats Stats;

    private Command[] _skillCommands = new Command[4];
    private float[] _skillCooldowns = new float[4];
    private bool isSkill;
    

    //프로퍼티
    public PlayerHealth Health => _health;
    public PlayerMana Mana => _mana;
    public float[] SkillCooldowns => _skillCooldowns;
    
    public bool IsBaseAttack() { return (CH_STATE.BASEATTACK == CHState);}
    public bool IsDoubleAttack() { return (CH_STATE.DOUBLEATTACK == CHState);}
    public bool IsSpinAttack() { return (CH_STATE.SPINATTACK == CHState);}
    public bool IsDead() { return (CH_STATE.DEAD == CHState);}
    public int Gold { get { return gold.Value; } set { gold.SetValue(value); } }
    
    
    //Unity 콜백
    protected override void Awake()
    {
        base.Awake();
        
        _health = this.GetComponent<PlayerHealth>();
        _mana = this.GetComponent<PlayerMana>();
        Stats = this.GetComponent<PlayerStats>();
    }
    
    private void Start()
    {
        for(int i=0; i<4; i++)
        {
            _skillCommands[i] = new SkillCommand(i);
        }
    }
    
    private void Update()
    {
        if (IsDead()) return;
        if (CHState != CH_STATE.DEAD)
        {
            playerMovement();
            OnSkillAction();
            UseItem();
        }
        UpdateSkillCooldowns();
    }
    
    
    //일반 메소드
    private void playerMovement()
     {
         if (isSkill == false)
         {
             State.horizontal = Input.GetAxis("Horizontal");
             State.vertical = Input.GetAxis("Vertical");
             float gravity = -10f;
             
             Vector3 direction = new Vector3(State.horizontal, 0f, State.vertical).normalized;
             
             if (_charactercontroller.isGrounded == false)
             {
                 direction.y += gravity * Time.deltaTime * 2;
             }

             if (direction.magnitude >= 0.1f)
             {
                 direction.Normalize();
                 float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                 float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref State.playerRotationSpeed, 0.1f);
                 transform.rotation = Quaternion.Euler(0f, angle, 0f);

                 _charactercontroller.Move(direction.normalized * State.playerSpeed * Time.deltaTime);
             }
         }
     }
    
    private void playerPointMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            
            transform.LookAt(lookAtPosition);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //아이템 획득
        var item = other.GetComponent<GroundItem>();
        
        if (item)
        {
            if (Stats.inventory.AddItem(new Item(item.item), 1))
            {
                Destroy(other.gameObject);
            }
        }
    }
    
    private void OnSkillAction()
    {
        if (Stats.equipment.GetSlots[0].GetItemObject() != null)
        {
            if (Input.GetMouseButton(0) && CanUseSkill(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _skillCommands[0].Execute(gameObject);
            }

            if (Input.GetKeyDown(KeyCode.E) && CanUseSkill(1))
            {
                _skillCommands[1].Execute(gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Q) && CanUseSkill(2))
            {
                _skillCommands[2].Execute(gameObject);
            }
        }

        if (Stats.equipment.GetSlots[1].GetItemObject() != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && CanUseSkill(3))
            {
                _skillCommands[3].Execute(gameObject);
            }
        }
    }
    
    private void UpdateSkillCooldowns()
    {
        for (int i = 0; i < 4; i++)
        {
            _skillCooldowns[i] -= Time.deltaTime;
            
            if (_skillCooldowns[i] < 0)
            {
                _skillCooldowns[i] = 0;
            }
        }
    }
    
    private bool CanUseSkill(int index)
    {
        return _skillCooldowns[index] <= 0;
    }
    
    public void TakeDamage(float enemyAttack)
    {
        Health.TakeDamage(enemyAttack);
        
        PlayerIO.SaveData();
        
        if (Health.IsDeath())
        {
            SetState(CH_STATE.DEAD);
        }
    }
    
    public void UseItem()
    {
        if (Stats.ItemSlot.GetSlots != null && Stats.ItemSlot.GetSlots.Length > 0 && Stats.ItemSlot.GetSlots[0] != null)
        {
            if (Stats.ItemSlot.GetSlots[0].GetItemObject() != null && Stats.ItemSlot.GetSlots[0].GetItemObject().type == ItemType.HpPotion)
            {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    if (Stats.ItemSlot.GetSlots[0].amount > 1)
                    {
                        Stats.ItemSlot.GetSlots[0].AddAmount(-1);
                        Health.RecoveryHP(50);
                    }
                    else if (Stats.ItemSlot.GetSlots[0].amount == 1)
                    {
                        Stats.ItemSlot.GetSlots[0].RemoveItem();
                        Health.RecoveryHP(50);
                    }
                }
            }
        }

        if (Stats.ItemSlot.GetSlots != null && Stats.ItemSlot.GetSlots.Length > 1 && Stats.ItemSlot.GetSlots[1] != null)
        {
            if (Stats.ItemSlot.GetSlots[1].GetItemObject() != null && Stats.ItemSlot.GetSlots[1].GetItemObject().type == ItemType.MpPotion)
            {
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    if (Stats.ItemSlot.GetSlots[1].amount > 1)
                    {
                        Stats.ItemSlot.GetSlots[1].AddAmount(-1);
                        Mana.RecoveryMP(50);
                    }
                    else if (Stats.ItemSlot.GetSlots[1].amount == 1)
                    {
                        Stats.ItemSlot.GetSlots[1].RemoveItem();
                        Mana.RecoveryMP(50);
                    }
                }
            }
        }
    }
    
    public void OnAttack()
    {
        _enemy.TakeDamage();
    }

    private void ResetDeath()
    {
        Health.currentHP = Health.MaxHP;
        Mana.currentMP = Mana.MP;
        SetState(CH_STATE.IDLE);
    }
    
    
    //코루틴
    protected override IEnumerator IDLE()
    {
        do
        {
            isSkill = false;
            yield return null;
            if (State.horizontal != 0 || State.vertical != 0)
            {
                SetState(CH_STATE.RUN);
            }
            
        } while (!isNewState);
    }

    protected virtual IEnumerator RUN()
    {
        do
        {
            yield return null;
            
            if (State.horizontal == 0 && State.vertical == 0)
            {
                SetState(CH_STATE.IDLE);
            }
            
        } while (!isNewState);
    }
    
    protected virtual IEnumerator BASEATTACK()
    {
        playerPointMouse();
        
        do
        {
            isSkill = true;
            
            yield return null;

            float currentAnimationTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) 
            {
                if (Input.GetMouseButton(0)) 
                {
                    if (currentAnimationTime >= 0.8f)
                    {
                        _animator.Play("Attack2");
                        SoundManager.Instance.PlaySFX("BaseAttack",0.3f);
                    }
                }
                else
                {
                    SetState(CH_STATE.IDLE);
                }
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            {
                if (Input.GetMouseButton(0)) 
                {
                    if (currentAnimationTime >= 0.8f)
                    {
                        _animator.Play("Attack3");
                        SoundManager.Instance.PlaySFX("BaseAttack",0.3f);
                    }
                }
                else
                {
                    SetState(CH_STATE.IDLE);
                }
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3")
                && currentAnimationTime >= 0.7f)
            {
                SetState(CH_STATE.IDLE);
            }


        } while (!isNewState);
    }
    
    protected virtual IEnumerator DOUBLEATTACK()
    {
        playerPointMouse();
        do
        {
            isSkill = true;
            yield return null;
            
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
                && _animator.GetCurrentAnimatorStateInfo(0).IsName("DOUBLEATTACK"))
            {
                isSkill = false;
                SetState(CH_STATE.IDLE);
                _skillCooldowns[1] = 10f;
            }

        } while (!isNewState);
    }
    
    protected virtual IEnumerator SPINATTACK()
    {
        playerPointMouse();
        do
        {
            isSkill = true;
            yield return null;
            
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f
                && _animator.GetCurrentAnimatorStateInfo(0).IsName("SPINATTACK"))
            {
                isSkill = false;
                SetState(CH_STATE.IDLE);
                _skillCooldowns[2] = 15f;
            }
            
        } while (!isNewState);
    }
    
    protected virtual IEnumerator BLOCK()
    {
        playerPointMouse();
        do
        {
            isSkill = true;
            yield return null;
            
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
                && _animator.GetCurrentAnimatorStateInfo(0).IsName("BLOCK"))
            {
                isSkill = false;
                SetState(CH_STATE.IDLE);
                _skillCooldowns[3] = 8f;
            }

        } while (!isNewState);
    }

    protected virtual IEnumerator DEAD()
    {
        Health.LifeCount--;
        
        yield return new WaitForSeconds(5f);
        
        ResetDeath();
        
        if (Health.LifeCount == 0)
        {
            LoadingSceneManager.LoadingScene("02.Town");
            transform.position = Vector3.zero;
            Health.LifeCount = 3;
        }
    }
}