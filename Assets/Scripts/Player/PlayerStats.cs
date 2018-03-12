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
		EventDeath += Kill;
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
		//transform.root.GetComponent<EnemyManager>().AllEnemiesPatrol();
		//transform.root.GetComponent<GameOver>().InitializeGameOverScene();
		Debug.Log("GameOver!");
    }
    public void Kill()
    {
		EventDeath -= Kill;
		Debug.Log("Player Kill called");
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