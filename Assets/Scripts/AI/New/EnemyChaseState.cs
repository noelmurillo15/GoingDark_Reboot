/// <summary>
/// Going Dark Reboot
/// EnemyChaseState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyChaseState : IEnemyState {


    #region Chase Properties
    readonly EnemyStatePattern enemy;

	float nextCheck;
	float offsetX;
	float offsetY;
	#endregion



	public EnemyChaseState(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
		nextCheck = 0f;
		offsetX = Random.Range(-500f,500f);
		offsetY = Random.Range(-500f, 500f);
	}

    #region Ienemy Methods
    public void UpdateState()
    {
		if (enemy.locationOfInterest != null)
			Look();		
		else
			ToAlertState();		
    }

	public void ToAlertState()
    {
		enemy.myEnemyMaster.CallEventSetAttackTargetCoordinates(null);
		enemy.meshRendererFlag.material.color = Color.yellow;
		enemy.myEnemyMaster.GetMoveData().boost = 1f;
		enemy.currentState = enemy.stateAlert;
	}

    public void ToPatrolState() {/* not used by Chase state */}
    public void ToAttackState() {/* not used by Chase state */}
	public void ToChaseState() {/* not used by Chase state */}
	#endregion

	#region Chase
	/// <summary>
	/// Selects the closest enemy to go after
	/// </summary>
	void Look()
	{
		Collider[] colliders = Physics.OverlapSphere(
			enemy.myTransform.position, enemy.sightRange * 1.25f, enemy.enemyLayers);

		if (colliders.Length == 0)
			ToAlertState();					

		Pursue();
	}	
	/// <summary>
	/// enemy will follow it's attack target
	/// and switch to an attack state depending on distance
	/// </summary>
	void Pursue()
	{
		TurnTowardsTarget();
		if (Time.time > nextCheck)
		{
			nextCheck = Time.time + Random.Range(1f, 3f);
			float distanceToTarget = Vector3.Distance(
				enemy.transform.position, enemy.locationOfInterest.position);

			if (distanceToTarget <= enemy.attackRange)
				enemy.myEnemyMaster.CallEventSetAttackTargetCoordinates(enemy.locationOfInterest);
			else
				enemy.myEnemyMaster.CallEventSetAttackTargetCoordinates(null);
		}
	}
	/// <summary>
	/// 
	/// </summary>
	void TurnTowardsTarget()
	{
		Vector3 dir = enemy.locationOfInterest.position - new Vector3(
			enemy.myTransform.position.x + offsetX,
			enemy.myTransform.position.y + offsetY,
			enemy.myTransform.position.z);

		Vector3 rotation = Vector3.RotateTowards(
			enemy.myTransform.forward, dir,
			Time.fixedDeltaTime * enemy.myEnemyMaster.GetMoveData().rotateSpeed, 0.0f);

		enemy.myTransform.rotation = Quaternion.LookRotation(rotation);
	}
	#endregion
}