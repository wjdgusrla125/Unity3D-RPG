using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ItemIO : MonoBehaviour
{
    public static void ItemDataSave()
    {
        PlayerStats _playerStats = GameManager.Instance.Player.GetComponent<PlayerStats>();
        
        _playerStats.equipment.Save();
        _playerStats.inventory.Save();
        _playerStats.ItemSlot.Save();
    }

    public static void ItemDataLoad()
    {
        PlayerStats _playerStats = GameManager.Instance.Player.GetComponent<PlayerStats>();
        
        _playerStats.equipment.Load();
        _playerStats.inventory.Load();
        _playerStats.ItemSlot.Load();
    }
}
