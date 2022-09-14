using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gold : MonoBehaviour
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<Player>();
        if (obj)
        {
            _collider.enabled = false;
            transform.SetParent(other.transform);
            transform.DOLocalJump(Vector3.zero, 1.5f, 1, 0.5f).OnComplete(
                () =>
                {
                    Datas.Coin.CoinAdd(1);
                    transform.SetParent(PoolManager.Instance.holdPool);
                    gameObject.SetActive(false);
                }
            );
        }
    }
    
    // private int _index;
    // IEnumerator Yaz()
    // {
    //     yield return new WaitForSeconds(.5f);
    //     ParticleExtension.PlayCoinEffect(transform.position, _index);
    //     _index = 0;
    // }
    private void OnEnable()
    {
        _collider.enabled = true;
    }

    // IEnumerator GoMove(Transform target)
    // {
    //     while (Vector3.Distance(transform.position,target.position) > 1.5f)
    //     {
    //         var pos = target.position;
    //         pos.y += 2;
    //         transform.position = Vector3.MoveTowards(transform.position,pos, 
    //             Time.deltaTime * 30f);
    //         yield return new WaitForEndOfFrame();
    //     }
    //
    //     gameObject.SetActive(false);
    // }
}