using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/HPpotion")]
public class HPpotionObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.HpPotion;
    }
}
