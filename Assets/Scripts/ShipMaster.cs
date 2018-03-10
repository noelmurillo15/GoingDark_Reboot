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
	public float baseDamage = 10f;
	[SerializeField] protected MovementProperties myMoveData;
	[SerializeField] protected HealthProperties myHealthData;
	[SerializeField] protected ShieldProperties myShieldData;
	[SerializeField] protected DebuffProperties myDebuffData;
	[SerializeField] protected Transform myAttackTarget;
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
		myDebuffData = new DebuffProperties();
		myHealthData = new HealthProperties(hp, this);
		myShieldData = new ShieldProperties(hp, shield);
		myMoveData = new MovementProperties(0f, 1f, 100f, 50f, 5f);
	}
	/// <summary>
	/// Initializes Enemy
	/// </summary>
	/// <param name="hp"></param>
	/// <param name="shield"></param>
	protected void Initialize(float hp, bool shield)
	{
		myAttackTarget = null;
		myDebuffData = new DebuffProperties();
		myHealthData = new HealthProperties(hp, this);
		myMoveData = new MovementProperties();
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
		Debug.Log("Event Death Called");
		if (EventDeath != null)
		{
			if (GetComponent<EnemyTrail>() != null)
				GetComponent<EnemyTrail>().Kill();

			EventDeath();

			//myManager.RemoveEnemy(this);
			Destroy(gameObject, 1f);
		}
	}

	public void CallEventProjectileHit(ProjectileType _type, float baseDmg, Impairments debuff)
	{
		Debug.Log("Event Projectile Hit Called");
		if (EventProjectileHit != null)
		{
			EventProjectileHit(_type, baseDmg);
			myHealthData.Damage(baseDmg);
			myDebuffData.ActivateDebuff(debuff, 5f);
		}
	}
	#endregion
}