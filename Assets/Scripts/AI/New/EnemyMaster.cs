///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;
using GoingDark.Core.Enums;


public class EnemyMaster : ShipMaster {


	#region Variables
	[Header("Enemy")]
	[SerializeField] EnemyTypes myType;
    [SerializeField] Impairments myImpairments;
    [SerializeField] float baseHealth = 10f;
    [SerializeField] bool hasShield;
    [SerializeField] GameObject stunnedPrefab;

    float multiplier;
    float collisionTimer;
    EnemyManager myManager;
    EnemyStatePattern myStatePattern;
    #endregion

    

    void Awake()
    {		
        stunnedPrefab.SetActive(false);
        myImpairments = Impairments.None;
        myStatePattern = GetComponent<EnemyStatePattern>();
        LoadEnemyDifficulty();
    }    

    #region Accessors      
    public float GetDifficultyMultiplier() { return multiplier; }

    public EnemyTypes GetEnemyType() { return myType; }
    public Impairments GetImpairmentData() { return myImpairments; }

    public EnemyManager GetManager() { return myManager; }    
    public EnemyStatePattern GetStateManager() { return myStatePattern; }
    #endregion

    #region Modifiers      
    public Transform AttackTarget
    {
        get { return myAttackTarget; }
        set { myAttackTarget = value; }
    }
    #endregion

    #region Debuffs
    void ResetDebuff()
    {
        Debug.Log("Enemy Resetting Debuffs");
        myImpairments = Impairments.None;
        stunnedPrefab.SetActive(false);
    }
    #endregion

    #region Damage Calls 
    void EMPHit()
    {
        Debug.Log("Enemy Emp Hit");
        stunnedPrefab.SetActive(true);
        myImpairments = Impairments.Stunned;
        Invoke("ResetDebuff", 5f);
    }

    void SplashDmg()
    {
        if (myType != EnemyTypes.FinalBoss)
        {
            Debug.Log("Enemy Splash Dmg Hit");
            if (hasShield && myShieldData.GetShieldActive())
                myShieldData.Damage(myShieldData.maxHealth * .1f);
            else
                myHealthData.Damage(myHealthData.maxHealth * .1f);
        }
    }

    public void CrashHit(float _speed)
    {
        if (myType != EnemyTypes.FinalBoss)
        {
            Debug.Log("Enemy Crashed");

            if (hasShield && myShieldData.GetShieldActive())
                myShieldData.Damage(_speed * myShieldData.maxHealth * .5f);
            else
                myHealthData.Damage(_speed * myHealthData.maxHealth * .5f);
        }
    }

    //public void MissileHit(MissileProjectile missile)
    //{
    //    Debug.Log("Enemy Missile Hit");
    //    if (hasShield && myShieldData.GetShieldActive())
    //    {
    //        if (myType != EnemyTypes.FinalBoss)
    //        {
    //            if (missile.Type == ProjectileType.ShieldBreakMissile)
    //            {
    //                myShieldData.Damage(100f);
    //                missile.Kill();
    //            }
    //            else if (missile.Type == ProjectileType.EmpMissile)
    //            {
    //                EMPHit();
    //                missile.Kill();
    //            }
    //            else
    //                missile.Deflect();
    //        }
    //        else
    //        {
    //            missile.Deflect();
    //        }
    //    }
    //    else
    //    {
    //        myHealthData.Damage(missile.GetBaseDmg());
    //        missile.Kill();
    //    }
    //}

    //public void LaserDmg(LaserProjectile laser)
    //{
    //    Debug.Log("Enemy Laser Hit");
    //    if (hasShield && myShieldData.GetShieldActive())
    //    {
    //        if (myType != EnemyTypes.FinalBoss)
    //            myShieldData.Damage(laser.GetBaseDmg());
    //    }
    //    else
    //        myHealthData.Damage(laser.GetBaseDmg());

    //    laser.Kill();
    //}
    #endregion

	#region Private Methods
	void LoadEnemyDifficulty()
    {
        switch (PlayerPrefs.GetString("Difficulty"))
        {
            case "Easy":
                multiplier = 1f;
                break;
            case "Medium":
                multiplier = 1.5f;
                break;
            case "Hard":
                multiplier = 2f;
                break;
            case "Nightmare":
                multiplier = 3f;
                break;
        }
        Invoke("LoadEnemyData", .5f);
    }

	void LoadEnemyData()
	{
		base.Initialize(baseHealth * multiplier, hasShield);
		//myManager = transform.root.GetComponent<EnemyManager>();
		//myManager.AddEnemy(this);
	}
    #endregion 

    #region Collision
    void OnCollisionEnter(Collision hit)
    {
        Debug.Log("Enemy Collided with : " + hit.gameObject.name);
        if (collisionTimer <= 0f)
        {
            if (hit.transform.CompareTag("Player"))
            {
                if (myType == EnemyTypes.Droid)
                {
                    hit.transform.SendMessage("EMPHit");
                    CallEventDeath();
                }
                else
                {
                    CrashHit(GetMoveData().speed / GetMoveData().maxSpeed);
                }
                collisionTimer = 2f;
            }

            if (hit.transform.CompareTag("Meteor"))
            {
                collisionTimer = 5f;
                CallEventDeath();
            }
        }
    }
    #endregion
}