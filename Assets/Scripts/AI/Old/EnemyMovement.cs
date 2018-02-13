using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    #region Properties
    [SerializeField] MovementProperties MoveData;
    [SerializeField] Vector3 targetRotation;

    private bool autopilot;
    private float headingChange;
    private float headingX, headingY;
    private Vector3 autopilotlocation;

    private EnemyMaster stats;
    private EnemyStatePattern stateManager;    

    private Transform MyTransform;
    private Rigidbody MyRigidbody;
    #endregion



    //#region Accessors
    //public MovementProperties GetMoveData()
    //{
    //    return MoveData;
    //}
    //#endregion

    //#region Modifiers
    //void InBounds()
    //{
    //    autopilot = false;
    //}
    //void OutOfBounds(Vector3 _loc)
    //{
    //    autopilot = true;
    //    autopilotlocation = _loc;
    //}
    //public void StopMovement()
    //{
    //    MoveData.Speed = 0f;
    //}
    //public void SetSpeedBoost(float newBoost)
    //{
    //    MoveData.Boost = newBoost;
    //}    
    //public void LoadEnemyData(float mult)
    //{
    //    mult *= .5f;
    //    switch (stats.GetEnemyType())
    //    {
    //        case EnemyTypes.Droid:
    //            MoveData.Set(0f, .5f, 125f * mult, 2f - mult, 18f * mult);
    //            break;
    //        case EnemyTypes.JetFighter:
    //            MoveData.Set(0f, .5f, 120f * mult, 2.25f - mult, 20f * mult);
    //            break;
    //        case EnemyTypes.Trident:
    //            MoveData.Set(0f, .5f, 100f * mult, 2.5f - mult, 16f * mult);
    //            break;
    //        case EnemyTypes.Basic:
    //            MoveData.Set(0f, .5f, 90f * mult, 2.75f - mult, 15f * mult);
    //            break;
    //        case EnemyTypes.SquadLead:
    //            MoveData.Set(0f, .5f, 85f * mult, 3f - mult, 15f * mult);
    //            break;
    //        case EnemyTypes.Transport:
    //            MoveData.Set(0f, .5f, 70f * mult, 4f - mult, 12f * mult);
    //            break;
    //        case EnemyTypes.Tank:
    //            MoveData.Set(0f, .5f, 60f * mult, 5f - mult, 10f * mult);
    //            break;
    //        case EnemyTypes.FinalBoss:
    //            MoveData.Set(0f, 0f, 0f, 6f - mult, 0f);
    //            break;
    //    }
    //}
    //#endregion

    //#region States
    //void Alert()
    //{
    //    MoveData.IncreaseSpeed();
    //    Vector3 lastplayerdir = stateManager.LastKnownPos - MyTransform.position;
    //    if (Vector3.Distance(MyTransform.position, stateManager.LastKnownPos) < 250f)
    //    {
    //        stateManager.SetEnemyTarget(null);
    //        return;
    //    }

    //    Vector3 dir = Vector3.RotateTowards(MyTransform.forward, lastplayerdir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);
    //    MyTransform.rotation = Quaternion.LookRotation(dir);
    //}
    //void Attack()
    //{
    //    MoveData.IncreaseSpeed();
    //    if (stateManager.Target != null)
    //    {
    //        Vector3 playerDir = stateManager.Target.position - MyTransform.position;
    //        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);
    //        if (stats.GetEnemyType() == EnemyTypes.Droid)
    //        {
    //            MyTransform.rotation = Quaternion.LookRotation(direction);
    //        }
    //        else
    //        {
    //            if (Vector3.Distance(stateManager.Target.position, MyTransform.position) > 500f)
    //                MyTransform.rotation = Quaternion.LookRotation(direction);
    //            else
    //            {
    //                direction.x += 75f;
    //                MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(direction), Time.fixedDeltaTime / MoveData.RotateSpeed);
    //            }
    //        }
    //    }
    //    headingX = MyTransform.eulerAngles.x;
    //    headingY = MyTransform.eulerAngles.y;
    //}

    //void Flee()
    //{
    //    MoveData.IncreaseSpeed();
    //    Vector3 playerDir = MyTransform.position - stateManager.Target.position;
    //    Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);
    //    if (stats.GetEnemyType() == EnemyTypes.Droid)
    //    {
    //        MyTransform.rotation = Quaternion.LookRotation(direction);
    //    }
    //    else
    //    {
    //        if (Vector3.Distance(stateManager.Target.position, MyTransform.position) > 200f)
    //            MyTransform.rotation = Quaternion.LookRotation(direction);
    //        else
    //        {
    //            direction.x += 75f;
    //            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(direction), Time.fixedDeltaTime / MoveData.RotateSpeed);
    //        }
    //    }
    //    headingX = MyTransform.eulerAngles.x;
    //    headingY = MyTransform.eulerAngles.y;
    //}
    //#endregion

}