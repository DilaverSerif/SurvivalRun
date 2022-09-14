using UnityEngine;
using UnityEngine.Events;

public class PoolItem : MonoBehaviour
{
    [HideInInspector] public Enum_PoolObject _PoolEnum;
    public UnityEvent OnDeath;
    public UnityEvent OnSpawn;

    void OnEnable()
    {
        OnSpawn?.Invoke();
        EventManager.OnBeforeLoadedLevel += Kill;
    }
    public virtual void OnDisable()
    {
        OnDeath?.Invoke();
        EventManager.OnBeforeLoadedLevel -= Kill;
        PoolManager.Instance.BackToList(this);
    }
    private void Kill()
    {
        gameObject.SetActive(false);
    }

    #region Defualts

    public void SetEnum(Enum_PoolObject en)
    {
        _PoolEnum = en;
    }

    public PoolItem SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }

    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public PoolItem SetRotation(Vector3 rot)
    {
        transform.eulerAngles = rot;
        return this;
    }

    public void SetLocalRotation(Vector3 rot)
    {
        transform.localEulerAngles = rot;
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void SetLayer(int layer)
    {
        gameObject.layer = layer;
    }

    public void SetTag(string tag)
    {
        gameObject.tag = tag;
    }

    public void SetName(string name)
    {
        gameObject.name = name;
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }

    public void AddPower(Vector3 pow)
    {
        GetComponent<Rigidbody>().velocity = pow;
    }

    public float distance;
    private void Update()
    {
        if(distance == 0) return;
        var checkFront = Player.Instance.transform.position.z - transform.position.z;

        if (checkFront > distance)
        {
            gameObject.SetActive(false);
        }
    }
    
    #endregion
}