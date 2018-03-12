/// <summary>
/// Going Dark Reboot
/// EnemyStatePattern.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;
using System.Collections;


public class EnemyStatePattern : MonoBehaviour {


    #region Decision Making Properties
    [Header("Dependancies")]
    public EnemyMaster myEnemyMaster;

    [Header("Attack")]
    public float attackRange = 500f;
    public GameObject myRangedWeapon;
    [SerializeField] bool isAttacking;

    [Header("Detection")]
    public Transform myFollowTarget;
	[Space]
    public float sightRange = 600f;
    public float fleeRange = 800f;
    public float detectBehindRange = 250f;
	[Space]
    public float offset = 0.5f;
    public int requiredDetectionCount = 10;   

    [Header("Layer")]
    public LayerMask enemyLayers;
    public string[] enemyTags;
	[Space]
    public LayerMask friendlyLayers;
    public string[] friendlyTags;    

    [Header("Debugging")]
    public MeshRenderer meshRendererFlag;

    //  References
    [HideInInspector] public Transform myTransform;
    [HideInInspector] public Rigidbody myRigidbody;

    //  Targets & Wander Pts
    [HideInInspector] public Transform myAttacker;
    [HideInInspector] public Vector3 locationOfInterest;

    //  State AI    
    public IEnemyState currentState;
    public IEnemyState capturedState;
    public EnemyPatrolState statePatrol;
    public EnemyAlertState stateAlert;
    public EnemyChaseState stateChase;
    public EnemyAttackState stateAttack;
    public EnemyGetHitState stateGetHit;
    public EnemyInvestigateState stateInvestigate;
    public EnemyFleeState fleeState;
    //public EnemyFollowState followState;
    #endregion



    void Awake()
    {
        Initialize();
        myEnemyMaster.EventCriticalHealth += ActivateFleeState;
        myEnemyMaster.EventHealthRecovered += ActivatePatrolState;
        myEnemyMaster.EventOnHit += ActivateGetHitState;

		if(gameObject.GetComponentInChildren<LaserSystem>() != null)
		{
			Debug.Log("Enemy laser system attached");
		}
		else if(gameObject.GetComponentInChildren<MissileSystem>() != null)
		{
			Debug.Log("Enemy missile system attached");
		}
		else
		{
			Debug.LogError("Something is missing...");
		}
    }

    void Initialize()
    {
        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody>();
        SetupStateRefs();
        ActivatePatrolState();
    }

    void SetupStateRefs()
    {
        statePatrol = new EnemyPatrolState(this);
        stateAlert = new EnemyAlertState(this);
        stateChase = new EnemyChaseState(this);
        stateAttack = new EnemyAttackState(this);
        stateGetHit = new EnemyGetHitState(this);
        stateInvestigate = new EnemyInvestigateState(this);
        fleeState = new EnemyFleeState(this);
        //followState = new EnemyFollowState(this);
    }

    void OnDisable()
    {
        myEnemyMaster.EventCriticalHealth -= ActivateFleeState;
        myEnemyMaster.EventHealthRecovered -= ActivatePatrolState;
        myEnemyMaster.EventOnHit -= ActivateGetHitState;
        StopAllCoroutines();
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, sightRange);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, detectBehindRange);
	}

	void FixedUpdate()
    {
        currentState.UpdateState();
        myRigidbody.MovePosition(myTransform.position + myTransform.forward * Time.fixedDeltaTime * myEnemyMaster.GetMoveData().speed);
    }

    #region Accessors
    public Transform MyAttackTarget
    {
        get { return myEnemyMaster.AttackTarget; }
        set { myEnemyMaster.AttackTarget = value; }
    }
    #endregion

    #region Enemy State Pattern
    void ActivateFleeState()
    {
        //myRangedWeapon.SetActive(false);
        if (currentState == stateGetHit)
        {
            capturedState = fleeState;
            return;
        }
        currentState = fleeState;
    }

    void ActivatePatrolState()
    {
        currentState = statePatrol;
        //myRangedWeapon.SetActive(false);
    }

    void ActivateGetHitState()
    {
        StopAllCoroutines();

        if (currentState != stateGetHit)
        {
            capturedState = currentState;
        }

        currentState = stateGetHit;
        StartCoroutine(RecoverFromGetHit());
    }

    IEnumerator RecoverFromGetHit()
    {
        meshRendererFlag.material.color = Color.grey;
        yield return new WaitForSeconds(.1f);

        isAttacking = false;
        myEnemyMaster.CallEventHealthRecovered();
        currentState = capturedState;
    }

    public void Distract(Vector3 distractionPos)
    {
        locationOfInterest = distractionPos;

        if (currentState == statePatrol)
        {
            currentState = stateAlert;
        }
    }
    #endregion
}