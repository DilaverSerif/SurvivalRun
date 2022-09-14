using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    public Item Item;
    [Range(0, 100)] public int Amount;
    public bool IsRandom;

    public void DropTheItem([Optional] Vector3 pos)
    {
        var amount = Amount;
        if (IsRandom) amount = Random.Range(Amount/2, Amount);

        if (Item.ItemType == ItemType.Coin)
        {
            Amount += EnemySpawnerManager.Instance.ExtraGold;
        }
        
        // if (Item.ItemType == ItemType.Coin)
        // {
        //     Debug.Log("Gold Coming");
        //     ParticleExtension.PlayCoinEffect(pos, amount);
        //     Datas.Coin.CoinAdd(amount);
        //     return;
        // }

        for (int i = 0; i < amount; i++)
        {
            var posRandom = pos + new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
            var spawn = GameObject.Instantiate(Item.Prefab, pos, Quaternion.identity);
            spawn.transform.SetParent(Base.GetLevelHolder());
            spawn.transform.DOJump(posRandom, 2.5f, 3, 0.5f);
            spawn.transform.DORotate(new Vector3(0, Random.Range(0, 180), 0), 0.5f);
        }
    }
}