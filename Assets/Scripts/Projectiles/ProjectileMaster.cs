///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;
using GoingDark.Core.Enums;


public class ProjectileMaster : MonoBehaviour {


	#region Variables
	[Header("Projectile General")]
	[SerializeField] bool isPlayerControlled = false;
	[SerializeField] ProjectileType _type;
	[SerializeField] Impairments debuff;
	[SerializeField] float damage = 10f;
	[SerializeField] float curSpeed = 10f;
	[SerializeField] float maxSpeed = 10f;
	[SerializeField] float aliveTimer = 10f;
	[SerializeField] AudioClip audioClip;

	[SerializeField] GameObject projectile;
	[SerializeField] GameObject explosion;

	protected bool init = false;
	protected Transform myTransform;
	#endregion



	void OnDisable()
	{
		curSpeed = maxSpeed;
		if (projectile != null)
			projectile.SetActive(true);
	}

	#region Accessors
	public bool GetIsPlayerControlled() { return isPlayerControlled; }

	public ProjectileType GetProjectileType() { return _type; }
	public Impairments GetDebuff() { return debuff; }

	public float GetDamage() { return damage; }
	public float GetCurSpeed() { return curSpeed; }
	public float GetMaxSpeed() { return maxSpeed; }
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
			if (col.transform.CompareTag("Enemy"))
			{				
				if (col.gameObject.GetComponent<ShipMaster>() != null)
					col.gameObject.GetComponent<ShipMaster>().CallEventProjectileHit(GetProjectileType(), GetDamage(), GetDebuff());
				Kill();
			}

			if (col.transform.CompareTag("Asteroid"))
			{
				col.transform.SendMessage("Kill");
				Kill();
			}
		}
		else
		{
			Debug.Log("Projectile controlled by Enemy");
			if (col.transform.CompareTag("Player"))
			{
				if (col.gameObject.GetComponent<ShipMaster>() != null)
					col.gameObject.GetComponent<ShipMaster>().CallEventProjectileHit(GetProjectileType(), GetDamage(), GetDebuff());

				Debug.Log("Player was Hit by Enemy!");
				Kill();
			}
			if (col.transform.CompareTag("Asteroid"))
			{
				Debug.Log("Asteroid was Hit by Enemy!");
				col.transform.SendMessage("Kill");
				Kill();
			}
		}
	}
	#endregion

	#region Recycle
	void SetInactive()
	{
		curSpeed = 0;
		if (projectile != null)
			projectile.SetActive(false);
		else
			Debug.Log("Projectile has no Projectile reference attached!");
	}

	public void Kill()
	{		
		if (IsInvoking("Kill"))
			CancelInvoke("Kill");

		SetInactive();
		if (explosion != null)
		{
			explosion.transform.position = myTransform.position;
			explosion.SetActive(true);
		}
		else
		{			
			gameObject.SetActive(false);
			Debug.Log("Projectile has no explosion attached!");
		}		
	}
	#endregion
}