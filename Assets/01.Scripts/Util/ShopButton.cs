using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public IntVariable Gold;
    public PlayerStats Stats;

    private void Awake()
    {
        Stats = GameManager.Instance.Player.GetComponent<PlayerStats>();
    }

    public void Buy(ItemObject item)
    {
        if (Gold.Value > 0)
        {
            Stats.inventory.AddItem(new Item(item), 1);
            Gold.ApplyChange(-100);
        }
    }
}
