/// <summary>
/// Going Dark Reboot
/// EnemyAttackState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyAttackState : IEnemyState {


    #region Range Attack Properties
    private readonly EnemyStatePattern enemy;
    private RaycastHit hit;
    #endregion    



    public EnemyAttackState(EnemyStatePattern npcStatePattern)
    {
        enemy = npcStatePattern;
    }

    #region INpc Methods
    public void UpdateState()
    {
		if(enemy.MyAttackTarget != null)
		{
			enemy.meshRendererFlag.material.color = Color.red;
			Look();
			TryAttack();
		}
		else
		{
			ToAlertState();
		}
    }

    public void ToPatrolState()
    {
        Debug.Log("Attack -> Patrol");        
        enemy.currentState = enemy.statePatrol;
		return;
    }

    public void ToAlertState()
    {
        Debug.Log("Attack -> Alert");
        enemy.currentState = enemy.stateAlert;
		return;
    }

    public void ToChaseState()
    {
        Debug.Log("Attack -> Chase");
        enemy.currentState = enemy.stateChase;
		return;
    }

    public void ToAttackState() {/* not used by Attack state */ }
	#endregion

	#region Attack
	/// <summary>
	/// 
	/// </summary>
	bool TargetInRange(Vector3 attackTarget, float range)
	{
		float distanceToTarget = Vector3.Distance(attackTarget, enemy.myTransform.position);
		return distanceToTarget <= range;
	}
	/// <summary>
	/// 
	/// </summary>
	void TurnTowardsTarget()
	{
		//enemy.myEnemyMaster.GetMoveData().IncreaseSpeed();
		Vector3 dir = enemy.locationOfInterest - new Vector3(enemy.myTransform.position.x - 250, enemy.myTransform.position.y, enemy.myTransform.position.z);
		Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dir, Time.fixedDeltaTime * enemy.myEnemyMaster.GetMoveData().rotateSpeed, 0.0f);
		enemy.myTransform.rotation = Quaternion.LookRotation(rotation);
	}
	/// <summary>
	/// Npc will detect if an enemy is in Ranged attack range and turn towards target &
	/// Will go to Chase state if no enemies in Ranged attack range
	/// </summary>
	void Look()
    {
        Collider[] colliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.attackRange, enemy.enemyLayers);
        if (colliders.Length == 0)
        {
			if (enemy.MyAttackTarget == null)
				ToAlertState();
			else
				ToChaseState();

            return;
        }

        foreach (Collider col in colliders)
        {
            if (col.transform == enemy.myTransform)
            {
                continue;
            }
            if (col.transform == enemy.MyAttackTarget)
            {
				if(TargetInRange(enemy.MyAttackTarget.position, enemy.attackRange))
				{
					enemy.myRangedWeapon.SetActive(true);
					TurnTowardsTarget();                
				}
                return;
            }
        }
    }
    /// <summary>
    /// Npc will detect if an enemy is in Ranged Attack range &
    /// will turn towards attack target
    /// </summary>
    void TryAttack()
    {     
        if (TargetInRange(enemy.MyAttackTarget.position, enemy.attackRange))
        {
            enemy.myRangedWeapon.SetActive(true);
        }
        else
        {
            ToChaseState();
        }
    }       
    #endregion
}