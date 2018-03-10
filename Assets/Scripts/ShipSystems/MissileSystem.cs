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
		public float maxFireRate;

		public void Initialize(ProjectileType _type)
		{
			type = _type;
			count = 10;
			maxFireRate = 1f;
		}
	}

	[SerializeField] int currType = 0;
	[SerializeField] bool isPlayerControlled = false;
	[SerializeField] Missile[] missiles = new Missile[8];

	[SerializeField] Hitmarker lockon;
	[SerializeField] Transform fireLocation;
	[SerializeField] MissileSystemUI playerUI;

    Transform myTransform;
	#endregion



	#region Unity Functions
	void Start()
    {
		maxCooldown = 1f;
        myTransform = transform;

		if (transform.root.GetComponent<MissileSystemUI>() != null)
		{
			isPlayerControlled = true;
			playerUI = transform.root.GetComponent<MissileSystemUI>();
			lockon = GameObject.Find("PlayerReticle").GetComponent<Hitmarker>();
			for (int i = 0; i < (int)ProjectileType.MissileEnd - 1; i++)
				missiles[i].Initialize((ProjectileType)i);

			playerUI.UpdateUI(missiles[currType]);
		}
    }

    void FixedUpdate()
    {
		if (isPlayerControlled)
		{
			if (Activated)
				Launch();

			if (cooldown > 0f)
				cooldown -= Time.deltaTime;
		}
		else
		{

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
	public void Launch()
    {
		if (GetMissileCount() > 0)
        {
			DeActivate();
			GetMissile();			
			AudioManager.instance.PlayMissileLaunch();
		}
    }

    public void WeaponSwap()
    {
		currType++;
		if (currType == (int)ProjectileType.MissileEnd - 1)
			currType = 0;

		playerUI.MissileSwap(missiles[currType]);
	}
   
	public void AddRandomMissile()
    {
		missiles[currType].count += 10;
		if (isPlayerControlled)
			playerUI.UpdateUI(missiles[currType]);
	}
	#endregion

	#region Private Functions
	void LockOn(Transform target)
	{
		Debug.Log("Locking On");
		Vector3 playerDir = target.position - myTransform.position;
		Vector3 direction = Vector3.RotateTowards(myTransform.forward, playerDir, Time.fixedDeltaTime * 30f, 15.0f);
		myTransform.rotation = Quaternion.LookRotation(direction);

		if (Vector3.Angle(playerDir, direction) <= .1f)
			Launch();
	}

	void GetMissile()
	{
		GameObject obj = ObjectPoolManager.Instance.GetProjectile((ProjectileType)currType);
		if (obj != null)
		{
			missiles[currType].count--;
			obj.transform.position = fireLocation.position;
			obj.transform.rotation = fireLocation.rotation;
			obj.SetActive(true);
			obj.GetComponent<ProjectileMaster>().SetIsPlayerControlled(isPlayerControlled);

			if (isPlayerControlled)
			{
				playerUI.UpdateUI(missiles[currType]);				
				if (lockon.GetLockedOn())
					obj.SendMessage("LockedOn", lockon.GetRaycastHit());
			}
		}
		else
			ObjectPoolManager.Instance.InitializeProjectile((ProjectileType)currType);
	}	
	#endregion
}