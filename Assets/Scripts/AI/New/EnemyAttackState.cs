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
        enemy.meshRendererFlag.material.color = Color.red;
        Look();
        TryAttack();
    }

    public void ToPatrolState()
    {
        Debug.Log("Enemy Lasers De-activated : Patrol Mode");        
        enemy.currentState = enemy.statePatrol;
    }

    public void ToAlertState()
    {
        Debug.Log("Enemy Lasers De-activated : Alert Mode");
        enemy.currentState = enemy.stateAlert;
    }

    public void ToChaseState()
    {
        Debug.Log("Enemy Lasers De-activated : Chase Mode");
        enemy.currentState = enemy.stateChase;
    }

    public void ToAttackState() {/* not used by Attack state */ }
    #endregion

    #region Ranged Attack
    /// <summary>
    /// Npc will detect if an enemy is in Ranged attack range and turn towards target &
    /// Will go to Chase state if no enemies in Ranged attack range
    /// </summary>
    void Look()
    {
        if (enemy.MyAttackTarget == null)
        {
            ToAlertState();
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.attackRange, enemy.enemyLayers);

        if (colliders.Length == 0)
        {
            ToAlertState();
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
                enemy.myRangedWeapon.SetActive(true);
                TurnTowardsTarget();                
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
        if (enemy.MyAttackTarget == null)
        {
            ToAlertState();
            return;
        }
        
        if (TargetInRange(enemy.MyAttackTarget.position, enemy.attackRange))
        {
            Debug.Log("Enemy Lasers Active");
            enemy.myRangedWeapon.SetActive(true);
        }
        else
        {
            ToChaseState();
        }
    }
    

    /// <summary>
    /// 
    /// </summary>
    bool TargetInRange(Vector3 attackTarget, float range)
    {
        float distanceToTarget = Vector3.Distance(attackTarget, enemy.myTransform.position);
        return distanceToTarget <= enemy.attackRange;
    }

    /// <summary>
    /// 
    /// </summary>
    void TurnTowardsTarget()
    {
        enemy.myEnemyMaster.GetMoveData().IncreaseSpeed();
        Vector3 dir = enemy.locationOfInterest - new Vector3(enemy.myTransform.position.x - 100, enemy.myTransform.position.y, enemy.myTransform.position.z);
        Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dir, Time.fixedDeltaTime, 0.0f);
        enemy.myTransform.rotation = Quaternion.LookRotation(rotation);
    }
    #endregion
}