using UnityEngine;
using ANM.Utilities;
using GoingDark.Core.Enums;

public class Orbs : MonoBehaviour
{
    public int orbHp;
    [SerializeField]
    private BossStats boss;

    void OnEnable()
    {       

        orbHp = 100;
    }

    //public void MissileHit(MissileProjectile missile)
    //{
    //    switch (missile.Type)
    //    {
    //        case ProjectileType.BasicMissile:
    //            orbHp -= 35;
    //            break;
    //        case ProjectileType.EmpMissile:
    //            orbHp -= 5;
    //            break;
    //        case ProjectileType.ShieldBreakMissile:
    //            orbHp -= 5;
    //            break;
    //        case ProjectileType.ChromaticMissile:
    //            orbHp -= 100;
    //            break;
    //    }
    //    missile.Kill();
    //    if (orbHp <= 0f)
    //        Kill();
    //}

    //public void LaserDmg(LaserProjectile laser)
    //{
    //    switch (laser.Type)
    //    {
    //        case ProjectileType.BasicLaser:
    //            orbHp -= 25;
    //            break;
    //        case ProjectileType.ChargedLaser:
    //            orbHp -= 50;
    //            break;
    //    }
    //    laser.Kill();
    //    if(orbHp <= 0f)
    //        Kill();
    //}

    void Kill()
    {
        GameObject go = ObjectPoolManager.Instance.GetEnemyExplosion();
        go.transform.position = transform.position;
        go.SetActive(true);

        boss.DecreaseOrbCount();
        gameObject.SetActive(false);
    }
}