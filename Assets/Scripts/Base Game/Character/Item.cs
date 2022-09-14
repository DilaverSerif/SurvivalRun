using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int ItemID;
    public string ItemName;

    [PreviewField(50, ObjectFieldAlignment.Right)]
    public Sprite ItemIcon;

    public GameObject Prefab;
    public ItemType ItemType;
    [HideIf("ItemType", ItemType.Coin)] public int ItemLevel;
    [HideIf("ItemType", ItemType.Coin)] public List<ItemData> ItemData = new List<ItemData>();
    public bool Usable;
    [ShowIf("Usable")] public UnityEvent OnUse;
}

[System.Serializable]
public class ItemData
{
    public int Value;
    public ItemStatu Statu;
}

public enum ItemStatu
{
    Armor,
    AttackSpeed,
    AttackRange,
    AttackDamage,
    AttackAngle
}