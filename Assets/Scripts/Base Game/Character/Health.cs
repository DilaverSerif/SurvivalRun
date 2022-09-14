using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Health : CharacterSetup
{
    [Title("After Die")] public bool OptionsDie;
    [ShowIf("OptionsDie")] public float DelayDie;
    public int MaxHealth;
    private int health;
    private Collider collider;
    public int Health_ => health;
    protected override void Awake()
    {
        base.Awake();
        collider = GetComponent<Collider>();
    }

    protected override void OnStart()
    {
        
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        collider.enabled = true;
        health = MaxHealth;
    }
    
    public void AddHealth(int value)
    {
        health += value;
        if (health > MaxHealth)
            health = MaxHealth;
    }
    public void HealthSystem(int damage)
    {
        Debug.Log("Bullet Hit");
        if (health <= 0) return;
        if (health > MaxHealth) health = MaxHealth;

        health -= damage;
        if (health <= 0)
        {
            collider.enabled = false;
            _character.OnDeath?.Invoke();
            Debug.Log("You are dead");
            health = 0;
            WhenDead();
            _character._CharacterStat = CharacterStat.death;
            return;
        }

        _character.OnHit?.Invoke();
    }

    public virtual void WhenDead()
    {
        if (DelayDie > 0)
        {
            DOVirtual.DelayedCall(DelayDie, () => { gameObject.SetActive(false); });
            return;
        }

        gameObject.SetActive(false);
    }
}