/// <summary>
/// Going Dark Reboot
/// EnemyPatrolState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyPatrolState : IEnemyState {


    #region Patrol Properties
    readonly EnemyStatePattern enemy;

    //  Old    
    bool autopilot;
    float headingChange;
    float headingX, headingY;
    Vector3 autopilotlocation;
    Vector3 targetRotation;

    //  New
    int nextWaypoint;
    float dotProduct;
    float wanderRange = 25f;
    Vector3 heading;
    Vector3 lookAtPoint;
    Collider[] colliders;

    float nextCheck;
    #endregion



    public EnemyPatrolState(EnemyStatePattern EnemyStatePattern)
    {
        enemy = EnemyStatePattern;

		enemy.meshRendererFlag.material.color = Color.green;
		autopilotlocation = Vector3.zero;
        targetRotation = Vector3.zero;
        headingChange = 45f;
        autopilot = false;
		nextCheck = 0f;

		RandomHeadingDirection();
	}

	#region IEnemy Methods
	public void UpdateState()
	{
		RandomHeadingDirection();
		Look();
		Patrol();
	}

    public void ToAlertState()
    {
		enemy.meshRendererFlag.material.color = Color.yellow;
		enemy.currentState = enemy.stateAlert;
	}

    public void ToChaseState() {/* not used by Patrol state */}
    public void ToPatrolState() {/* not used by Patrol state */ }
    public void ToAttackState() {/* not used by Patrol state */ }
    #endregion

    #region Patrol
    /// <summary>
    /// Alerts the Npc of target's last known location to investigate
    /// </summary>
    /// <param name="target"></param>
    void AlertStateActions(Transform target)
    {
        enemy.locationOfInterest = target;
        ToAlertState();
    }
    /// <summary>
    /// Checks if the target is really close to the enemy
    /// </summary>
    /// <param name="target"></param>
    void CheckBehindDistance(Transform target)
    {
        if (Vector3.Distance(target.position, enemy.myTransform.position) <= enemy.detectBehindRange)
            AlertStateActions(target);        
    }
    /// <summary>
    /// Checks if the enemy is looking at the target
	/// A positive dotproduct means the enemy is facing its target
    /// </summary>
    /// <param name="target"></param>
    void VisibilityCalculations(Transform target)
    {
        lookAtPoint = target.position;
        heading = lookAtPoint - enemy.myTransform.position;
        dotProduct = Vector3.Dot(heading, enemy.myTransform.forward);
    }
    /// <summary>
    /// Selects a random direction for the enemy to travel in
    /// </summary>
    void RandomHeadingDirection()
    {
		if (Time.time > nextCheck)
		{
			nextCheck = Time.time + Random.Range(5f, 10f);

			var floor = Mathf.Clamp(headingX - headingChange, 0, 360);
			var ceil = Mathf.Clamp(headingX + headingChange, 0, 360);
			headingX = Random.Range(floor, ceil);

			floor = Mathf.Clamp(headingY - headingChange, 0, 360);
			ceil = Mathf.Clamp(headingY + headingChange, 0, 360);
			headingY = Random.Range(floor, ceil);

			targetRotation = new Vector3(headingX, headingY, 0f);
		}
    }
    /// <summary>
    /// 
    /// </summary>
    void Patrol()
    {        
        if (enemy.myFollowTarget != null)
        {
			//  TODO : follow
			return;
        }

        if (autopilot)
        {
            Vector3 dir = autopilotlocation - enemy.myTransform.position;
            Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dir, Time.fixedDeltaTime, 0.0f);
            enemy.myTransform.rotation = Quaternion.LookRotation(rotation);
        }
        else
            enemy.myTransform.rotation = Quaternion.Slerp(enemy.myTransform.rotation, Quaternion.Euler(targetRotation), Time.fixedDeltaTime);
    }
    /// <summary>
    /// Alerts enemy of any enemies in close proximity &
    /// Checks if there are any enemies in my line of sight
    /// </summary>
    void Look()
    {
        //  Checks Close Range
        colliders = Physics.OverlapSphere(enemy.myTransform.position,
						enemy.detectBehindRange, enemy.enemyLayers);
        foreach (Collider col in colliders)
        {
            if (col.transform == enemy.myTransform)
                continue;
			
            CheckBehindDistance(col.transform);
        }

        //  Checks Max Sight Range
        colliders = Physics.OverlapSphere(enemy.myTransform.position,
						enemy.sightRange, enemy.enemyLayers);
        foreach (Collider col in colliders)
        {
            if (col.transform == enemy.myTransform)
                continue;            

            RaycastHit hit;
            VisibilityCalculations(col.transform);
            if (Physics.Linecast(enemy.myTransform.position, lookAtPoint, out hit, enemy.enemyLayers))
            {
                foreach (string tags in enemy.enemyTags)
                {
                    if (hit.transform.CompareTag(tags))
                    {
                        if (dotProduct > 0)
                        {
                            AlertStateActions(col.transform);
                            return;
                        }
                    }
                }
            }
        }
    }
    #endregion
}