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
	[Space]
    [Header("Detection")]
    public float sightRange = 2000f;
    public float attackRange = 1200f;
    public float detectBehindRange = 650f;
    public float fleeRange = 2500f;
	[Space]
    public int requiredDetectionCount = 100;

	[Header("Layers")]
    public LayerMask enemyLayers;
    public string[] enemyTags;
	[Space]
    public LayerMask friendlyLayers;
    public string[] friendlyTags;  
	
	[Header("Follow")]
    public Transform myFollowTarget;

    [Header("Debugging")]
    public MeshRenderer meshRendererFlag;

    //  References
    [HideInInspector] public Transform myTransform;
    [HideInInspector] public Rigidbody myRigidbody;

    //  Targets & Wander Pts
    [HideInInspector] public Transform myAttacker;
    [HideInInspector] public Transform locationOfInterest;

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
    #endregion



	#region Unity Functions
    void Awake()
    {
        Initialize();
        myEnemyMaster.EventCriticalHealth += ActivateFleeState;
        myEnemyMaster.EventHealthRecovered += ActivatePatrolState;
        myEnemyMaster.EventOnHit += ActivateGetHitState;
    }
	
    void OnDisable()
    {
        myEnemyMaster.EventCriticalHealth -= ActivateFleeState;
        myEnemyMaster.EventHealthRecovered -= ActivatePatrolState;
        myEnemyMaster.EventOnHit -= ActivateGetHitState;
        StopAllCoroutines();
    }

	//void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.white;
	//	Gizmos.DrawWireSphere(transform.position, sightRange);
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawWireSphere(transform.position, attackRange);
	//	Gizmos.color = Color.yellow;
	//	Gizmos.DrawWireSphere(transform.position, detectBehindRange);
	//}

	void FixedUpdate()
    {
        currentState.UpdateState();
        myRigidbody.MovePosition(myTransform.position + myTransform.forward * 
			Time.fixedDeltaTime * myEnemyMaster.GetMoveData().speed);
    }
	#endregion

	#region Initialization
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
		stateGetHit = new EnemyGetHitState(this);
		stateInvestigate = new EnemyInvestigateState(this);
		fleeState = new EnemyFleeState(this);
		//followState = new EnemyFollowState(this);
	}
	#endregion

	#region Enemy State Pattern
	void ActivateFleeState()
    {
        if (currentState == stateGetHit)
        {
            capturedState = fleeState;
            return;
        }
        currentState = fleeState;
    }

    void ActivatePatrolState()
    {
		myEnemyMaster.GetMoveData().boost = 1f;
        currentState = statePatrol;
		myEnemyMaster.CallEventSetAttackTargetCoordinates(null);
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

        myEnemyMaster.CallEventHealthRecovered();
        currentState = capturedState;
    }

    public void Distract(Transform distractionPos)
    {
        locationOfInterest = distractionPos;
        if (currentState == statePatrol)
        {
            currentState = stateAlert;
        }
    }
    #endregion
}