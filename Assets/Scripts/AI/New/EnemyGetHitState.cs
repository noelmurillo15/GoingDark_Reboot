/// <summary>
/// Going Dark Reboot
/// EnemyGetHitState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>
using UnityEngine;


public class EnemyGetHitState : IEnemyState {


    #region Get Hit Properties
    private readonly EnemyStatePattern enemy;

    private float informRate = .5f;
    private float nextInform;
    private Collider[] colliders;
    private Collider[] friendlyColliders;
    #endregion



    public EnemyGetHitState(EnemyStatePattern npcStatePattern)
    {
        enemy = npcStatePattern;
    }

    #region INpc Methods
    public void UpdateState()
    {
        enemy.meshRendererFlag.material.color = Color.black;
        InformAllies();
    }

    public void ToPatrolState() {/* not used by GetHit state */ }
    public void ToAlertState() {/* not used by GetHit state */ }
    public void ToChaseState() {/* not used by GetHit state */ }
    public void ToAttackState() {/* not used by GetHit state */ }
    #endregion

    #region Get Hit
    /// <summary>
    /// Informs allies of useful Information about the attacker
    /// </summary>
    void InformAllies()
    {       
        if (Time.time > nextInform)
        {
            nextInform = Time.time + informRate;
        }
        else
        {
            return;
        }

        if (enemy.myAttacker != null)
        {
            friendlyColliders = Physics.OverlapSphere(enemy.myTransform.position, enemy.sightRange, enemy.friendlyLayers);

            if (CheckAttackerDistance())
            {
                AlertAllies();
                SetMyselfToInvestigate();
            }
        }
    }

    /// <summary>
    /// Informs nearby allies of the attacker's last known location
    /// </summary>
    void AlertAllies()
    {
        foreach (Collider ally in friendlyColliders)
        {
            if (ally.transform.GetComponent<EnemyStatePattern>() != null)
            {
                EnemyStatePattern allyState = ally.transform.GetComponent<EnemyStatePattern>();
                if (allyState.currentState == allyState.statePatrol)
                {
                    allyState.MyAttackTarget = enemy.myAttacker;
                    allyState.locationOfInterest = enemy.myAttacker.position;
                    allyState.currentState = allyState.stateInvestigate;
                }
            }
        }
    }

    /// <summary>
    /// Switches to Investigate state if Npc was currently on Patrol
    /// </summary>
    void SetMyselfToInvestigate()
    {
        if (enemy.capturedState == enemy.statePatrol)
        {
            enemy.MyAttackTarget = enemy.myAttacker;
            enemy.locationOfInterest = enemy.myAttacker.position;
            enemy.capturedState = enemy.stateInvestigate;
        }
    }

    /// <summary>
    /// Is Npc's attacker within my max sight range
    /// </summary>
    bool CheckAttackerDistance()
    {
        if (Vector3.Distance(enemy.myTransform.position, enemy.myAttacker.position) <= enemy.sightRange * 1.25f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}