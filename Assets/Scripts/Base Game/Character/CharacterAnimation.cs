using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterAnimation : CharacterSetup
{
    public Animator _animator;
    private Vector3 lastPosition;
    [SerializeField] public bool RandAnim;
    private int rand;
    public bool FakeWalking;
    private float speed;
    public float SpeedForAnim => speed;
    public int WeaponID;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
    }

    protected override void OnStart()
    {
        Setup();
    }

    public void SetWeaponLevel(int a)
    {
        WeaponID = a;
        _animator.SetInteger("Weapon", WeaponID);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (transform.name == "Player")
            _animator.speed = 1;
        else _animator.speed = 0.75f;
        _character.OnAttack += () => PlayAnim(CharacterAnimStat.Attack);
        _character.OnHit += () => PlayAnim(CharacterAnimStat.Hit);
        _character.OnDeath += () => PlayAnim(CharacterAnimStat.Die);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _character.OnAttack -= () => PlayAnim(CharacterAnimStat.Attack);
        _character.OnHit -= () => PlayAnim(CharacterAnimStat.Hit);
        _character.OnDeath -= () => PlayAnim(CharacterAnimStat.Die);
    }

    private void Setup()
    {
        _animator.SetBool("Die", false);
        if (RandAnim)
        {
            rand = Random.Range(0, 2);
            _animator.SetInteger("Rand", rand);
        }
    }

    protected override void OnUpdate()
    {
        if (!Base.IsPlaying()) return;
        if (transform.name == "Player")
            _animator.SetInteger("Hungry", (int)Player.Instance.Hungry);
        if (FakeWalking) speed = 1f;
        else
        {
            speed = (transform.position - lastPosition).magnitude;
            lastPosition = transform.position;
        }

        _animator.SetFloat("Speed", speed);
        _animator.SetBool("Ground", IsGrounded());
    }

    private bool IsGrounded()
    {
        //Check ground with raycast
        var ray = new Ray(transform.position, Vector3.down);
        var hit = Physics.Raycast(ray, 0.1f);
        return hit;
    }

    private bool CheckSameAnim(string animName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("animName");
    }

    public void PlayAnim(CharacterAnimStat animStat)
    {
        switch (animStat)
        {
            case CharacterAnimStat.Idle:
                //Speed With Working
                break;
            case CharacterAnimStat.Run:
                //Speed With Working
                break;
            case CharacterAnimStat.Walk:
                //Speed With Working
                break;
            case CharacterAnimStat.Jump:
                if (IsGrounded())
                    _animator.SetTrigger("Jump");
                break;
            case CharacterAnimStat.Fall:
                if (!IsGrounded())
                    _animator.SetTrigger("Fall");
                break;
            case CharacterAnimStat.Attack:
                if (!CheckSameAnim("Attack"))
                    _animator.SetTrigger("Attack");
                else if (CheckSameAnim("Hit"))
                    _animator.Play("Attack", 0, 0);
                else _animator.Play("Attack", 0, 0);
                break;
            case CharacterAnimStat.Hit:
                if (!CheckSameAnim("Hit"))
                    _animator.SetTrigger("Hit");
                else _animator.Play("Hit", 0, 0);
                break;
            case CharacterAnimStat.Die:
                _animator.SetBool("Die", true);
                break;
            default:
                Debug.Log("Not Found");
                break;
        }
    }
}