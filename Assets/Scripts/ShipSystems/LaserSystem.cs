///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;
using ANM.Utilities;
using GoingDark.Core.Enums;


public class LaserSystem : ShipSystem {


	#region Properties
	[System.Serializable]
	public struct Laser
	{
		public ProjectileType type;
		public float maxFireRate;

		public void Initialize(ProjectileType _type)
		{
			type = _type;
			maxFireRate = .2f;
		}
	}

	[SerializeField] bool isPlayerControlled = false;
	[SerializeField] Laser[] lasers = new Laser[2];
	[SerializeField] int currType = 0;
	[SerializeField] Transform gun1;
	[SerializeField] Transform gun2;
	[SerializeField] LaserSystemUI playerUI;
	[SerializeField] bool flip;
	[SerializeField] bool singleBarrel;

	Transform myTransform;
	#endregion


	void Start()
	{
		flip = false;
		currType = 0;
		myTransform = transform;

		if (gun2 == null)
			singleBarrel = true;

		if (transform.root.GetComponent<LaserSystemUI>() != null)
		{
			isPlayerControlled = true;
			playerUI = transform.root.GetComponent<LaserSystemUI>();
			for (int i = 0; i < 2; i++)
				lasers[i].Initialize(ProjectileType.BasicLaser + i);

			maxCooldown = lasers[currType].maxFireRate;
		}
	}

	void FixedUpdate()
	{
		if (isPlayerControlled)
		{
			if (Activated)
				ShootLaser();

			if (cooldown > 0f)
				cooldown -= Time.deltaTime;
		}
		else
		{

		}
	}

	private void LockOn(Transform target)
	{
		Debug.Log("Locking On");
		Vector3 playerDir = target.position - myTransform.position;
		Vector3 direction = Vector3.RotateTowards(myTransform.forward, playerDir, Time.fixedDeltaTime * 30f, 15.0f);
		myTransform.rotation = Quaternion.LookRotation(direction);

		ShootLaser();
	}

	public void ShootLaser()
	{
		DeActivate();
		flip = !flip;
		GetLaser();
	}

	private void GetLaser()
	{
		GameObject laser = ObjectPoolManager.Instance.GetProjectile(ProjectileType.BasicLaser + currType);
		if (flip && !singleBarrel)
		{
			laser.transform.position = gun1.position;
			laser.transform.rotation = gun1.rotation;
		}
		else if (!singleBarrel)
		{
			laser.transform.position = gun2.position;
			laser.transform.rotation = gun2.rotation;
		}
		else
		{
			laser.transform.position = gun1.position;
			laser.transform.rotation = gun1.rotation;
		}
		laser.SetActive(true);
		laser.GetComponent<ProjectileMaster>().SetIsPlayerControlled(isPlayerControlled);
	}

	public void WeaponSwap()
	{
		currType++;
		if (currType == (int)ProjectileType.LaserEnd - (int)ProjectileType.BasicLaser)
			currType = 0;

		if (isPlayerControlled)
			playerUI.LaserSwap(lasers[currType]);
	}
}