///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;
using ANM.Utilities;
using GoingDark.Core.Enums;


public class MissileSystem : ShipSystem {


	#region Variables
	[System.Serializable]
	public struct Missile
	{
		public ProjectileType type;
		public int count;

		public void Initialize(ProjectileType _type)
		{
			type = _type;
			count = 10;
		}
	}

	[Header("Missile")]
	[SerializeField] int currType = 0;
	[SerializeField] bool isPlayerControlled = false;
	[SerializeField] Missile[] missiles = new Missile[8];

	[SerializeField] Hitmarker lockon;
	[SerializeField] Transform fireLocation;
	[SerializeField] MissileSystemUI playerUI;

	ShipMaster shipMaster;
    Transform myTransform;
	Transform myTarget;
	#endregion



	#region Unity Functions
	void Start()
    {
        myTransform = transform;
		if (maxCooldown == 0f)
			maxCooldown = 3f;

		currType = (int)missiles[0].type;

		shipMaster = transform.root.GetComponent<ShipMaster>();
		if (shipMaster == null)
			Debug.LogError("No ShipMaster Reference Attached!");
		else
			shipMaster.EventSetAttackTargetCoordinates += LockOn;

		if (transform.root.GetComponent<MissileSystemUI>() != null)
		{
			isPlayerControlled = true;
			playerUI = transform.root.GetComponent<MissileSystemUI>();
			lockon = GameObject.Find("PlayerReticle").GetComponent<Hitmarker>();
			for (int i = 0; i < (int)ProjectileType.MissileEnd; i++)
				missiles[i].Initialize((ProjectileType)i);

			playerUI.UpdateUI(missiles[currType]);
		}

		if (fireLocation == null)
			Debug.LogError("Missile system has no fire location attached!");
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
			{
				Shoot();
				AudioManager.instance.PlayMissileLaunch();
			}
		}	
    }
	#endregion

	#region Accessors
	public int GetMissileCount()
    {
        return missiles[currType].count;
    }
	#endregion 

	#region Public Functions
	public void Shoot()
    {
		if (GetMissileCount() > 0 && cooldown == 0f)
        {
			DeActivate();
			FireMissile();						
		}
    }
    public void WeaponSwap()
    {
		currType++;
		if (currType == (int)ProjectileType.MissileEnd)
			currType = 0;

		playerUI.MissileSwap(missiles[currType]);
	}   
	public void AddRandomMissile()
    {
		missiles[currType].count += 10;
		if (isPlayerControlled)
			playerUI.UpdateUI(missiles[currType]);
	}
	public void LockOn(Transform target)
	{
		myTarget = target;
		if (myTarget != null)
		{
			Vector3 playerDir = myTarget.position - myTransform.position;
			Vector3 direction = Vector3.RotateTowards(myTransform.forward, playerDir, Time.fixedDeltaTime * 30f, 15f);
			myTransform.rotation = Quaternion.LookRotation(direction);
			if (Vector3.Angle(playerDir, direction) <= 2f)
				Shoot();
		}
	}
	#endregion

	#region Private Functions
	void FireMissile()
	{
		GameObject projectile = ObjectPoolManager.Instance.GetProjectile((ProjectileType)currType);
		if (projectile != null)
		{
			missiles[currType].count--;
			projectile.transform.position = fireLocation.position;
			projectile.transform.rotation = fireLocation.rotation;
			projectile.SetActive(true);
			projectile.GetComponent<ProjectileMaster>().SetIsPlayerControlled(isPlayerControlled);

			if (isPlayerControlled)
			{
				playerUI.UpdateUI(missiles[currType]);				
				if (lockon.GetLockedOn())
					projectile.SendMessage("LockedOn", lockon.GetRaycastHit());
			}
		}
	}	
	#endregion
}