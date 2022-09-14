using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Attack : CharacterSetup
{
    [Title("Attack System")] public LayerMask EnemyMask;
    public float AttackSpeed;
    public int AttackPower;
    public bool LookAt;
    [ShowIf("@!DontNeedView")] public float AttackRange;
    public bool DontNeedView;
    public bool NeedAngle;
    [ShowIf("NeedAngle")] public float AttackAngle;
    protected bool _isAttacking;
    private Transform[] TargetEnemys;
    private bool cantAttack;
    
    protected override void OnStart()
    {
        if (DontNeedView)
        {
            AttackRange = 100;
            AttackAngle = 360;
        }
    }

    protected override void OnUpdate()
    {
        SearchEnemy();
    }

    // protected virtual Vector3 TargetPos()
    // {
    //     if (TargetEnemys == null) return transform.position;
    //     if (TargetEnemys.Length == 0)
    //         return transform.position;
    //
    //     return TargetEnemys[0].position;
    // }
    protected override void OnDeath()
    {
        cantAttack = true;
    }

    public virtual void SearchEnemy()
    {
        var range = AttackRange / 2;

        if (DontNeedView) range = 1000;

        TargetEnemys = Physics.SphereCastAll(transform.position, range, transform.forward, AttackRange,
            EnemyMask).Select(x => x.transform).ToArray();

        if (TargetEnemys.Length == 0 | cantAttack) return;

        foreach (var item in TargetEnemys)
        {
            AttackFunc(item.GetComponent<Health>());
        }
    }

    protected virtual void AttackEffect(Transform target)
    {
        _character.OnAttack?.Invoke();
        if(LookAt)
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        Debug.Log(transform.name + " Attacking to " + target.name);
    }

    public virtual void AttackFunc(Health target)
    {
        if (target == null | target.Health_ <= 0) return;

        if (!_isAttacking)
        {
            if (!DontNeedView)
                if (!DistanceCheck(target.transform))
                    return;

            if (NeedAngle)
                if (!AngleCheck(target.transform))
                    return;

            _isAttacking = true;
            AttackEffect(target.transform);
            target.HealthSystem(AttackPower);
            DOVirtual.DelayedCall(AttackSpeed, () =>
            {
                DOTween.Kill("Rotate");
                _isAttacking = false;
                transform.DORotate(Vector3.zero, 0.5f).SetId("Rotate");
            });
        }
    }

    public bool DistanceCheck(Transform _target)
    {
        return Vector3.Distance(transform.position, _target.position) < AttackRange;
    }

    public bool AngleCheck(Transform _target)
    {
        return Vector3.Angle(_target.position - transform.position, transform.forward) < AttackAngle;
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (DontNeedView) return;
        //base.OnDrawGizmos();
        ExtensionMethods.DrawDisc(transform.position, AttackRange, Color.red);
        AngleGizmos();
    }

    void AngleGizmos()
    {
        if (!NeedAngle) return;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-AttackAngle, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(AttackAngle, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, leftRayDirection * AttackRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * AttackRange);
    }

#endif
}