///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using System;
using UnityEngine;


[Serializable]
public class HealthProperties {


	#region Variables
	public float health;
	public float maxHealth;
	public bool hpCritical;
	public bool isDead;

	ShipMaster shipMaster;
	#endregion


	/// <summary>
	/// Constructor : Player
	/// </summary>
	/// <param name="_hp"></param>
	/// <param name="_player"></param>
	public HealthProperties(float _hp, ShipMaster _ref)
	{
		isDead = false;
		hpCritical = false;

		health = _hp;
		maxHealth = _hp;
		shipMaster = _ref;
	}

	#region Modifiers
	public void Heal(float _hp)
	{
		health = Mathf.Clamp(health + _hp, 0f, maxHealth);
		if(hpCritical)
		{
			if(HealthPercentage() > .1f)
			{
				hpCritical = false;
			}
		}
	}

	public void Damage(float dmg)
	{
		health = Mathf.Clamp(health - dmg, 0f, maxHealth);
		if (health <= 0f)
		{
			isDead = true;
			shipMaster.CallEventDeath();
		}
		else if (HealthPercentage() <= .1f)
		{
			hpCritical = true;
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