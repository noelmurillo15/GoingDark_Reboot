/// <summary>
/// Going Dark Reboot
/// EnemyPatrolState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyPatrolState : IEnemyState {


    #region Patrol Properties
    private readonly EnemyStatePattern enemy;

    //  Old    
    private bool autopilot;
    private float headingChange;
    private float headingX, headingY;
    private Vector3 autopilotlocation;
    [SerializeField] Vector3 targetRotation;

    //  New
    [SerializeField] float wanderRange = 25f;

    private int nextWaypoint;
    private float dotProduct;

    private Vector3 heading;
    private Vector3 lookAtPoint;
    private Collider[] colliders;

    private float nextCheck;
    private float checkRate = 1f;
    #endregion



    public EnemyPatrolState(EnemyStatePattern EnemyStatePattern)
    {
        enemy = EnemyStatePattern;

        autopilotlocation = Vector3.zero;
        targetRotation = Vector3.zero;
        headingChange = 45f;
        autopilot = false;

        headingY = Random.Range(1f, 359f);
        headingX = Random.Range(1f, 359f);
        enemy.myTransform.eulerAngles = new Vector3(headingX, headingY, 0);
        RandomHeadingDirection();
    }

    #region IEnemy Methods
    public void UpdateState()
    {
        enemy.meshRendererFlag.material.color = Color.green;
        Look();
        Patrol();
        RandomHeadingDirection();
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.stateAlert;
    }

    public void ToChaseState() {/* not used by Patrol state */}
    public void ToPatrolState() {/* not used by Patrol state */ }
    public void ToAttackState() {/* not used by Patrol state */ }
    #endregion

    #region Patrol
    /// <summary>
    /// Alerts enemy of any enemies in close proximity &
    /// Checks if there are any enemies in my line of sight
    /// </summary>
    void Look()
    {
        //  Checks Close Range
        colliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.detectBehindRange, enemy.enemyLayers);
        foreach (Collider col in colliders)
        {
            if (col.transform == enemy.myTransform)
            {
                continue;
            }

            CheckBehindDistance(col.transform);
        }

        //  Checks Max Sight Range
        colliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.sightRange, enemy.enemyLayers);

        foreach (Collider col in colliders)
        {
            if (col.transform == enemy.myTransform)
            {
                continue;
            }

            RaycastHit hit;

            VisibilityCalculations(col.transform);

            if (Physics.Linecast(enemy.myTransform.position, lookAtPoint, out hit, enemy.mySightLayers))
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

    /// <summary>
    /// 
    /// </summary>
    void Patrol()
    {        
        if (enemy.myFollowTarget != null)
        {
            //  TODO
            //enemy.currentState = enemy.followState;
        }

        if (autopilot)
        {
            enemy.myEnemyMaster.GetMoveData().IncreaseSpeed();
            Vector3 dir = autopilotlocation - enemy.myTransform.position;
            Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dir, Time.fixedDeltaTime, 0.0f);
            enemy.myTransform.rotation = Quaternion.LookRotation(rotation);
            headingX = enemy.myTransform.eulerAngles.x;
            headingY = enemy.myTransform.eulerAngles.y;
        }
        else
        {
            enemy.myEnemyMaster.GetMoveData().IncreaseSpeed();
            enemy.myTransform.rotation = Quaternion.Slerp(enemy.myTransform.rotation, Quaternion.Euler(targetRotation), Time.fixedDeltaTime / enemy.myEnemyMaster.GetMoveData().RotateSpeed);
        }
    }

    /// <summary>
    /// Alerts the Npc of target's last known location to investigate
    /// </summary>
    /// <param name="target"></param>
    void AlertStateActions(Transform target)
    {
        enemy.locationOfInterest = target.position;
        ToAlertState();
    }

    /// <summary>
    /// Checks if the enemy is looking at the target
    /// </summary>
    /// <param name="target"></param>
    void VisibilityCalculations(Transform target)
    {
        lookAtPoint = new Vector3(target.position.x, target.position.y + enemy.offset, target.position.z);
        heading = lookAtPoint - enemy.myTransform.position;
        dotProduct = Vector3.Dot(heading, enemy.myTransform.forward);
    }

    /// <summary>
    /// Checks if the target is really close to the enemy
    /// </summary>
    /// <param name="target"></param>
    void CheckBehindDistance(Transform target)
    {
        if (Vector3.Distance(target.position, enemy.myTransform.position) <= enemy.detectBehindRange)
        {
            AlertStateActions(target);
        }
    }

    /// <summary>
    /// Selects a random direction for the enemy to travel in
    /// </summary>
    void RandomHeadingDirection()
    {
        if (Time.time > nextCheck)
        {
            nextCheck = Time.time + Random.Range(1f, 4f);

            var floor = Mathf.Clamp(headingX - headingChange, 0, 360);
            var ceil = Mathf.Clamp(headingX + headingChange, 0, 360);
            headingX = Random.Range(floor, ceil);

            floor = Mathf.Clamp(headingY - headingChange, 0, 360);
            ceil = Mathf.Clamp(headingY + headingChange, 0, 360);
            headingY = Random.Range(floor, ceil);

            targetRotation = new Vector3(headingX, headingY, 0f);
        }
    }
    #endregion
}