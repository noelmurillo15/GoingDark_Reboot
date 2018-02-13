/// <summary>
/// Going Dark Reboot
/// EnemyMaster.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;
using GoingDark.Core.Enums;


[System.Serializable]
public class EnemyMaster : MonoBehaviour {


    #region Properties
    [Header("General")]
    [SerializeField] EnemyTypes myType;
    [SerializeField] HealthProperties myHealthData;
    [Tooltip("Droid : 5 - Final Boss : 150")]
    [SerializeField] float baseHealth = 10f;
    [SerializeField] bool isHealthCritical = false;

    [Header("Shield")]
    [SerializeField] bool hasShield;
    [SerializeField] ShieldProperties myShieldData;

    [Header("Movement")]
    [SerializeField] MovementProperties myMoveData;

    [Header("Misc")]
    [SerializeField] GameObject stunnedPrefab;
    [SerializeField] Impairments myDebuffData;
    [SerializeField] Transform myAttackTarget;

    private float multiplier;
    private float collisionTimer;
    private EnemyManager myManager;
    private EnemyStatePattern myStatePattern;
    #endregion

    #region Events
    public delegate void GeneralEventHandler();

    public event GeneralEventHandler EventEnemyDie;              
    public event GeneralEventHandler EventEnemyGetHit;            
    public event GeneralEventHandler EventEnemyRecovered;         
    public event GeneralEventHandler EventEnemyCriticalHealth;    
    public event GeneralEventHandler EventEnemyHealthRecovered;           
    #endregion



    void Awake()
    {
        stunnedPrefab.SetActive(false);
        myDebuffData = Impairments.None;
        myStatePattern = GetComponent<EnemyStatePattern>();
        LoadEnemyDifficulty();
    }    

    #region Accessors      
    public EnemyTypes GetEnemyType() { return myType; }
    public Impairments GetDebuffData() { return myDebuffData; }

    public EnemyManager GetManager() { return myManager; }
    public MovementProperties GetMoveData() { return myMoveData; }
    public HealthProperties GetHealthData() { return myHealthData; }
    public ShieldProperties GetShieldData() { return myShieldData; }
    public EnemyStatePattern GetStateManager() { return myStatePattern; }

    public float GetDifficultyMultiplier() { return multiplier; }
    public bool GetIsHealthCritical() { return isHealthCritical; }
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
        myDebuffData = Impairments.None;
        stunnedPrefab.SetActive(false);
    }
    #endregion

    #region Damage Calls 
    void EMPHit()
    {
        Debug.Log("Enemy Emp Hit");
        stunnedPrefab.SetActive(true);
        myDebuffData = Impairments.Stunned;
        Invoke("ResetDebuff", 5f);
    }

    void SplashDmg()
    {
        if (myType != EnemyTypes.FinalBoss)
        {
            Debug.Log("Enemy Splash Dmg Hit");

            if (hasShield && myShieldData.GetShieldActive())
                myShieldData.Damage(myShieldData.MaxHealth * .1f);
            else
                myHealthData.Damage(myHealthData.MaxHealth * .1f);
        }
    }

    public void CrashHit(float _speed)
    {
        if (myType != EnemyTypes.FinalBoss)
        {
            Debug.Log("Enemy Crashed");

            if (hasShield && myShieldData.GetShieldActive())
                myShieldData.Damage(_speed * myShieldData.MaxHealth * .5f);
            else
                myHealthData.Damage(_speed * myHealthData.MaxHealth * .5f);
        }
    }

    public void MissileHit(MissileProjectile missile)
    {
        Debug.Log("Enemy Missile Hit");
        if (hasShield && myShieldData.GetShieldActive())
        {
            if (myType != EnemyTypes.FinalBoss)
            {
                if (missile.Type == MissileType.ShieldBreak)
                {
                    myShieldData.Damage(100f);
                    missile.Kill();
                }
                else if (missile.Type == MissileType.Emp)
                {
                    EMPHit();
                    missile.Kill();
                }
                else
                    missile.Deflect();
            }
            else
            {
                missile.Deflect();
            }
        }
        else
        {
            myHealthData.Damage(missile.GetBaseDmg());
            missile.Kill();
        }
    }

    public void LaserDmg(LaserProjectile laser)
    {
        Debug.Log("Enemy Laser Hit");
        if (hasShield && myShieldData.GetShieldActive())
        {
            if (myType != EnemyTypes.FinalBoss)
                myShieldData.Damage(laser.GetBaseDmg());
        }
        else
            myHealthData.Damage(laser.GetBaseDmg());

        laser.Kill();
    }
    #endregion

    #region Event Methods
    public void CallEventEnemyHealthRecovered()
    {
        Debug.Log("Enemy Event Health Recovered Called");
        if (EventEnemyHealthRecovered != null)
        {
            isHealthCritical = false;
            EventEnemyHealthRecovered();
        }
    }

    public void CallEventEnemyLowHealth()
    {
        Debug.Log("Enemy Event Low Health Called");
        if (EventEnemyCriticalHealth != null)
        {
            isHealthCritical = true;
            EventEnemyCriticalHealth();
        }
    }

    public void CallEventEnemyRecovered()
    {
        Debug.Log("Enemy Event Recovered Called");
        if (EventEnemyRecovered != null)
            EventEnemyRecovered();
    }

    public void CallEventEnemyGetHit()
    {
        Debug.Log("Enemy Event Get Hit Called");
        if (EventEnemyGetHit != null)
        {
            EventEnemyGetHit();
        }
    }

    public void CallEventEnemyDie()
    {
        Debug.Log("Enemy Event Death Called");
        if (EventEnemyDie != null)
        {
            if (GetComponent<EnemyTrail>() != null)
                GetComponent<EnemyTrail>().Kill();

            EventEnemyDie();

            myManager.RemoveEnemy(this);
            Destroy(gameObject, 1f);
        }
    }
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
        if (hasShield)
            myShieldData = new ShieldProperties(transform.GetChild(0).gameObject, 100f * multiplier);

        switch (myType)
        {
            case EnemyTypes.Droid:
                myHealthData = new HealthProperties(baseHealth * multiplier, this); break;
            case EnemyTypes.JetFighter:
                myHealthData = new HealthProperties(baseHealth * multiplier, this); break;
            case EnemyTypes.Trident:
                myHealthData = new HealthProperties(baseHealth * multiplier, this); break;
            case EnemyTypes.Basic:
                myHealthData = new HealthProperties(baseHealth * multiplier, this); break;
            case EnemyTypes.SquadLead:
                myHealthData = new HealthProperties(baseHealth * multiplier, this); break;
            case EnemyTypes.Transport:
                myHealthData = new HealthProperties(baseHealth * multiplier, this); break;
            case EnemyTypes.Tank:
                myHealthData = new HealthProperties(baseHealth * multiplier, this); break;
            case EnemyTypes.FinalBoss:
                myHealthData = new HealthProperties(baseHealth * multiplier, this); break;
        }
        myManager = transform.root.GetComponent<EnemyManager>();

        //  TODO : 
        myManager.AddEnemy(this);
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
                    CallEventEnemyDie();
                }
                else
                {
                    CrashHit(GetMoveData().Speed / GetMoveData().MaxSpeed);
                }
                collisionTimer = 2f;
            }

            if (hit.transform.CompareTag("Meteor"))
            {
                collisionTimer = 5f;
                CallEventEnemyDie();
            }
        }
    }
    #endregion
}