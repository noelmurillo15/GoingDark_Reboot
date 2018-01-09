using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAi/Actions/Patrol")]
public class PatrolAction : ActionAi {

    public override void Act(StateController controller)
    {
        throw new System.NotImplementedException();
    }

    private void Patrol(StateController controller)
    {
        
        //if (autopilot)
        //{
        //    MoveData.IncreaseSpeed();
        //    Vector3 dir = autopilotlocation - MyTransform.position;
        //    Vector3 rotation = Vector3.RotateTowards(MyTransform.forward, dir, Time.fixedDeltaTime, 0.0f);
        //    MyTransform.rotation = Quaternion.LookRotation(rotation);
        //    headingX = MyTransform.eulerAngles.x;
        //    headingY = MyTransform.eulerAngles.y;
        //}
        //else
        //{
        //    MoveData.IncreaseSpeed();
        //    MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(targetRotation), Time.fixedDeltaTime / MoveData.RotateSpeed);
        //}
    }
}
