/*
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 5f;
    private Rigidbody _rigidbody;
    public Transform CamPivot;
    public Transform Cam;
    
    //능력치
    public Attribute[] attributes;
    public Transform SavePoints;
    public bool isDead;

    //인벤토리
    public InventoryObject inventory;
    public InventoryObject equipment;
    
    public Transform shield;
    public Transform sword;

    public Transform weaponTransform;
    public Transform shieldTransform;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        isDead = false;
    }

    private void Start()
    {
        /*for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }#1#
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            equipment.Save();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            equipment.Load();
        }
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }
    
    private void PlayerMovement()
    {
        if (InputManager.Instance.IsInput == true && isDead == false)
        {
            _rigidbody.transform.rotation = Quaternion.Euler(InputManager.Instance.MoveRotation);
            Vector3 camPivotAngle = CamPivot.eulerAngles;

            Vector3 moveHorizontal = transform.right * -InputManager.Instance.MoveInput.x;
            Vector3 moveVertical = transform.forward * InputManager.Instance.MoveInput.y;

            Vector3 velocity = (moveHorizontal + moveVertical).normalized * playerSpeed;
        
            _rigidbody.transform.Translate(velocity * Time.deltaTime);
        }
        AnimationManager.Instance.PlayerMoveAnimation();
    }

    public RaycastHit ExecuteRaycast()
    {
        Vector3 direction = transform.position - Cam.position;
        Ray ray = new Ray(Cam.position, direction);
        Debug.DrawRay(Cam.position,direction,Color.red);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        //아이템 획득
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);
            
            if (inventory.AddItem(new Item(item.item), 1))
            {
                Destroy(other.gameObject);
            }
        }
        
        //세이브포인트 저장
        if (other.gameObject.CompareTag("Savepoint"))
        {
            SavePoints = other.transform;
        }
    }
    
    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }
    
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }
    
    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                            break;
                        }
                    }
                }
                
                if (_slot.ItemObject.characterDisplay != null)
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Weapon:
                            if (sword != null)
                            {
                                Destroy(sword.gameObject);
                            }
                            break;
                        case ItemType.Shield:
                            if (shield != null)
                            {
                                Destroy(shield.gameObject);
                            }
                            break;
                    }
                }
                break;
            default:
                break;
        }
    }
    
    public void OnAddItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        
        
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                                attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                if (_slot.ItemObject.characterDisplay != null)
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Weapon:
                            sword = Instantiate(_slot.ItemObject.characterDisplay, weaponTransform).transform;
                            break;
                        case ItemType.Shield:
                            shield = Instantiate(_slot.ItemObject.characterDisplay, shieldTransform).transform;
                            break;
                    }
                }
                
                break;
        }
    }
    
}
//능력치
/*
[System.Serializable]
 public class Attribute
 {
     [System.NonSerialized] 
     public PlayerController parent;

     
     public Attributes type;
     public ModifiableInt value;
     
     public void SetParent(PlayerController _parent)
     {
         parent = _parent;
         value = new ModifiableInt(AttributeModified);
     }
     public void AttributeModified()
     {
         parent.AttributeModified(this);
     }
 }
 #1#
 */
