///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using System;
using UnityEngine;
using GoingDark.Core.Enums;


[Serializable]
public class DebuffProperties : MonoBehaviour{


	#region Variables
	ShipMaster shipMaster;
	[SerializeField] bool stunned;
	[SerializeField] bool slowed;
	[SerializeField] bool enflamed;
	[SerializeField] bool sysrupt;
	[SerializeField] GameObject stunParticles;
    [SerializeField] GameObject slowParticles;
	[SerializeField] GameObject enflameParticles;
	[SerializeField] GameObject sysruptParticles;
	#endregion

	private void Start()
	{
		shipMaster = GetComponent<ShipMaster>();
		stunParticles = transform.GetChild(1).GetChild(0).gameObject;
		slowParticles = transform.GetChild(1).GetChild(1).gameObject;
		enflameParticles = transform.GetChild(1).GetChild(2).gameObject;
		sysruptParticles = transform.GetChild(1).GetChild(3).gameObject;
	}


	public void ActivateDebuff(Impairments debuff, float duration)
	{
		if (debuff == Impairments.None)
			return; 

		switch (debuff)
		{
			case Impairments.Slowed:				
				Slow(duration);
				break;
			case Impairments.Stunned:
				Stun(duration);
				break;
			case Impairments.Enflamed:
				Enflame(duration);
				break;
			case Impairments.Sysrupt:
				Sysrupt(duration);
				break;
		}
	}

	#region Private Functions
	void Stun(float duration)
	{
		if (IsInvoking("RemoveStun"))
			CancelInvoke("RemoveStun");

		stunned = true;
		shipMaster.GetMoveData().boost = 0f;
		stunParticles.SetActive(true);
		Invoke("RemoveStun", duration);
	}
	void Slow(float duration)
	{
		if (IsInvoking("RemoveSlow"))
			CancelInvoke("RemoveSlow");

		slowed = true;
		shipMaster.GetMoveData().boost = .25f;
		slowParticles.SetActive(true);
		Invoke("RemoveSlow", duration);
	}
	void Enflame(float duration)
	{
		enflamed = true;
		if (IsInvoking("RemoveEnflame"))
			CancelInvoke("RemoveEnflame");

		stunParticles.SetActive(true);
		Invoke("RemoveEnflame", duration);
	}
	void Sysrupt(float duration)
	{
		sysrupt = true;
		if (IsInvoking("RemoveSysrupt"))
			CancelInvoke("RemoveSysrupt");

		slowParticles.SetActive(true);
		Invoke("RemoveSysrupt", duration);
	}

	void RemoveStun()
    {
		stunned = false;
		shipMaster.GetMoveData().boost = 1f;
		stunParticles.SetActive(false);
    }
    void RemoveSlow()
    {
		slowed = false;
		shipMaster.GetMoveData().boost = 1f;
		slowParticles.SetActive(false);
    }
	void RemoveEnflame()
	{
		enflamed = false;
		enflameParticles.SetActive(false);
	}
	void RemoveSysrupt()
	{
		sysrupt = false;
		sysruptParticles.SetActive(false);
	}
	#endregion
}