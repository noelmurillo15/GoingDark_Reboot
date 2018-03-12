///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using System;
using UnityEngine;
using GoingDark.Core.Enums;


[Serializable]
public class HealthProperties {


	#region Variables
	ShipMaster shipMaster;
	public bool isDead;
	public bool healthCritical;
	public float health;
	public float maxHealth;
	#endregion


	/// <summary>
	/// Constructor : Player
	/// </summary>
	/// <param name="_hp"></param>
	/// <param name="_player"></param>
	public HealthProperties(float _hp, ShipMaster _ref)
	{
		isDead = false;
		healthCritical = false;


		health = _hp;
		maxHealth = _hp;
		shipMaster = _ref;

		shipMaster.EventProjectileHit += Damage;
	}

	#region Modifiers
	public void Heal(float _hp)
	{
		health = Mathf.Clamp(health + _hp, 0f, maxHealth);
		if(healthCritical)
		{
			if(HealthPercentage() > .1f)
			{
				healthCritical = false;
			}
		}
	}

	public void Damage(ProjectileType _type, float _dmg)
	{
		health = Mathf.Clamp(health - _dmg, 0f, maxHealth);

		if(health <= 0f)
		{
			shipMaster.EventProjectileHit -= Damage;
			isDead = true;
			shipMaster.CallEventDeath();
		}
		else if(HealthPercentage() <= .1f)
		{
			healthCritical = true;
		}
	}

	public void FullRestore()
	{
		health = maxHealth;
	}

	public float HealthPercentage()
	{
		return (health / maxHealth) * .5f;
	}	
	#endregion
}