/// <summary>
/// Going Dark Reboot
/// EnemyInvestigateState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyInvestigateState : IEnemyState {
	

    #region Investigate Properties
    private readonly EnemyStatePattern enemy;
    private RaycastHit hit;
    private Vector3 lookAtTarget;
    #endregion



    public EnemyInvestigateState(EnemyStatePattern npcStatePattern)
    {
        enemy = npcStatePattern;
    }

    #region INpc Methods
    public void UpdateState()
    {
		enemy.meshRendererFlag.material.color = Color.black;
		Look();
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.statePatrol;
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.stateAlert;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.stateChase;
    }

    public void ToAttackState() {/* not used by Investigate state */ }
    #endregion

    #region Investigate
    /// <summary>
    /// If Npc was attacked, check if there are enemies in sight
    /// </summary>
    void Look()
    {
        if (enemy.locationOfInterest == null)
        {
            ToPatrolState();
            return;
        }
        CheckTargetInSight();
    }

    /// <summary>
    /// Looks in the direction of the last known location for any enemies
    /// </summary>
    void CheckTargetInSight()
    {
        lookAtTarget = enemy.locationOfInterest.position;
        if (Physics.Linecast(enemy.myTransform.position, lookAtTarget, out hit, enemy.enemyLayers))
        {
            if (hit.transform == enemy.locationOfInterest && hit.transform != enemy.myTransform)
            {
                enemy.locationOfInterest = hit.transform;
                GoToLocationOfInterest();

                if (Vector3.Distance(enemy.myTransform.position, lookAtTarget) <= enemy.sightRange)
                {
                    ToChaseState();
                }
            }
            else
            {
                ToAlertState();
            }
        }
        else
        {
            ToAlertState();
        }
    }

	/// <summary>
	/// Npc will move toward Location of interest
	/// </summary>
	void GoToLocationOfInterest()
	{
		enemy.myEnemyMaster.GetMoveData().IncreaseSpeed();
		Vector3 dir = enemy.locationOfInterest.position - enemy.myTransform.position;
		Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dir, Time.fixedDeltaTime, 0.0f);
		enemy.myTransform.rotation = Quaternion.LookRotation(rotation);

		//  TODO :
		//  Go to patrol state if you arrive at location of interest and find nothing
	}
    #endregion
}