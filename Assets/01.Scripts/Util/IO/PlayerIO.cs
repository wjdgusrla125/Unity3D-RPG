using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public float currentHP;
    public float currentMP;
    public int currentGold;
}

public sealed class PlayerIO : MonoBehaviour
{
    public static void SaveData()
    {
        FSMPlayer _player = GameManager.Instance.Player.GetComponent<FSMPlayer>();
        PlayerStats _playerStats = GameManager.Instance.Player.GetComponent<PlayerStats>();
        
        PlayerData playerData = new PlayerData();
        playerData.currentHP = _player.Health.HP;
        playerData.currentMP = _player.Mana.MP;
        playerData.currentGold = _player.Gold;
        
        string jsonData = JsonUtility.ToJson(playerData);
        
        string filePath = Application.dataPath + "/Resources/PlayerData.json";
        File.WriteAllText(filePath, jsonData);
    }

    public static void LoadData()
    {
        string filePath = Application.dataPath + "/Resources/PlayerData.json";
        
        if (!File.Exists(filePath)) return;
        
        string jsonData = File.ReadAllText(filePath);
        
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);

        FSMPlayer _player = GameManager.Instance.Player.GetComponent<FSMPlayer>();
        PlayerStats _playerStats = GameManager.Instance.Player.GetComponent<PlayerStats>();
        
        _player.Health.HP = playerData.currentHP;
        _player.Mana.MP = playerData.currentMP;
        _player.Gold = playerData.currentGold;
    }
}