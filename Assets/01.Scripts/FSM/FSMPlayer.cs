using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FSMPlayer : FSMBase
{
    public PlayerHealth Health;
    public PlayerMana Mana;
    private PlayerStats Stats;
    public FSMEnemy _enemy;
    
    //스킬
    private Command[] SkillCommands = new Command[4];
    public float[] skillCooldowns = new float[4];
    private bool isSkill;
    
    
    //유니티 메소드
    protected override void Awake()
    {
        base.Awake();
        Health = this.GetComponent<PlayerHealth>();
        Mana = this.GetComponent<PlayerMana>();
        Stats = this.GetComponent<PlayerStats>();
    }
    private void Start()
    {
        for(int i=0; i<4; i++)
        {
            SkillCommands[i] = new SkillCommand(i);
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
    
             Vector3 direction = new Vector3(State.horizontal, 0f, State.vertical).normalized;
    
             if (direction.magnitude >= 0.1f)
             {
                 direction.Normalize();
                 float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                 float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref State.playerRotationSpeed, 0.1f);
                 transform.rotation = Quaternion.Euler(0f, angle, 0f);
    
                 Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    
                 //Vector3 targetPosition = transform.position + moveDirection * State.playerSpeed * Time.deltaTime;
                 _charactercontroller.Move(moveDirection.normalized * State.playerSpeed * Time.deltaTime);
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

        if (other.CompareTag("DungeonEntry"))
        {
            UIManager.Instance.Dungeon1UI.SetActive(true);
        }
    }
    
    private void OnSkillAction()
    {
        if (Stats.equipment.GetSlots[0].GetItemObject() != null)
        {
            if (Input.GetMouseButton(0) && CanUseSkill(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                SkillCommands[0].Execute(gameObject);
            }

            if (Input.GetKeyDown(KeyCode.E) && CanUseSkill(1))
            {
                SkillCommands[1].Execute(gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Q) && CanUseSkill(2))
            {
                SkillCommands[2].Execute(gameObject);
            }
        }

        if (Stats.equipment.GetSlots[1].GetItemObject() != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && CanUseSkill(3))
            {
                SkillCommands[3].Execute(gameObject);
            }
        }
    }
    
    private void UpdateSkillCooldowns()
    {
        for (int i = 0; i < 4; i++)
        {
            skillCooldowns[i] -= Time.deltaTime;
            
            if (skillCooldowns[i] < 0)
            {
                skillCooldowns[i] = 0;
            }
        }
    }
    
    private bool CanUseSkill(int index)
    {
        return skillCooldowns[index] <= 0;
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
        if (Stats.ItemSlot.GetSlots[0].GetItemObject().type == ItemType.HpPotion)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (Stats.ItemSlot.GetSlots[0].amount > 1)
                {
                    Stats.ItemSlot.GetSlots[0].AddAmount(-1);
                    Health.RecoveryHP(50);
                }
                else if(Stats.ItemSlot.GetSlots[0].amount == 1)
                {
                    Stats.ItemSlot.GetSlots[0].RemoveItem();
                    Health.RecoveryHP(50);
                }
            }
        }

        if (Stats.ItemSlot.GetSlots[1].GetItemObject().type == ItemType.MpPotion)
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                if (Stats.ItemSlot.GetSlots[1].amount > 1)
                {
                    Stats.ItemSlot.GetSlots[1].AddAmount(-1);
                    Mana.RecoveryMP(50);
                }
                else if(Stats.ItemSlot.GetSlots[1].amount == 1)
                {
                    Stats.ItemSlot.GetSlots[1].RemoveItem();
                    Mana.RecoveryMP(50);
                }
            }
        }
    }

    public void OnAttack()
    {
        _enemy.TakeDamage();
    }
    
    
    public bool IsBaseAttack() { return (CH_STATE.BASEATTACK == CHState);}
    public bool IsDoubleAttack() { return (CH_STATE.DOUBLEATTACK == CHState);}
    public bool IsSpinAttack() { return (CH_STATE.SPINATTACK == CHState);}
    public bool IsBlock() { return (CH_STATE.BLOCK == CHState);}
    public bool IsDead() { return (CH_STATE.DEAD == CHState);}

    //코루틴
    protected override IEnumerator IDLE()
    {
        do
        {
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
            
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f
                && _animator.GetCurrentAnimatorStateInfo(0).IsName("BASEATTACK"))
            {
                isSkill = false;
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
                skillCooldowns[1] = 10f;
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
                skillCooldowns[2] = 15f;
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
                skillCooldowns[3] = 8f;
            }

        } while (!isNewState);
    }

    protected virtual IEnumerator DEAD()
    {
        yield return new WaitForSeconds(5f);

        if (Health.LifeCount > 0)
        {
            Health.LifeCount--;
            UIManager.Instance.LifeNum.text = Health.LifeCount.ToString();
            Health.currentHP = Health.MaxHP;
            Mana.currentMP = Mana.MP;
            SetState(CH_STATE.IDLE);
        }
        else
        {
            UIManager.Instance.LifeNum.text = Health.LifeCount.ToString();
            Health.currentHP = Health.MaxHP;
            Mana.currentMP = Mana.MP;
            Health.LifeCount = 3;
            LoadingSceneManager.LoadingScene("Town");
            SetState(CH_STATE.IDLE);
        }
    }
}