using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicInventory : MonoBehaviour
{
    public List<BasicItem> Items = new List<BasicItem>();
    private CharacterLevel characterLevel;
    public static BasicInventory Instance;
    public int Level;
    private void Awake()
    {
        Instance = this;
        characterLevel = GetComponent<CharacterLevel>();
    }

    private void Start()
    {
        LevelUp();
    }

    private void OnEnable()
    {
        characterLevel.OnLevelUp += LevelUp;
    }

    private void OnDisable()
    {
        characterLevel.OnLevelUp -= LevelUp;
    }

    public void LevelUp()
    {
        Level++;
        GetComponent<CharacterAnimation>().SetWeaponLevel(Level / 2);
        foreach (var item in Items)
        {
            item.CloseAllTheItems();
        }


        var closest = Items.OrderBy(item => Math.Abs(Level - item.Level));

        if (closest.First().Level > Level)
        {
            closest.ToList()[1].OpenTheItem();
        }
        else closest.First().OpenTheItem();
    }
    
}

[System.Serializable]
public class BasicItem
{
    public List<GameObject> Items = new List<GameObject>();
    public int Level;
    public void CloseAllTheItems()
    {
        foreach (var item in Items)
        {
            item.SetActive(false);
        }
    }

    public void OpenTheItem()
    {
        foreach (var item in Items)
        {
            item.SetActive(true);
        }
    }
}