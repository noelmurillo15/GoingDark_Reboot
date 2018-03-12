/// <summary>
/// Going Dark Reboot
/// EnemyAlertState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyAlertState : IEnemyState {


    #region Alert State Properties
    private readonly EnemyStatePattern enemy;

    private float informRate = 1f;
    private float nextInform;
    private float offset = 0.333f;

    private Vector3 targetPos;
    private Vector3 lookAtTarget;

    private RaycastHit hit;
    private Collider[] colliders;
    private Collider[] friendlyColliders;

    private int detectionCount;
    private int lastDetectionCount;
    private Transform possileTarget;
    #endregion



    public EnemyAlertState(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
    }

    #region Ienemy Methods
    public void UpdateState()
    {
        enemy.meshRendererFlag.material.color = Color.yellow;
        Look();
    }

    public void ToPatrolState()
    {
		Debug.Log("Alert -> Patrol");
		enemy.currentState = enemy.statePatrol;
		return;
	}

    public void ToChaseState()
    {
		Debug.Log("Alert -> Chase");
		enemy.currentState = enemy.stateChase;
		return;
	}

    public void ToAlertState() {/* not used by Alert state */}
    public void ToAttackState() {/* not used by Alert state */ }
    #endregion

    #region Alert
    /// <summary>
    /// 
    /// </summary>
    void Look()
    {        
        //  Check Max Sight Range
        lastDetectionCount = detectionCount;
        colliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.sightRange, enemy.enemyLayers);
        foreach (Collider col in colliders)
        {
            if (col.transform == enemy.myTransform)
            {
                continue;
            }

            lookAtTarget = new Vector3(col.transform.position.x, col.transform.position.y + offset, col.transform.position.z);

            if (Physics.Linecast(enemy.myTransform.position, lookAtTarget, out hit, enemy.enemyLayers))
            {
                foreach (string tags in enemy.enemyTags)
                {
                    if (hit.transform.CompareTag(tags))
                    {
                        detectionCount++;
                        possileTarget = col.transform;
                        break;
                    }
                }
            }
        }

        if (detectionCount == lastDetectionCount)
        {
            detectionCount = 0;
        }

        if (detectionCount >= enemy.requiredDetectionCount)
        {
            detectionCount = 0;
            enemy.locationOfInterest = possileTarget.position;
            enemy.myEnemyMaster.AttackTarget = possileTarget;
            InformNearbyAllies();
            ToChaseState();
        }
        GoToLocationOfInterest();
    }
    /// <summary>
    /// 
    /// </summary>
    void InformNearbyAllies()
    {
        if (Time.time > nextInform)
        {
            nextInform = Time.time + informRate;

            friendlyColliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.sightRange, enemy.friendlyLayers);
            if (friendlyColliders.Length == 0)
            {
                return;
            }

            foreach (Collider ally in friendlyColliders)
            {
                if (ally.transform.GetComponent<EnemyStatePattern>() != null)
                {
                    EnemyStatePattern allyState = ally.transform.GetComponent<EnemyStatePattern>();

                    if (allyState.currentState == allyState.statePatrol)
                    {
                        if (enemy.myEnemyMaster.AttackTarget != enemy.myTransform)
                        {
                            allyState.myEnemyMaster.AttackTarget = enemy.myEnemyMaster.AttackTarget;
                            allyState.locationOfInterest = allyState.myEnemyMaster.AttackTarget.position;
                            allyState.currentState = allyState.stateAlert;
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void GoToLocationOfInterest()
    {
        if (enemy.locationOfInterest != Vector3.zero)
        {
            enemy.myEnemyMaster.GetMoveData().IncreaseSpeed();
            Vector3 dir = enemy.locationOfInterest - new Vector3(enemy.myTransform.position.x - 100, enemy.myTransform.position.y, enemy.myTransform.position.z);
            Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dir, Time.fixedDeltaTime, 0.0f);
            enemy.myTransform.rotation = Quaternion.LookRotation(rotation);

            //  TODO :
            //  Go to patrol state if you arrive at location of interest and find nothing
        }
    }
    #endregion
}