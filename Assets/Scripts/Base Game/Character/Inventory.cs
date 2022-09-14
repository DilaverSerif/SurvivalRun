using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public abstract class Inventory : MonoBehaviour
{
    public bool HaveHelmet;
    [ShowIf("HaveHelmet")] public Item Helmet;

    public bool HaveArmor;
    [ShowIf("HaveArmor")] public Item Armor;
    [ShowIf("HaveArmor")] public Transform SpawnPointForArmor;
    public bool HaveBoots;
    [ShowIf("HaveBoots")] public Item Boots;

    public bool HaveWeapon;
    [ShowIf("HaveWeapon")] public Item Weapon;
    [ShowIf("HaveWeapon")] public Transform SpawnPointForWeapon;

    public bool HaveShield;
    [ShowIf("HaveShield")] public Item Shield;

    public bool HaveRing;
    [ShowIf("HaveRing")] public Item Ring;

    public int MaxSize;
    public List<Item> items = new List<Item>();
    public bool Auto_Equip;
    
    private CharacterLevel _characterLevel;

    private void Awake()
    {
        _characterLevel = GetComponent<CharacterLevel>();
    }

    private void OnEnable()
    {
        if (Auto_Equip)
            _characterLevel.OnLevelUp += AutoEquip;
    }

    private void OnDisable()
    {
        if (Auto_Equip)
            _characterLevel.OnLevelUp -= AutoEquip;
    }

    public Item GetItemLocationWithType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                return Weapon;
            case ItemType.Armor:
                return Armor;
            case ItemType.Helmet:
                return Helmet;
            case ItemType.Boot:
                return Boots;
            case ItemType.Glove:
                break;
            case ItemType.Shield:
                return Shield;
            case ItemType.Ring:
                return Ring;
            case ItemType.Potion:
                break;
            case ItemType.Food:
                break;
            case ItemType.Scroll:
                break;
            case ItemType.Book:
                break;
            case ItemType.Key:
                break;
            case ItemType.None:
                break;
            default:
                return null;
        }

        return null;
    }

    protected void AutoEquip()
    {
        // foreach (var item in items)
        // {
        //     if (item.ItemType == ItemType.Weapon)
        //     {
        //         if (Weapon.ItemPower < item.ItemPower)
        //         {
        //             Weapon = item;
        //         }
        //     }
        //     else if (item.ItemType == ItemType.Armor)
        //     {
        //         if (Armor.ItemPower < item.ItemPower)
        //         {
        //             Armor = item;
        //         }
        //     }
        // }
    }

    private void SpawnItems()
    {
        if (HaveArmor)
        {
            if (Armor == null) return;
            var armor = Instantiate(Armor.Prefab, SpawnPointForArmor.position, Quaternion.identity);
            armor.transform.SetParent(SpawnPointForArmor);
        }

        if (HaveWeapon)
        {
            if (Weapon == null) return;
            var weapon = Instantiate(Weapon.Prefab, SpawnPointForWeapon.position, Quaternion.identity);
            weapon.transform.SetParent(SpawnPointForWeapon);
        }


        // foreach (var item in Inventory.items)
        // {
        //     var spawnedItem = Instantiate(item.Prefab, SpawnPoint);
        //     spawnedItem.transform.position = Vector3.zero;
        //     spawnedItem.transform.rotation = Quaternion.identity;
        //     //spawnedItem.gameObject.SetActive(false);
        // }
    }
}

public static class InventoryExtension
{
    public static bool IsFull(this Inventory inventory)
    {
        return inventory.items.Count >= inventory.MaxSize;
    }

    public static Item CheckBodyItems(this ItemType itemType, Inventory _Inventory, int Level)
    {
        foreach (var item in _Inventory.items)
        {
            if (item.ItemType == itemType)
            {
                if (_Inventory.Armor == null)
                    _Inventory.Armor = item;
                else if (_Inventory.Armor.ItemLevel < item.ItemLevel & _Inventory.Armor.ItemLevel < Level)
                    _Inventory.Armor = item;
            }
        }

        return null;
    }

    public static void DropRandomItem(this List<DropItem> list, [Optional] Vector3 pos)
    {
        list[Random.Range(0, list.Count)].DropTheItem(pos);
    }

    public static void DropRandomItemGameObject(this List<DropItem> list,Vector3 pos)
    {
        list[Random.Range(0, list.Count)].DropTheItem(pos);
    }

    public static void AddItem(this Item item, Inventory inventory)
    {
        if (inventory.IsFull())
            return;

        inventory.items.Add(item);
    }

    public static void RemoveItem(this Item item, Inventory inventory)
    {
        inventory.items.Remove(item);
    }
}