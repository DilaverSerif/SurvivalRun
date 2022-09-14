using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : CharacterSetup
{
    [Title("Stats")] public bool CanMove;

    [OnValueChanged("SetNavMesh")] [ShowIf("CanMove")]
    public bool WithNavMesh;

    [ShowIf("CanMove")] public Vector3 MoveOffset;
    [ShowIf("@CanMove && !DontNeedView")] public float FarDistance;
    [ShowIf("@CanMove && !DontNeedView")] public float NearDistance;
    [ShowIf("@CanMove && !DontNeedView")] public float FarSpeed;
    [ShowIf("CanMove")] public float NearSpeed;
    [ShowIf("CanMove")] public bool DontNeedView;
    [ShowIf("CanMove")] public Transform[] Targets;
    [ShowIf("CanMove")] public bool FindTarget;
    [ShowIf("FindTarget")] public LayerMask TargetLayer;


//
    protected NavMeshAgent agent;
    private float _moveSpeed;

    protected override void Awake()
    {
        base.Awake();
        if (WithNavMesh)
            agent = GetComponent<NavMeshAgent>();
    }

    protected override void OnStart()
    {
        if (DontNeedView)
            FarDistance = 999f;
    }

    protected override void OnUpdate()
    {
        if (!Base.IsPlaying()) return;
        if (FindTarget)
            FindTargets();
        if (CanMove)
            Move();
    }

    protected virtual void FindTargets()
    {
        Targets = Physics.SphereCastAll(transform.position, FarDistance, transform.forward, FarDistance,
            TargetLayer).Select(x => x.transform).ToArray();
    }

    public virtual void Move()
    {
        if (_character._CharacterStat != CharacterStat.live) return;

        var goTo = new Vector3(TargetPos().x,MoveOffset.y,TargetPos().z);
        SpeedChanger();
        if (!WithNavMesh)
        {
            transform.LookAt(new Vector3(TargetPos().x, transform.position.y, TargetPos().z));
            transform.position = Vector3.MoveTowards(
                transform.position, goTo,
                Time.deltaTime * _moveSpeed);
        }
        else
        {
            agent.speed = _moveSpeed;
            agent.destination = TargetPos();
        }
    }

    protected override void OnDeath()
    {
        if (!WithNavMesh)
        {
        }
        else
        {
            agent.destination = transform.position;
            agent.isStopped = true;
        }
    }


    private void SetNavMesh()
    {
        if (WithNavMesh)
        {
            gameObject.AddComponent<NavMeshAgent>();
        }
        else
        {
            DestroyImmediate(gameObject.GetComponent<NavMeshAgent>());
        }
    }

    protected bool SpeedChanger()
    {
        if (DontNeedView)
        {
            if (DistanceWithPlayer() > FarDistance)
            {
                _moveSpeed = FarSpeed;
                return true;
            }
            else if (DistanceWithPlayer() > NearDistance)
            {
                _moveSpeed = NearSpeed;
                return true;
            }
            else return false;
        }

        if (DistanceWithPlayer() > FarDistance)
        {
            _moveSpeed = 0;
            return true;
        }
        else if (DistanceWithPlayer() < FarDistance & DistanceWithPlayer() > NearDistance)
        {
            _moveSpeed = FarSpeed;
            return true;
        }
        else if (DistanceWithPlayer() < NearDistance)
        {
            _moveSpeed = NearSpeed;
            return true;
        }
        else return false;
    }

    private float DistanceWithPlayer()
    {
        return Vector3.Distance(transform.position, TargetPos());
    }

    protected virtual Vector3 TargetPos()
    {
        if (Targets.Length != 0)
            return Targets[0].position;
        return transform.position;
    }


#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        ExtensionMethods.DrawDisc(transform.position, FarDistance, Color.white);
    }
#endif
}