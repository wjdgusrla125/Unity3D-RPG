using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject ItemSlot;

    private Transform shield;
    private Transform sword;
    public Collider[] Colliders;

    public Transform weaponTransform;
    public Transform shieldTransform;

    public Attribute[] attributes;
    public Attribute Agility => attributes[0];
    public Attribute Defence => attributes[1];

    private void Start()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }

        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].onBeforeUpdated += OnRemoveItem;
            equipment.GetSlots[i].onAfterUpdated += OnAddItem;
        }
    }

    public void OnRemoveItem(InventorySlot slot)
    {
        if (slot.GetItemObject() == null)
            return;

        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                for (int i = 0; i < slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == slot.item.buffs[i].stat)
                            attributes[j].value.RemoveModifier(slot.item.buffs[i]);
                    }
                }

                if (slot.GetItemObject().characterDisplay != null)
                {
                    switch (slot.AllowedItems[0])
                    {
                        case ItemType.Weapon:
                            Destroy(sword.gameObject);
                            Colliders[0] = null;
                            break;
                        case ItemType.Shield:
                            Destroy(shield.gameObject);
                            Colliders[1] = null;
                            break;
                    }
                }
                break;
        }
    }

    public void OnAddItem(InventorySlot slot)
    {
        if (slot.GetItemObject() == null)
            return;

        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                
                for (int i = 0; i < slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == slot.item.buffs[i].stat)
                        {
                            attributes[j].value.AddModifier(slot.item.buffs[i]);
                        }
                    }
                }

                if (slot.GetItemObject().characterDisplay != null)
                {
                    switch (slot.AllowedItems[0])
                    {
                        case ItemType.Weapon:
                            sword = Instantiate(slot.GetItemObject().characterDisplay, weaponTransform).transform;
                            Colliders[0] = sword.gameObject.GetComponent<BoxCollider>();
                            Colliders[0].enabled = false;
                            break;
                        case ItemType.Shield:
                            shield = Instantiate(slot.GetItemObject().characterDisplay, shieldTransform).transform;
                            Colliders[1] = shield.gameObject.GetComponent<BoxCollider>();
                            Colliders[1].enabled = false;
                            break;
                    }
                }
                break;
            case InterfaceType.ItemSlot:
                break;
        }
    }
    
    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
        ItemSlot.Clear();
    }
    
    public void AttributeModified(Attribute attribute)
    {
        //Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }
}