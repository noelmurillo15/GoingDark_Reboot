/// <summary>
/// Going Dark Reboot
/// EnemyAlertState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyAlertState : IEnemyState {


    #region Alert State Properties
    readonly EnemyStatePattern enemy;

    float informRate = 1f;
    float nextInform;
    float offset = 0.333f;

    Vector3 targetPos;
    Vector3 lookAtTarget;

    RaycastHit hit;
    Collider[] colliders;
    Collider[] friendlyColliders;

    int detectionCount;
    int lastDetectionCount;
    Transform possileTarget;
    #endregion



    public EnemyAlertState(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
    }

    #region Ienemy Methods
    public void UpdateState()
    {		
        Look();
    }
    public void ToPatrolState()
    {
		enemy.meshRendererFlag.material.color = Color.green;
		enemy.currentState = enemy.statePatrol;
	}
    public void ToChaseState()
    {
		enemy.meshRendererFlag.material.color = Color.blue;
		enemy.myEnemyMaster.GetMoveData().boost = 1.5f;
		enemy.currentState = enemy.stateChase;
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
		if (colliders.Length == 0)
			ToPatrolState();

        foreach (Collider col in colliders)
        {
            if (col.transform == enemy.myTransform)
                continue;            

            lookAtTarget = col.transform.position;
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
			nextInform = 0f;
            detectionCount = 0;
            enemy.locationOfInterest = possileTarget;
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
                return;            

            foreach (Collider ally in friendlyColliders)
            {
                if (ally.transform.GetComponent<EnemyStatePattern>() != null)
                {
                    EnemyStatePattern allyState = ally.transform.GetComponent<EnemyStatePattern>();
					if (allyState.currentState == allyState.statePatrol)
					{
						allyState.locationOfInterest = enemy.locationOfInterest;
						allyState.currentState = allyState.stateAlert;
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
        if (enemy.locationOfInterest != null)
        {
            Vector3 dir = enemy.locationOfInterest.position - enemy.myTransform.position;
            Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dir, Time.fixedDeltaTime, 0.0f);
            enemy.myTransform.rotation = Quaternion.LookRotation(rotation);
        }
    }
    #endregion
}