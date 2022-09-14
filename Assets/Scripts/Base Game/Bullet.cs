using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public ParticleSystem hitParticle;

    public int Damage;
    public bool Gravity;
    public bool ContactAfterDestory;
    [ShowIf("ContactAfterDestory")] public float ContactAfterDelay;
    public LayerMask ContactMask;
    protected Rigidbody rb;
    protected SphereCollider _collider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = Gravity;
        _collider = GetComponent<SphereCollider>();
    }

    public virtual void OnEnable()
    {
        StartCoroutine("DelayDestory");
        _collider.enabled = true;
        _collider.isTrigger = true;
    }

    private void OnDisable()
    {
        StopCoroutine("DelayDestory");
    }

    public void Spawn(Vector3 power, Vector3 rot)
    {
        transform.eulerAngles = rot;
        rb.velocity = power;
    }

    protected virtual void Contact(Transform contactTransform)
    {
    }

    private IEnumerator DelayDestory()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CheckLayer(ContactMask))
        {
            var pos = transform.position;
            pos.z -= 1.5f;
            Enum_PoolParticle.Hit.GetParticle().SetPosition(pos);
            if (other.TryGetComponent<Health>(out var health))
            {
                health.HealthSystem(Damage);
            }

            if (ContactAfterDestory)
            {
                DOVirtual.DelayedCall(ContactAfterDelay, () => { gameObject.SetActive(false); });
            }

            Contact(other.transform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground"))
            OnContact();
    }

    protected virtual void OnContact()
    {
        hitParticle.Play();
        rb.velocity = Vector3.zero;
    }
}