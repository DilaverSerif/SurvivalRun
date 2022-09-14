using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Lean.Touch;

public enum WeaponType
{
    Pistol,
    Shotgun,
    Auto,
    HalfAuto
}

public enum WeaponStat
{
    Shooting,
    Reloading,
    Idle
}
public abstract class Weapon: MonoBehaviour
{
    public float ShootingSpeed;
    public int Damage;
    public bool HaveMagazine;
    public Enum_PoolObject Bullet;
    [ShowIf("HaveMagazine")]
    public int MagazineAmmo;
    [ShowIf("HaveMagazine")]
    public int ReserveAmmo;
    public int MaxMagazineAmmo;

    [ShowIf("HaveMagazine")]
    public float ReloadSpeed;

    public bool BullShit;
    [ShowIf("BullShit")]
    public AnimationCurve BullShitCurve;

    protected float ShootingTiming;
    protected WeaponStat _weaponStat = WeaponStat.Idle;
    protected Coroutine StartShootingTiming;
    protected Coroutine StartSpawnBullet;
    protected virtual void Shoot(LeanFinger leanFinger)
    {
        if (_weaponStat == WeaponStat.Reloading | StartSpawnBullet != null) return;
        _weaponStat = WeaponStat.Shooting;
        // if(StartShootingTiming != null) 
        //     StartShootingTiming = StartCoroutine("ShootingTimer");
        StartSpawnBullet = StartCoroutine(SpawnBullet());
    }

    private void StopShoot(LeanFinger obj)
    {
        if(_weaponStat != WeaponStat.Shooting) return;
        _weaponStat = WeaponStat.Idle;
        StopCoroutine(StartShootingTiming);
    }
    
    protected virtual void Reload()
    {
        _weaponStat = WeaponStat.Reloading;
        
        ReserveAmmo += MagazineAmmo;
        MagazineAmmo = 0;

        if (ReserveAmmo >= MaxMagazineAmmo)
        {
            MagazineAmmo = MaxMagazineAmmo;
            ReserveAmmo -= MaxMagazineAmmo;
        }
        else
        {
            MagazineAmmo = ReserveAmmo;
            ReserveAmmo = 0;
        }

        DOVirtual.DelayedCall(ReloadSpeed,()=> _weaponStat = WeaponStat.Idle);
    }

    protected abstract IEnumerator SpawnBullet();
    
    protected IEnumerator ShootingTimer()
    {
        while (_weaponStat == WeaponStat.Shooting)
        {
            ShootingTiming += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        ShootingTiming = 0;
        yield return new WaitForEndOfFrame();
    }
    
    private void OnEnable()
    {
        LeanTouch.OnFingerDown += Shoot;
        LeanTouch.OnFingerUp += StopShoot;
    }
    
    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= Shoot;
        LeanTouch.OnFingerUp -= StopShoot;
    }
}
