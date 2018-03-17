///<summary>
/// 3/12/2018
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
		public void Initialize(ProjectileType _type)
		{
			type = _type;
		}
	}

	[Header("Laser")]
	[SerializeField] Laser[] lasers = new Laser[2];
	[Space]
	[SerializeField] Transform myTarget;
	[Space]
	[SerializeField] Transform gun1;
	[SerializeField] Transform gun2;
	[SerializeField] LaserSystemUI playerUI;
	bool flip;
	int currType = 0;
	bool singleBarrel;
	bool isPlayerControlled = false;
	ShipMaster shipMaster;
	Transform myTransform;
	#endregion


	#region Unity Functions
	void Start()
	{
		flip = false;
		myTransform = transform;
		if (transform.root.GetComponent<ShipMaster>() != null)
		{
			shipMaster = transform.root.GetComponent<ShipMaster>();
			shipMaster.EventSetAttackTargetCoordinates += LockOn;
		}
		else
		{
			Debug.LogError("Could not find shipmaster");
		}
		if (transform.root.GetComponent<LaserSystemUI>() != null)
		{
			isPlayerControlled = true;
			playerUI = transform.root.GetComponent<LaserSystemUI>();

			for (int i = 0; i < 2; i++)
				lasers[i].Initialize(ProjectileType.BasicLaser + i);
		}

		if (maxCooldown == 0f)
			maxCooldown = 1f;

		if (gun2 == null && gun1 == null)
		{
			Debug.LogError("No guns attached");
			return;
		}

		if (gun1 == null || gun2 == null)
			singleBarrel = true;
	}

	void OnDisable()
	{
		shipMaster.EventSetAttackTargetCoordinates -= LockOn;
	}

	void FixedUpdate()
	{
		if (cooldown > 0f)
			cooldown -= Time.deltaTime;
		else cooldown = 0f;

		if (isPlayerControlled)
		{
			if (Activated)
				Shoot();
		}
	}
	#endregion

	#region Public Functions
	public void LockOn(Transform target)
	{
		myTarget = target;
		if (myTarget != null)
		{
			Vector3 playerDir = target.position - myTransform.position;
			Vector3 direction = Vector3.RotateTowards(myTransform.forward, playerDir, Time.fixedDeltaTime * 30f, 15f);
			myTransform.rotation = Quaternion.LookRotation(direction);
			if (Vector3.Angle(playerDir, direction) <= 2f)
				Shoot();
		}
	}

	public void WeaponSwap()
	{
		currType++;
		if (currType == (int)ProjectileType.LaserEnd - (int)ProjectileType.BasicLaser)
			currType = 0;

		if (isPlayerControlled)
			playerUI.LaserSwap(lasers[currType]);
	}
	#endregion

	#region Private Functions
	void Shoot()
	{
		if (cooldown == 0f)
		{
			cooldown = maxCooldown;
			DeActivate();
			FireLaser();
			flip = !flip;
		}
	}

	void FireLaser()
	{
		GameObject projectile = ObjectPoolManager.Instance.GetProjectile(ProjectileType.BasicLaser + currType);
		if (projectile != null)
		{
			if (flip && !singleBarrel)
			{
				projectile.transform.position = gun1.position;
				projectile.transform.rotation = gun1.rotation;
			}
			else if (!singleBarrel)
			{
				projectile.transform.position = gun2.position;
				projectile.transform.rotation = gun2.rotation;
			}
			else
			{
				projectile.transform.position = gun1.position;
				projectile.transform.rotation = gun1.rotation;
			}
			projectile.SetActive(true);
			projectile.GetComponent<ProjectileMaster>().SetIsPlayerControlled(isPlayerControlled);
		}
	}
	#endregion
}