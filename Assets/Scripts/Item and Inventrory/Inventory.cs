using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class Inventory : MonoBehaviour , InterfaceSavemanager
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory_UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statsSlotParent;

    [Header("Items CoolDowm")]
    private float lastTimeUseFlask;
    private float lastTimeUseArmor;

    public float flaskCooldown {  get; private set; }
    private float armorCooldown;

    private UI_ItemSlot[] inventoryItmeSlot;
    private UI_ItemSlot[] stashItmeSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Data base")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemDataEquipment> loadedEquipment;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItmeSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItmeSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statsSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        foreach(ItemDataEquipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for(int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }

        for (int i = 0; i < startingItems.Count; i++)
        {
            if(startingItems[i] != null )
                AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment OldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                OldEquipment = item.Key;
            }
        }

        if (OldEquipment != null)
        {
            UnequipItem(OldEquipment);
            AddItem(OldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifier();
        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemDataEquipment ItemToRemove)
    {
        if (equipmentDictionary.TryGetValue(ItemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(ItemToRemove);
            ItemToRemove.RemoveModifier();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItmeSlot.Length; i++)
        {
            inventoryItmeSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItmeSlot.Length; i++)
        {
            stashItmeSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItmeSlot[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItmeSlot[i].UpdateSlot(stash[i]);
        }
        for(int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValue();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashvalue))
        {
            if (stashvalue.stackSize <= 1)
            {
                stash.Remove(stashvalue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashvalue.RemoveStack();
            }
        }


        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if(inventory.Count >= inventoryItmeSlot.Length)
        {
            return false;
        }
        return true;
    }


    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            ItemData newItem = inventoryItems[inventoryItems.Count - 1].data;

            RemoveItem(newItem);
        }
    } 这一段是调试，看能不能删除物品*/

    public bool canCraft(ItemDataEquipment _itemToCraft, List<InventoryItem> _requireMaterials)
    {
        List<InventoryItem> MaterialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requireMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requireMaterials[i].data, out InventoryItem statsValue))
            {
                if (statsValue.stackSize < _requireMaterials[i].stackSize)
                {
                    Debug.Log("Material not enough");
                    return false;
                }
                else
                {
                    MaterialsToRemove.Add(statsValue);
                }
            }
            else
            {
                Debug.Log("Material not enough");
                return false;
            }
        }

        for(int i = 0; i < MaterialsToRemove.Count; i++)
        {
            RemoveItem(MaterialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("Craft success" + _itemToCraft.itemName);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemDataEquipment GetEquipment(EquipmentType _Type)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _Type)
            {
                equipedItem = item.Key;
            }
        }
        return equipedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canuUseFlask = Time.time > lastTimeUseFlask + flaskCooldown;

        if (canuUseFlask)
        {
            flaskCooldown = currentFlask.itemCoolDown;
            currentFlask.ExecuteItemEffect(null);
            lastTimeUseFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask on cooldown");
        }

    }

    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor);

        if(Time.time > lastTimeUseFlask + armorCooldown)
        {
            armorCooldown = currentArmor.itemCoolDown;
            lastTimeUseFlask = Time.time;
            return true;
        }
        Debug.Log("Armor on cooldown");
        return false;
    }

    public void LoadData(Gamedata _data)
    {
        foreach(KeyValuePair<string,int> pair in _data.inventory)
        {
            foreach(var item  in itemDataBase)
            {
                if(item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }
        foreach(string loadedItemID in _data.equipmentID)
        {
            foreach(var item in itemDataBase)
            {
                if(item != null && item.itemID == loadedItemID)
                {
                    loadedEquipment.Add(item as ItemDataEquipment);
                }
            }
        }
    }

    public void SaveData(ref Gamedata _data)
    {
        _data.equipmentID.Clear();
        _data.inventory.Clear();

        foreach(KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }
        foreach(KeyValuePair<ItemData,InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemDataEquipment,InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentID.Add(pair.Key.itemID);
        }

    }
#if UNITY_EDITOR

    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());


    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDatasbase = new List<ItemData>();
        string[] assetName = AssetDatabase.FindAssets("", new[] { "Assets/datas/Items" });

        foreach(string SONname in assetName)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SONname);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDatasbase.Add(itemData);
        }
        return itemDatasbase;
    }
#endif
}
