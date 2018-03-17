/// <summary>
/// Going Dark Reboot
/// EnemyAttackState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyAttackState : IEnemyState {


    #region Range Attack Properties
    readonly EnemyStatePattern enemy;
    RaycastHit hit;
	float offsetX;
	float offsetY;
	Transform myAttackTarget;
	#endregion



	public EnemyAttackState(EnemyStatePattern npcStatePattern)
    {
        enemy = npcStatePattern;
		offsetX = Random.Range(-500f,500f);
		offsetY = Random.Range(-500f,500f);
	}

    #region INpc Methods
    public void UpdateState()
    {
		if(enemy.locationOfInterest != null)
			Look();					
		else
			ToAlertState();		
    }

    public void ToAlertState()
    {
		Debug.Log("Attack -> Alert");
		enemy.meshRendererFlag.material.color = Color.yellow;
		enemy.myEnemyMaster.GetMoveData().boost = 1f;
		enemy.currentState = enemy.stateAlert;
    }

    public void ToChaseState()
    {
		Debug.Log("Attack -> Chase");
		//if (enemy.myLaserWeapon != null)
		//	enemy.myLaserWeapon.LockOn(null);

		//if (enemy.myMissileWeapon != null)
		//	enemy.myMissileWeapon.LockOn(null);

		enemy.meshRendererFlag.material.color = Color.blue;
		enemy.myEnemyMaster.GetMoveData().boost = 1.5f;
		enemy.currentState = enemy.stateChase;
    }
	public void ToPatrolState() {/* not used by Attack state */ }
    public void ToAttackState() {/* not used by Attack state */ }
	#endregion

	#region Attack
	bool TargetInRange(Vector3 attackTarget)
	{
		float distanceToTarget = Vector3.Distance(attackTarget, enemy.myTransform.position);
		return distanceToTarget <= enemy.attackRange;
	}
	void TurnTowardsTarget()
	{
		Vector3 dir = enemy.locationOfInterest.position - new Vector3(
			enemy.myTransform.position.x + offsetX, enemy.myTransform.position.y + offsetY,
			enemy.myTransform.position.z);
		Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dir, Time.fixedDeltaTime * enemy.myEnemyMaster.GetMoveData().rotateSpeed, 0.0f);
		enemy.myTransform.rotation = Quaternion.LookRotation(rotation);
	}
	/// <summary>
	/// Npc will detect if an enemy is in Ranged Attack range &
	/// will turn towards attack target
	/// </summary>
	void TryAttack()
	{		
		if (enemy.locationOfInterest != null)
		{
			if (TargetInRange(enemy.locationOfInterest.position))
			{
				//if(enemy.myMissileWeapon != null)
				//	enemy.myMissileWeapon.LockOn(enemy.myEnemyMaster.GetAttackTarget());
				//if (enemy.myLaserWeapon != null)
				//	enemy.myLaserWeapon.LockOn(enemy.myEnemyMaster.GetAttackTarget());
			}
		}
	}
	/// <summary>
	/// Npc will detect if an enemy is in Ranged attack range and turn towards target &
	/// Will go to Chase state if no enemies in Ranged attack range
	/// </summary>
	void Look()
    {
        Collider[] colliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.attackRange, enemy.enemyLayers);
        if (colliders.Length == 0)
			ToChaseState();
        
        foreach (Collider col in colliders)
        {
            if (col.transform == enemy.myTransform)
            {
                continue;
            }
            else if (col.transform == enemy.locationOfInterest)
            {
				TurnTowardsTarget();                
				TryAttack();
            }
        }
    }       
    #endregion
}