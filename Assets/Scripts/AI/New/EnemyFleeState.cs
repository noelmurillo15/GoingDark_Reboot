/// <summary>
/// FPS Core Project Created By: Allan Murillo
/// Npc State Machine Behaviour
/// </summary>
using UnityEngine;
using UnityEngine.AI;


public class EnemyFleeState : IEnemyState
{



    #region Flee Properties
    private readonly EnemyStatePattern enemy;

    private Vector3 dirToEnemy;
    private NavMeshHit navhit;
    #endregion



    public EnemyFleeState(EnemyStatePattern npcStatePattern)
    {
        enemy = npcStatePattern;
    }

    #region INpc Methods
    public void UpdateState()
    {
        enemy.meshRendererFlag.material.color = Color.white;
        Flee();
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.statePatrol;
    }


    public void ToAlertState() {/* not used by Flee state */ }
    public void ToChaseState() {/* not used by Flee state */ }
    public void ToAttackState() {/* not used by Flee state */ }
    #endregion



    #region Flee
    /// <summary>
    /// Npc will move in the direction away from the enemy
    /// </summary>
    void Flee()
    {
        Collider[] colliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.sightRange, enemy.enemyLayers);

        if (colliders.Length == 0)
        {
            ToPatrolState();
            return;
        }

        enemy.myEnemyMaster.GetMoveData().IncreaseSpeed();
        dirToEnemy = enemy.myTransform.position - colliders[0].transform.position;
        Vector3 rotation = Vector3.RotateTowards(enemy.myTransform.forward, dirToEnemy, Time.fixedDeltaTime, 0.0f);
        enemy.myTransform.rotation = Quaternion.LookRotation(rotation);
    }
    #endregion
}