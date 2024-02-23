using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/MPpotion")]
public class MPpotionObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.MpPotion;
    }
}