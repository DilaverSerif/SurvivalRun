using System;
using System.Collections;
using UnityEngine;

public class AutoAttack : CharacterSetup
{
    private BasicInventory characterLevel;
    public float attackSpeed;

    public GameObject RockPoint, spearPoint;
    
    protected override void Awake()
    {
        base.Awake();
        characterLevel = GetComponent<BasicInventory>();
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
        EventManager.FirstTouch += OnFirstTouch;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.FirstTouch -= OnFirstTouch;
    }

    private void OnFirstTouch()
    {
        StartCoroutine("Attack");
    }

    private void Rock()
    {
        Enum_PoolObject.Rock.GetObject().SetPosition(transform.position)
            .AddPower(new Vector3(0, 0.5f, 3.5f) * 10);
    }

    private void Arrow()
    {
        Enum_PoolObject.Arrow.GetObject().SetPosition(transform.position).AddPower(new Vector3(0, 0, 7) * 10);
    }

    private void Spear()
    {
        Enum_PoolObject.Spear.GetObject().SetPosition(transform.position)
            .SetRotation(new Vector3(90,0,0)).AddPower(new Vector3(0, 0.25f, 5.5f) * 10);
    }

    private void Sapan()
    {
        Enum_PoolObject.Rock.GetObject().SetPosition(transform.position)
            .AddPower(new Vector3(0, 0.2f, 4.5f) * 10);
    }

    // private void (
    IEnumerator Attack()
    {
        while (Base.IsPlaying())
        {
            _character.OnAttack.Invoke();

            if (characterLevel.Level < 2)
            {
                Rock();
                yield return new WaitForSeconds(attackSpeed);
            }
            else if (characterLevel.Level < 4)
            {
                Sapan();
                yield return new WaitForSeconds(attackSpeed);
            }
            else if (characterLevel.Level < 6)
            {
                Spear();
                yield return new WaitForSeconds(attackSpeed);
            }
            else
            {
                Arrow();
                yield return new WaitForSeconds(attackSpeed);
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }
}