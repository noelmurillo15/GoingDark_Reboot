using UnityEngine;
using GoingDark.Core.Enums;

public class ShieldSystem : MonoBehaviour
{

    private ShieldProperties ShieldData;

    // Use this for initialization
    void Start()
    {
        ShieldData = new ShieldProperties(100f, gameObject);
    }

    //public void MissileHit(MissileProjectile missile)
    //{
    //    if (ShieldData.GetShieldActive())
    //    {
    //        switch (missile.Type)
    //        {
    //            case ProjectileType.BasicMissile:
    //                missile.Deflect();
    //                break;
    //            case ProjectileType.EmpMissile:
    //                missile.Deflect();
    //                break;
    //            case ProjectileType.ChromaticMissile:
    //                missile.Deflect();
    //                break;
    //            case ProjectileType.ShieldBreakMissile:
    //                ShieldData.Damage(100);
    //                missile.Kill();
    //                break;
    //        }
    //    }
    //}

    //public void LaserHit(LaserProjectile laser)
    //{
    //    switch (laser.Type)
    //    {
    //        case ProjectileType.BasicLaser:
    //            ShieldData.Damage(5f);
    //            break;
    //        case ProjectileType.ChargedLaser:
    //            ShieldData.Damage(12.5f);
    //            break;
    //    }
    //}
}