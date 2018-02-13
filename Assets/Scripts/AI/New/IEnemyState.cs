/// <summary>
/// Going Dark Reboot
/// IEnemyState.cs
/// 2/4/2018
/// By: Allan Murillo
/// </summary>


public interface IEnemyState {


    void UpdateState();
    void ToPatrolState();
    void ToAlertState();
    void ToChaseState();
    void ToAttackState();
}