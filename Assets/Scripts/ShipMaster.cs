///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;
using GoingDark.Core.Enums;


[System.Serializable]
public class ShipMaster : MonoBehaviour {


	#region Variables
	[Header("General")]
	[SerializeField] protected Transform myAttackTarget;
	[Space]
	[SerializeField] protected MovementProperties myMoveData;
	[Space]
	[SerializeField] protected HealthProperties myHealthData;
	[Space]
	[SerializeField] protected ShieldProperties myShieldData;
	[Space]
	[SerializeField] protected DebuffProperties myDebuffData;

	[SerializeField] GameObject stunnedPrefab;
	#endregion

	#region Events
	public delegate void GeneralEventHandler();
	public delegate void ProjectileHitEventHandler(ProjectileType _type, float baseDmg);

	public event GeneralEventHandler EventDeath;
	public event GeneralEventHandler EventOnHit;
	public event GeneralEventHandler EventCriticalHealth;
	public event GeneralEventHandler EventHealthRecovered;

	public event ProjectileHitEventHandler EventProjectileHit;
	#endregion


	/// <summary>
	/// Initializes Player
	/// </summary>
	/// <param name="hp"></param>
	/// <param name="hpBar"></param>
	/// <param name="shieldBar"></param>
	protected void Initialize(float hp, GameObject shield)
	{
		myAttackTarget = null;
		myHealthData = new HealthProperties(hp, this);
		myShieldData = new ShieldProperties(hp, shield);
		myMoveData = new MovementProperties(0f, 1f, 100f, 50f, 5f);
		myDebuffData = gameObject.AddComponent<DebuffProperties>();
	}
	/// <summary>
	/// Initializes Enemy
	/// </summary>
	/// <param name="hp"></param>
	/// <param name="shield"></param>
	protected void Initialize(float hp, bool shield)
	{
		myAttackTarget = null;
		myHealthData = new HealthProperties(hp, this);
		myMoveData = new MovementProperties(0f, 1f, 100f, .5f, 20f);
		myDebuffData = gameObject.AddComponent<DebuffProperties>();
		if (shield)
			myShieldData = new ShieldProperties(hp, transform.GetChild(0).gameObject);
	}	

	#region Accessors
	public MovementProperties GetMoveData() { return myMoveData; }
	public HealthProperties GetHealthData() { return myHealthData; }
	public ShieldProperties GetShieldData() { return myShieldData; }
	public DebuffProperties GetDebuffData() { return myDebuffData; }
	#endregion

	#region Event Methods
	public void CallEventOnHit()
	{
		Debug.Log("Event Get Hit Called");
		if (EventOnHit != null)
		{
			EventOnHit();
		}
	}

	public void CallEventInBounds()
	{
		Debug.Log("Event in bounds");
	}

	public void CallEventOutOfBounds()
	{
		Debug.Log("Event out of bounds");
	}

	public void CallEventCriticalHealth()
	{
		Debug.Log("Event Low Health Called");
		if (EventCriticalHealth != null)
		{
			EventCriticalHealth();
		}
	}

	public void CallEventHealthRecovered()
	{
		Debug.Log("Event Health Recovered Called");
		if (EventHealthRecovered != null)
		{
			EventHealthRecovered();
		}
	}

	public void CallEventDeath()
	{
		if (EventDeath != null)
		{
			Debug.Log("Death Event was succesful!");
			EventDeath();		
		}
	}

	public void CallEventProjectileHit(ProjectileType _type, float baseDmg, Impairments debuff)
	{
		if (EventProjectileHit != null)
		{
			Debug.Log("ProjectileHit Event was succesful!");
			EventProjectileHit(_type, baseDmg);
		}
		if (debuff != Impairments.None)
		{
			myDebuffData.ActivateDebuff(debuff, 5f);	
		}
	}
	#endregion

	

	//#region Player Damage Calls   
	//public void ProjectileHit(ProjectileType _type, float baseDmg)
	//{
	//	Debug.Log("Projectile Hit Called");
	//	switch (_type)
	//	{
	//		case ProjectileType.BasicMissile:
	//			Debug.Log("Player Hit by : BasicMissile");
	//			break;
	//		case ProjectileType.EmpMissile:
	//			Debug.Log("Player Hit by : EmpMissile");
	//			break;
	//		case ProjectileType.ShieldBreakMissile:
	//			Debug.Log("Player Hit by : ShieldBreakMissile");
	//			break;
	//		case ProjectileType.ChromaticMissile:
	//			Debug.Log("Player Hit by : ChromaticMissile");
	//			break;
	//		case ProjectileType.SlowMissile:
	//			Debug.Log("Player Hit by : SlowMissile");
	//			break;
	//		case ProjectileType.SysruptMissile:
	//			Debug.Log("Player Hit by : SysruptMissile");
	//			break;
	//		case ProjectileType.BasicLaser:
	//			Debug.Log("Player Hit by : BasicLaser");
	//			break;
	//		case ProjectileType.ChargedLaser:
	//			Debug.Log("Player Hit by : ChargedLaser");
	//			break;
	//		default:
	//			Debug.Log("Unknown Projectile");
	//			break;
	//	}
	//}
	//public void CrashHit(float _speed)
	//{
	//	//controller.AddRumble(1f, rumbleIntesity);
	//	myHealthData.Damage(_speed * 20f);
	//	healthBar.fillAmount = GetHealthData().HealthPercentage();
	//	shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
	//	UnCloak();
	//}


	//void ShieldHit(LaserProjectile laser)
	//{
	//	//controller.AddRumble(.5f, rumbleIntesity);
	//	myShieldData.Damage(laser.GetBaseDmg() * dmgMultiplier);
	//	healthBar.fillAmount = GetHealthData().HealthPercentage();
	//	shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
	//	laser.Kill();
	//}
	//void ShieldHit(MissileProjectile missile)
	//{
	//	//controller.AddRumble(.5f, rumbleIntesity);
	//	myShieldData.Damage(missile.GetBaseDmg() * dmgMultiplier);
	//	healthBar.fillAmount = GetHealthData().HealthPercentage();
	//	shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
	//	missile.Kill();
	//}
	//void LaserDmg(LaserProjectile laser)
	//{
	//	UnCloak();
	//	if (myShieldData.GetShieldActive())
	//	{
	//		ShieldHit(laser);
	//		ShieldRecharge();
	//		return;
	//	}
	//	//controller.AddRumble(1f, rumbleIntesity);
	//	myHealthData.Damage(laser.GetBaseDmg() * dmgMultiplier);
	//	healthBar.fillAmount = GetHealthData().HealthPercentage();
	//	shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
	//	laser.Kill();
	//}
	//void MissileDebuff(MissileProjectile missile)
	//{
	//	switch (missile.Type)
	//	{
	//		case ProjectileType.SlowMissile:
	//			SlowHit();
	//			break;
	//		case ProjectileType.EmpMissile:
	//			EMPHit();
	//			break;
	//		case ProjectileType.SysruptMissile:
	//			SysruptHit();
	//			break;
	//		case ProjectileType.ShieldBreakMissile:
	//			ShieldHit(missile);
	//			break;
	//	}
	//	missile.Kill();
	//}
	//void MissileDmg(MissileProjectile missile)
	//{
	//	UnCloak();
	//	if (myShieldData.GetShieldActive())
	//	{
	//		ShieldHit(missile);
	//		ShieldRecharge();
	//		return;
	//	}
	//	//controller.AddRumble(1f, rumbleIntesity);
	//	myHealthData.Damage(missile.GetBaseDmg() * dmgMultiplier);
	//	healthBar.fillAmount = GetHealthData().HealthPercentage();
	//	shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
	//	missile.Kill();
	//}
	//#endregion

	//#region Enemy Damage Calls 
	//void EMPHit()
	//{
	//	Debug.Log("Enemy Emp Hit");
	//	stunnedPrefab.SetActive(true);
	//	myImpairments = Impairments.Stunned;
	//	Invoke("ResetDebuff", 5f);
	//}

	//void SplashDmg()
	//{
	//	if (myType != EnemyTypes.FinalBoss)
	//	{
	//		Debug.Log("Enemy Splash Dmg Hit");
	//		if (hasShield && myShieldData.GetShieldActive())
	//			myShieldData.Damage(myShieldData.maxHealth * .1f);
	//		else
	//			myHealthData.Damage(myHealthData.maxHealth * .1f);
	//	}
	//}
	//public void CrashHit(float _speed)
	//{
	//	if (myType != EnemyTypes.FinalBoss)
	//	{
	//		Debug.Log("Enemy Crashed");

	//		if (hasShield && myShieldData.GetShieldActive())
	//			myShieldData.Damage(_speed * myShieldData.maxHealth * .5f);
	//		else
	//			myHealthData.Damage(_speed * myHealthData.maxHealth * .5f);
	//	}
	//}
	//public void MissileHit(MissileProjectile missile)
	//{
	//	Debug.Log("Enemy Missile Hit");
	//	if (hasShield && myShieldData.GetShieldActive())
	//	{
	//		if (myType != EnemyTypes.FinalBoss)
	//		{
	//			if (missile.Type == ProjectileType.ShieldBreakMissile)
	//			{
	//				myShieldData.Damage(100f);
	//				missile.Kill();
	//			}
	//			else if (missile.Type == ProjectileType.EmpMissile)
	//			{
	//				EMPHit();
	//				missile.Kill();
	//			}
	//			else
	//				missile.Deflect();
	//		}
	//		else
	//		{
	//			missile.Deflect();
	//		}
	//	}
	//	else
	//	{
	//		myHealthData.Damage(missile.GetBaseDmg());
	//		missile.Kill();
	//	}
	//}
	//public void LaserDmg(LaserProjectile laser)
	//{
	//	Debug.Log("Enemy Laser Hit");
	//	if (hasShield && myShieldData.GetShieldActive())
	//	{
	//		if (myType != EnemyTypes.FinalBoss)
	//			myShieldData.Damage(laser.GetBaseDmg());
	//	}
	//	else
	//		myHealthData.Damage(laser.GetBaseDmg());

	//	laser.Kill();
	//}
	//#endregion
}