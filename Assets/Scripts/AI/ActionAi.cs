using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ActionAi : ScriptableObject {


    public abstract void Act(StateController controller);
}
