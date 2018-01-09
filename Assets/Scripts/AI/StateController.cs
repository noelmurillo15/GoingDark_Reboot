using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StateController : MonoBehaviour {

    public State currentState;
    public Transform enemyShip;

    public IEnemy enemyStats;


	void Awake () {
		
	}

    public void SetupAi() {

    }
	
	void Update () {
        //  TODO : 
        //if (!aiActive)
        //    return;

        currentState.UpdateState(this);
	}

    void OnDrawGizmos() {
        if(currentState != null /*&& Target != null*/)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(enemyShip.position, 20f);
        }
    }
}