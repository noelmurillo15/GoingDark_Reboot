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
	[SerializeField] GameObject stunParticles;
    [SerializeField] GameObject slowParticles;
	[SerializeField] GameObject enflameParticles;
	[SerializeField] GameObject sysruptParticles;
	#endregion



	public void ActivateDebuff(Impairments debuff, float duration)
	{
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

			default:
				break;
		}
	}

	#region Private Functions
	void Stun(float duration)
	{
		if (IsInvoking("RemoveStun"))
			CancelInvoke("RemoveStun");

		//move.GetMoveData().SetMaxSpeed(0f);
		stunParticles.SetActive(true);
		Invoke("RemoveStun", duration);
	}
	void Slow(float duration)
	{
		if (IsInvoking("RemoveSlow"))
			CancelInvoke("RemoveSlow");

		//move.GetMoveData().SetBoost(.5f);
		slowParticles.SetActive(true);
		Invoke("RemoveSlow", duration);
	}
	void Enflame(float duration)
	{
		if (IsInvoking("RemoveEnflame"))
			CancelInvoke("RemoveEnflame");

		stunParticles.SetActive(true);
		Invoke("RemoveEnflame", duration);
	}
	void Sysrupt(float duration)
	{
		if (IsInvoking("RemoveSysrupt"))
			CancelInvoke("RemoveSysrupt");

		slowParticles.SetActive(true);
		Invoke("RemoveSysrupt", duration);
	}

	void RemoveStun()
    {
        //move.GetMoveData().SetMaxSpeed(100f);
        stunParticles.SetActive(false);
    }
    void RemoveSlow()
    {
        //move.GetMoveData().SetBoost(1f);
        slowParticles.SetActive(false);
    }
	void RemoveEnflame()
	{
		//move.GetMoveData().SetMaxSpeed(100f);
		enflameParticles.SetActive(false);
	}
	void RemoveSysrupt()
	{
		//move.GetMoveData().SetBoost(1f);
		sysruptParticles.SetActive(false);
	}
	#endregion
}