﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/State")]
public class State : ScriptableObject {


    public ActionAi[] actions;
    public Color sceneGizmoColor;

    public void UpdateState(StateController controller) {
        DoAction(controller);
    }

    private void DoAction(StateController controller) {
        for(int i = 0; i < actions.Length; i++) {
            actions[i].Act(controller);
        }
    }
}