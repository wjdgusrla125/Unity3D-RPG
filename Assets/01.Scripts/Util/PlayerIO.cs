using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public sealed class PlayerIO : MonoBehaviour
{
    public static void SaveData()
    {
        FSMPlayer _player = GameManager.Instance.Player.GetComponent<FSMPlayer>();
        PlayerStats _playerStats = GameManager.Instance.Player.GetComponent<PlayerStats>();

        XmlDocument XmlDoc = new XmlDocument();
        XmlElement XmlEle = XmlDoc.CreateElement("PlayerDB");
        XmlDoc.AppendChild(XmlEle);
        
        XmlElement ElementSetting = XmlDoc.CreateElement("Player");

        int HP = (int)_player.Health.HP;
        int MP = (int)_player.Mana.MP;

        ElementSetting.SetAttribute("CurrentHP", HP.ToString());
        ElementSetting.SetAttribute("CurrentMP", MP.ToString());

        XmlEle.AppendChild(ElementSetting);
        
        XmlDoc.Save(Application.dataPath + "/Resources/PlayerDate.xml");
        
        _playerStats.equipment.Save();
        _playerStats.inventory.Save();
        _playerStats.ItemSlot.Save();
    }

    public static void LoadData()
    {
        if(!System.IO.File.Exists(Application.dataPath + "/Resources/PlayerData.xml")) return;

        XmlDocument XmlDoc = new XmlDocument();

        XmlDoc.Load(Application.dataPath + "/Resources/PlayerData.xml");
        XmlElement XmlEle = XmlDoc["PlayerDB"];

        FSMPlayer _player = GameManager.Instance.Player.GetComponent<FSMPlayer>();
        PlayerStats _playerStats = GameManager.Instance.Player.GetComponent<PlayerStats>();

        foreach (XmlElement itemElement in XmlEle.ChildNodes)
        {
            _player.Health.HP = System.Convert.ToSingle(itemElement.GetAttribute("CurrentHP"));
            _player.Mana.MP = System.Convert.ToSingle(itemElement.GetAttribute("CurrentMP"));
        }
        
        _playerStats.equipment.Load();
        _playerStats.inventory.Load();
        _playerStats.ItemSlot.Load();
    }
}
