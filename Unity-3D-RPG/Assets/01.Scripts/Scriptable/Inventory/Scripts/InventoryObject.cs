using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public InterfaceType type;

    [SerializeField] 
    private Inventory Container = new Inventory();
    public InventorySlot[] GetSlots => Container.Slots;

    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0) return false;
        
        InventorySlot slot = FindItemOnInventory(item);
        
        if (!database.ItemObjects[item.Id].stackable || slot == null)
        {
            GetEmptySlot().UpdateSlot(item, amount);
            return true;
        }
        
        slot.AddAmount(amount);
        return true;
    }
    
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public bool IsItemInInventory(ItemObject item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.data.Id)
            {
                return true;
            }
        }
        return false;
    }

    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1 == item2) return;
        
        if (item2.CanPlaceInSlot(item1.GetItemObject()) && item1.CanPlaceInSlot(item2.GetItemObject()))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }
    
    
    [ContextMenu("Save")]
    public void Save()
    {
        //예전꺼
        // IFormatter formatter = new BinaryFormatter();
        // Stream stream = new FileStream(Path.Combine(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        // formatter.Serialize(stream, Container);
        // stream.Close();
        
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }
    
    [ContextMenu("Load")]
    public void Load()
    {
        string filePath = Path.Combine(Application.persistentDataPath, savePath);
        
        if (File.Exists(filePath))
        {
            //예전꺼
            // IFormatter formatter = new BinaryFormatter();
            // Stream stream = new FileStream(Path.Combine(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            // Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            // for (int i = 0; i < GetSlots.Length; i++)
            // {
            //     GetSlots[i].UpdateSlot(GetSlots[i].item, GetSlots[i].amount);
            // }
            // stream.Close();
            
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
            }
            stream.Close();
        }
    }
    
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
}