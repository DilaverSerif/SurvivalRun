using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class CharacterSetup : MonoBehaviour
{
    protected Character _character;

    protected virtual void Awake()
    {
        _character = GetComponent<Character>();
    }
    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected virtual void OnEnable()
    {
        _character._CharacterStat = CharacterStat.live;
        _character.OnStart += OnStart;
        _character.OnUpdate += OnUpdate;
        _character.OnDeath += OnDeath;
    }
    protected virtual void OnDisable()
    {
        _character.OnStart -= OnStart;
        _character.OnUpdate -= OnUpdate;
        _character.OnDeath -= OnDeath;
    }

    protected virtual void OnDeath()
    {
        Debug.Log("Death");
    }
}