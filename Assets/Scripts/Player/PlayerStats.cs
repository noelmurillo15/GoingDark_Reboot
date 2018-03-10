///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GoingDark.Core.Enums;
using UnityEngine.SceneManagement;


public class PlayerStats : ShipMaster {


	#region Variables
	[Header("Player")]
	[SerializeField] Transform station;
	[SerializeField] Image healthBar;
	[SerializeField] Image shieldBar;

	private string diff;
    private float dmgMultiplier;

    private CloakSystem cloak;
    private HyperdriveSystem hype;
    private SystemManager systemManager;

    private MessageScript msgs;
    private DeathTransition deathTransition;
    #endregion
    


    void Awake()
    {
		EventProjectileHit += ProjectileHit;
		PlayerPrefs.SetString("Difficulty", "Medium");
        diff = PlayerPrefs.GetString("Difficulty");         
        switch (diff)
        {
            case "Easy":
                dmgMultiplier = 1f;
                break;
            case "Medium":
                dmgMultiplier = 1.5f;
                break;
            case "Hard":
                dmgMultiplier = 2f;
                break;
            case "Nightmare":
                dmgMultiplier = 3f;
                break;
            default:
                Debug.LogError("Player Could not get Game difficulty");
                diff = "Easy";
                dmgMultiplier = 1f;
                break;
        }

		
        systemManager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
		Invoke("FindSystems", 1.5f);
    }

	void FindSystems()
    {
		base.Initialize(100f, GameObject.FindGameObjectWithTag("Shield"));
		cloak = systemManager.GetSystemScript(SystemType.Cloak) as CloakSystem;
        hype = systemManager.GetSystemScript(SystemType.Hyperdrive) as HyperdriveSystem;
		healthBar.fillAmount = GetHealthData().HealthPercentage();
		shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
	}

	#region Accessors
    public CloakSystem GetCloak()
    {
        return cloak;
    }
    public SystemManager GetSystemData()
    {
        return systemManager;
    }
    #endregion

    #region Modifiers
    public void UnCloak()
    {
        if (cloak.GetCloaked())
            cloak.UnCloakShip();
    }
    public void HealShield()
    {
        if (myShieldData.GetShieldActive())
            myShieldData.Heal(2.5f);
        else
            CancelInvoke("HealShield");

        if (myShieldData.health >= 100)
            CancelInvoke("HealShield");
    }
    public void ShieldRecharge()
    {
        if (IsInvoking("HealShield"))
            CancelInvoke("HealShield");

        if(myShieldData.GetShieldActive())
            InvokeRepeating("HealShield", 10f, .5f);
    }
    public void PlayerSlowed()
    {
        msgs.Slow(8f);
        //myDebuffData.Slow(8f);
    }
    public void PlayerStunned()
    {              
        msgs.Stun(3f);
        //myDebuffData.Stun(3f);
    }
	#endregion

	#region Damage Calls   
	public void ProjectileHit(ProjectileType _type, float baseDmg)
	{
		Debug.Log("Projectile Hit Called");
		switch (_type)
		{
			case ProjectileType.BasicMissile:
				Debug.Log("Player Hit by : BasicMissile");
				break;
			case ProjectileType.EmpMissile:
				Debug.Log("Player Hit by : EmpMissile");
				break;
			case ProjectileType.ShieldBreakMissile:
				Debug.Log("Player Hit by : ShieldBreakMissile");
				break;
			case ProjectileType.ChromaticMissile:
				Debug.Log("Player Hit by : ChromaticMissile");
				break;
			case ProjectileType.SlowMissile:
				Debug.Log("Player Hit by : SlowMissile");
				break;
			case ProjectileType.SysruptMissile:
				Debug.Log("Player Hit by : SysruptMissile");
				break;
			case ProjectileType.BasicLaser:
				Debug.Log("Player Hit by : BasicLaser");
				break;
			case ProjectileType.ChargedLaser:
				Debug.Log("Player Hit by : ChargedLaser");
				break;
			default:
				Debug.Log("Unknown Projectile");
				break;
		}
	}

	public void CrashHit(float _speed)
    {
        //controller.AddRumble(1f, rumbleIntesity);
        myHealthData.Damage(_speed * 20f);
		healthBar.fillAmount = GetHealthData().HealthPercentage();
		shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
		UnCloak();
    }    

    void EMPHit()
    {
        //controller.AddRumble(5f, rumbleIntesity);
        PlayerStunned();
		
		UnCloak();      
    }
    void SlowHit()
    {
        PlayerSlowed();
		healthBar.fillAmount = GetHealthData().HealthPercentage();
		shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
		UnCloak();
    }
    void SysruptHit()
    {
        systemManager.SystemDamaged();
		healthBar.fillAmount = GetHealthData().HealthPercentage();
		shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
		UnCloak();
    }
    void SplashDmg()
    {
        //controller.AddRumble(.25f, rumbleIntesity);
        if (myShieldData.GetShieldActive())
            myShieldData.Damage(5 * dmgMultiplier);
        else
            myHealthData.Damage(5 * dmgMultiplier);
		healthBar.fillAmount = GetHealthData().HealthPercentage();
		shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
	}
  //  void ShieldHit(LaserProjectile laser)
  //  {
  //      //controller.AddRumble(.5f, rumbleIntesity);
  //      myShieldData.Damage(laser.GetBaseDmg() * dmgMultiplier);
		//healthBar.fillAmount = GetHealthData().HealthPercentage();
		//shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
		//laser.Kill();
  //  }
  //  void ShieldHit(MissileProjectile missile)
  //  {
  //      //controller.AddRumble(.5f, rumbleIntesity);
  //      myShieldData.Damage(missile.GetBaseDmg() * dmgMultiplier);
		//healthBar.fillAmount = GetHealthData().HealthPercentage();
		//shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
		//missile.Kill();
  //  }
  //  void LaserDmg(LaserProjectile laser)
  //  {
  //      UnCloak();
  //      if (myShieldData.GetShieldActive())
  //      {
  //          ShieldHit(laser);
  //          ShieldRecharge();
  //          return;
  //      }
  //      //controller.AddRumble(1f, rumbleIntesity);
  //      myHealthData.Damage(laser.GetBaseDmg() * dmgMultiplier);
		//healthBar.fillAmount = GetHealthData().HealthPercentage();
		//shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
		//laser.Kill();                        
  //  }
  //  void MissileDebuff(MissileProjectile missile)
  //  {        
  //      switch (missile.Type)
  //      {
  //          case ProjectileType.SlowMissile:
  //              SlowHit();
  //              break;
  //          case ProjectileType.EmpMissile:
  //              EMPHit();
  //              break;
  //          case ProjectileType.SysruptMissile:
  //              SysruptHit();
  //              break;
  //          case ProjectileType.ShieldBreakMissile:
  //              ShieldHit(missile);
  //              break;
  //      }
  //      missile.Kill();
  //  }
  //  void MissileDmg(MissileProjectile missile)
  //  {
  //      UnCloak();
  //      if (myShieldData.GetShieldActive())
  //      {
  //          ShieldHit(missile);
  //          ShieldRecharge();
  //          return;
  //      }
  //      //controller.AddRumble(1f, rumbleIntesity);
  //      myHealthData.Damage(missile.GetBaseDmg() * dmgMultiplier);
		//healthBar.fillAmount = GetHealthData().HealthPercentage();
		//shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
		//missile.Kill();
  //  }
    #endregion

    #region Death
    public void GoToStation()
    {
        Fade();
    }
	public void Repair(int cost)
	{
		myHealthData.FullRestore();
		myShieldData.FullRestore();
		systemManager.FullSystemRepair();
		healthBar.fillAmount = GetHealthData().HealthPercentage();
		shieldBar.fillAmount = GetShieldData().ShieldHealthPercentage();
	}
    public void Respawn()
    {
        Repair(0);  
        GoToStation();
        if(hype != null)      
            hype.StopHyperdrive();
    }
    void GameOver()
    {
        transform.root.GetComponent<EnemyManager>().AllEnemiesPatrol();
        transform.root.GetComponent<GameOver>().InitializeGameOverScene();
    }
    public void Kill()
    {
		EventProjectileHit -= ProjectileHit;
		deathTransition.Death();
        Invoke("GameOver", 1.5f);
    }
    public void FadeOut()
    {
        FadeToSceneChange();
    }
    #endregion

    #region Coroutines
    IEnumerator Fade()
    {
        deathTransition.SpawnPlayer();
        deathTransition.NotDead();
        yield return new WaitForSeconds(1.0f);

        transform.SendMessage("StopMovement");
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(station.position.x, station.position.y, station.position.z - 150f);
        yield return new WaitForSeconds(1.0f);

        deathTransition.notSpawned();
    }
    IEnumerator FadeToSceneChange()
    {
        deathTransition.SpawnPlayer();
        deathTransition.NotDead();
        yield return new WaitForSeconds(1.0f);

        transform.SendMessage("StopMovement");
        yield return new WaitForSeconds(1.0f);
        deathTransition.notSpawned();

        if (SceneManager.GetActiveScene().name == "Level4")
            SceneManager.LoadScene("Credits");
        else
            SceneManager.LoadScene("LevelSelect");

    }
    #endregion
}