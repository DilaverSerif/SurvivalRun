using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Action OnHit;
    public Action OnDeath;
    public Action OnAttack;

    //Unity Functions
    public Action OnStart;
    public Action OnUpdate;
    public Action OnAwake;

    //Touch Functions
    // public Action OnFirstTouch;
    // public Action OnTouchDown;
    // public Action OnTouchUp;

    public CharacterStat _CharacterStat = CharacterStat.live;
    [HideInInspector]
    public CharacterAnimation _CharacterAnimation;
    [HideInInspector]
    public CharacterAI _CharacterAI;

    protected virtual void Awake()
    {
        OnAwake?.Invoke();
        _CharacterAnimation = GetComponent<CharacterAnimation>();
        _CharacterAI = GetComponent<CharacterAI>();
    }
    
    protected virtual void Start()
    {
        OnStart?.Invoke();
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    // private void OnEnable()
    // {
    //     OnStart += OnStartFunc;
    // }
    //
    // private void OnDisable()
    // {
    //     OnStart -= OnStartFunc;
    // }

    // protected virtual void OnStartFunc()
    // {
    //     
    // }
    
}

public enum CharacterStat
{
    live,
    death
}