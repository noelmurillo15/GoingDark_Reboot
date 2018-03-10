///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;
using ANM.Utilities;
using GoingDark.Core.Enums;


public class ProjectileMaster : MonoBehaviour {


	#region Variables
	[Header("Projectile General")]
	[SerializeField] bool isPlayerControlled = false;
	[SerializeField] ProjectileType _type;
	[SerializeField] Impairments debuff;
	[SerializeField] float speed = 10f;
	[SerializeField] float damage = 10f;
	[SerializeField] float aliveTimer = 10f;
	[SerializeField] AudioClip audioClip;

	protected bool init = false;
	protected Transform myTransform;
	#endregion


	void OnEnable()
	{
		Invoke("Kill", 3f);
	}
	#region Accessors
	public bool GetIsPlayerControlled() { return isPlayerControlled; }

	public ProjectileType GetProjectileType() { return _type; }
	public Impairments GetDebuff() { return debuff; }

	public float GetSpeed() { return speed; }
	public float GetDamage() { return damage; }
	public float GetAliveTimer() { return aliveTimer; }

	public AudioClip GetAudio() { return audioClip; }
	#endregion

	#region Modifiers
	public void SetIsPlayerControlled(bool b)
	{
		isPlayerControlled = b;
	}
	#endregion

	#region Collision
	void OnCollisionEnter(Collision col)
	{
		if (GetIsPlayerControlled())
		{
			if (col.transform.CompareTag("Enemy") || col.transform.CompareTag("Orb"))
			{
				if (col.gameObject.GetComponent<ShipMaster>() != null)
					col.gameObject.GetComponent<ShipMaster>().CallEventProjectileHit(GetProjectileType(), GetDamage(), GetDebuff());
			}

			if (col.transform.CompareTag("Asteroid"))
			{
				col.transform.SendMessage("Kill");
				Kill();
			}
		}
		else
		{
			if (col.transform.CompareTag("Player"))
			{
				if (IsInvoking("Kill"))
					CancelInvoke("Kill");

				if (col.gameObject.GetComponent<ShipMaster>() != null)
					col.gameObject.GetComponent<ShipMaster>().CallEventProjectileHit(GetProjectileType(), GetDamage(), GetDebuff());				
			}
			if (col.transform.CompareTag("Asteroid") || col.transform.CompareTag("Decoy"))
			{
				col.transform.SendMessage("Kill");
				Kill();
			}
		}
	}
	#endregion

	#region Recycle
	void SetInactive()
	{
		gameObject.SetActive(false);
	}
	void Kill()
	{
		if (IsInvoking("Kill"))
			CancelInvoke("Kill");

		GameObject go = ObjectPoolManager.Instance.GetProjectileExplosion(GetProjectileType());

		if (go != null)
		{
			go.transform.position = myTransform.position;
			go.SetActive(true);
		}
		SetInactive();
	}
	#endregion
}