using UnityEngine;
using ANM.Utilities;
using GoingDark.Core.Enums;


public class BossLaser : MonoBehaviour {

    [SerializeField]
    private GameObject Laser;
    
    // Use this for initialization
    void Start () {
        Laser.SetActive(false);
        InvokeRepeating("Attack", 5f, 20f);
	}

    void Attack()
    {
        Laser.SetActive(true);
        Invoke("Fire", 4.5f);
        Invoke("Cooldown", 6f);
    }

    void Fire()
    {
        GameObject obj1 = ObjectPoolManager.Instance.GetProjectile(ProjectileType.Cannon);

        if (obj1 != null)
        {
            obj1.SetActive(true);
            obj1.transform.position = Laser.transform.position;
            obj1.transform.rotation = Laser.transform.rotation;
        }
    }

    void Cooldown()
    {
        Laser.SetActive(false);
    }
}
