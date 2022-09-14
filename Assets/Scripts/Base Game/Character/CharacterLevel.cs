using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterLevel : MonoBehaviour, ILevelable
{
    public Action OnLevelUp;
    public int Exp;
    public int MaxExp;
    public int Level { get; set; }
    
    // public override void Setup()
    // {
    //     Level = DataExtension.GetExtraInt(transform.name);
    //     MaxHealth += Level * 10;
    //     AttackPower += Level * 5;
    //     base.Setup();
    //
    //     SpawnItems();
    // }

    public void LevelUp(int exp)
    {
        Exp += exp;
        if (Exp >= MaxExp)
        {
            Level++;
            MaxExp = (int)(MaxExp * 1.5f);
            Exp = 0;
            OnLevelUp?.Invoke();
        }
    }
}