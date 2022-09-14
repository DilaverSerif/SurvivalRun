using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawnerManager : Singleton<EnemySpawnerManager>
{
    [Title("Generally")] [SerializeField] private float spawnTime;
    [SerializeField] private float maxCordiX;
    public float MaxCordiX => maxCordiX;
    public int MaxEnemy;
    public bool HaveStartEnemy;
    [ShowIf("HaveStartEnemy")] public int StartEnemyCount;
    public List<Transform> enemys = new List<Transform>();
    public bool TimeByPower;
    [ShowIf("TimeByPower")] public int MultiplierTimeByPower;
    [ShowIf("TimeByPower")] public int PowerByTime;
    [Title("Enemys")] [SerializeField] private List<EnemySpawn> enemySpawns = new List<EnemySpawn>();
    public int ExtraGold;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);

        while (!GameBase.Instance.Player)
        {
            yield return new WaitForEndOfFrame();
        }
        
        if (enemys.Count > 0)
        {
            foreach (var enemy in enemys)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        
        var checkSpawn = 0;

        foreach (var spawn in enemySpawns)
        {
            checkSpawn += spawn.MaxSpawn;
        }

        if (checkSpawn < MaxEnemy)
            MaxEnemy = checkSpawn;


        if (MaxEnemy < StartEnemyCount)
        {
            MaxEnemy += StartEnemyCount;
        }

        while (enemys.Count < StartEnemyCount)
        {
            SpawnEnemy();
        }
    }

    private IEnumerator SpawnManager()
    {
        
        while (Base.IsPlaying())
        {
            if (SpawnEnemy())
                yield return new WaitForSeconds(spawnTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }
    private int a = 0;
    private bool SpawnEnemy()
    {
        var enemy = enemySpawns[Random.Range(0, enemySpawns.Count)];

        if (Base.GetTimer() % MultiplierTimeByPower == 0 & Base.GetTimer() != 0)
        {
            foreach (var enm in enemySpawns)
            {
                enm.AddPower(PowerByTime, PowerByTime);
            }

            spawnTime -= 0.1f;
            if (spawnTime < 1f)
            {
                spawnTime = 1;
            }

            ExtraGold += 2;
        }

        if (enemy.IsSpawned() & enemys.Count < MaxEnemy)
        {
            if (enemy.IsLucky())
            {
                for (int i = 0; i < (int)enemy.MaxSameSpawn.RandomValueVector2(); i++)
                {
                    enemys.Add(enemy.Spawn());
                    if (!enemy.IsSpawned()) break;
                }

                return true;
            }
        }

        return false;
    }

    void OnEnable()
    {
        EventManager.OnBeforeLoadedLevel += ResetEnemys;
        //EventManager.OnAfterLoadedLevel += FirstSpawn;
        EventManager.FirstTouch += WhenStartSpawn;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.OnBeforeLoadedLevel -= ResetEnemys;
        //EventManager.OnAfterLoadedLevel -= FirstSpawn;
        EventManager.FirstTouch -= WhenStartSpawn;
    }

    private void WhenStartSpawn()
    {
        if (enemySpawns.Count == 0) return;
        StartCoroutine("SpawnManager");
    }

    public void RemoveEnemy(PoolItem _enemy)
    {
        enemys.Remove(_enemy.transform);

        foreach (var item in enemySpawns)
        {
            if (item.EnemyObject == _enemy._PoolEnum)
            {
                item.DownSpawn();
                break;
            }
        }
    }

    private void ResetEnemys()
    {
        foreach (var item in enemySpawns)
        {
            item.Reset();
        }
    }
}

public static class ExtensionEnemySpawner
{
    public static Vector3 CheckDistance(float z)
    {
        var pos = Vector3.zero;
        var _player = GameBase.Instance.Player.transform;
        pos.y = 0;

        if (_player.position.z - z > 0)
        {
            pos.z = _player.position.z - z;
        }

        var maxCordiX = EnemySpawnerManager.Instance.MaxCordiX;
        pos.x = Random.Range(-maxCordiX, maxCordiX);

        return pos;
    }
}

[System.Serializable]
public class EnemySpawn
{
    public Enum_PoolObject EnemyObject;
    [MinMaxSlider(1, 100, true)] public Vector2 MaxSameSpawn; //Ayni anda kaç adet spawn edecek
    public bool RandomSameSpawn;
    public int MaxSpawn; //Max Spawn edilen sayı

    [MinMaxSlider(-500, 500, true)] public Vector2 FarByPlayer; //Player ile enemy arasındaki mesafe
    private int spawnCount; //Spawn edilen sayı
    [PropertyRange(0, 10)] public int Lucky;
    public bool TimeDelay;
    [ShowIf("TimeDelay")] public int Time;
    [ShowIf("TimeDelay")] public bool OpenByTime;

    public bool IsSpawned()
    {
        return spawnCount < MaxSpawn;
    }

    public void DownSpawn()
    {
        spawnCount--;
    }

    public bool IsLucky()
    {
        if (OpenByTime & Base.GetTimer() < Time)
            return false;
        return Lucky > Random.Range(0, 10);
    }

    public Transform Spawn()
    {
        var spawedEnemy = EnemyObject.GetObject();
        spawedEnemy.SetPosition(ExtensionEnemySpawner.CheckDistance(ExtensionMethods.RandomValueVector2(FarByPlayer)));
        spawnCount++;
        return spawedEnemy.transform;
    }

    public void Reset()
    {
        spawnCount = 0;
    }
    
    public void AddPower(int _MaxSpawm, int _MaxSameSpawn)
    {
        MaxSpawn += _MaxSpawm;
        if (MaxSpawn > 100)
            MaxSpawn = 100;
        MaxSameSpawn.y += _MaxSameSpawn;
        if (!(MaxSameSpawn.y > 15)) return;
        MaxSameSpawn.y = 15;
        if (MaxSameSpawn.x < 10)
            MaxSameSpawn.x++;
        

    }
}