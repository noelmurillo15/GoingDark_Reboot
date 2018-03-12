/// <summary>
/// Going Dark Reboot
/// EnemyChaseState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyChaseState : IEnemyState {


    #region Chase Properties
    private readonly EnemyStatePattern enemy;
    private float capturedDistance;
    #endregion



    public EnemyChaseState(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
    }

    #region Ienemy Methods
    public void UpdateState()
    {
		if (enemy.MyAttackTarget != null)
		{
			enemy.meshRendererFlag.material.color = Color.blue;
			Look();
			Pursue();
		}
		else
		{
			ToAlertState();
		}
    }

    public void ToPatrolState()
    {
		Debug.Log("Chase -> Patrol");
		enemy.currentState = enemy.statePatrol;
		return;
	}

	public void ToAlertState()
    {
		Debug.Log("Chase -> Alert");
		enemy.currentState = enemy.stateAlert;
		return;
	}

    public void ToAttackState()
    {
		Debug.Log("Chase -> Attack");
		enemy.currentState = enemy.stateAttack;
		return;
	}

    public void ToChaseState() {/* not used by Chase state */ }
    #endregion

    #region Chase
    /// <summary>
    /// Selects the closest enemy to go after
    /// </summary>
    void Look()
    {
        if (enemy.MyAttackTarget == null)
        {
            ToAlertState();
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.sightRange, enemy.enemyLayers);
        if (colliders.Length == 0)
        {
			ToAlertState();
            return;
        }

        capturedDistance = enemy.sightRange * 1.2f;
        foreach (Collider col in colliders)
        {
            if (col.transform == enemy.myTransform)
            {
                continue;
            }

            float distanceToTarget = Vector3.Distance(enemy.transform.position, col.transform.position);

            if (distanceToTarget < capturedDistance)
            {
                capturedDistance = distanceToTarget;
                enemy.MyAttackTarget = col.transform;
            }
        }
    }

    /// <summary>
    /// enemy will follow it's attack target
    /// and switch to an attack state depending on distance
    /// </summary>
    void Pursue()
    {
        if (enemy.MyAttackTarget != null)
        {
            enemy.locationOfInterest = enemy.MyAttackTarget.position;

            enemy.myEnemyMaster.GetMoveData().IncreaseSpeed();
            Vector3 dir = enemy.locationOfInterest - new Vector3(enemy.myTransform.position.x, enemy.myTransform.position.y, enemy.myTransform.position.z);
            Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dir, Time.fixedDeltaTime * enemy.myEnemyMaster.GetMoveData().rotateSpeed, 0.0f);
            enemy.myTransform.rotation = Quaternion.LookRotation(rotation);

            float distanceToTarget = Vector3.Distance(enemy.transform.position, enemy.locationOfInterest);
            if (distanceToTarget <= enemy.attackRange)
            {
                ToAttackState();
            }
        }
        else
        {
            ToAlertState();
        }
    }
    #endregion
}