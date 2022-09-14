using DG.Tweening;
using UnityEngine;

public class Deer : PoolItem
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            GetComponent<CharacterAI>().NearSpeed = 50f;
            DOVirtual.DelayedCall(5f,()=> gameObject.SetActive(false));
            GetComponent<CharacterAnimation>()._animator.speed = 1.5f;
        }
    }

    public override void OnDisable()
    {
        if(EnemySpawnerManager.Instance != null)
        {
            EnemySpawnerManager.Instance.RemoveEnemy(this);
        }
        base.OnDisable();
    }

    // private void OnDisable()
    // {
    //     EnemySpawnerManager.Instance.RemoveEnemy(GetComponent<PoolItem>());
    // }
}
