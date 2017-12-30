using UnityEngine;

public class FlightMovement : MonoBehaviour
{

    #region Properties
    public bool boostActive;
    public MovementProperties MoveData;


    private Transform MyTransform;
    private AudioManager _audioInstance;
    private Rigidbody MyRigidbody;
    #endregion


    void Start()
    {

        OVRInput.GetActiveController();

        if (OVRInput.IsControllerConnected(OVRInput.Controller.All))
        {
            Debug.Log("Oculus All Controller is Active");
        }

        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch))
        {
            Debug.Log("Oculus Left Touch Controller is Active");
        }

        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            Debug.Log("Oculus Right Touch Controller is Active");
        }


        boostActive = false;
        MyTransform = transform;
        MoveData.Set(0f, 1f, 250f, 80f, 40f);

        _audioInstance = AudioManager.instance;
        MyRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {

        float lefttrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);

        Vector2 primaryindextrigger = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 secndaryindextrigger = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (lefttrigger > 0f)
            MoveData.ChangeSpeed(lefttrigger);
        else
            MoveData.DecreaseSpeed();

        Yaw(primaryindextrigger);
        Roll(secndaryindextrigger);
        Pitch(primaryindextrigger);
        Flight();
    }

    #region Accessors
    public MovementProperties GetMoveData()
    {
        return MoveData;
    }
    #endregion

    #region Movement    
    public void StopMovement()
    {
        MoveData.Speed = 0f;
    }
    public void Yaw(Vector2 myInput)
    {
        if (myInput .x != 0f)
            MyTransform.Rotate(Vector3.up * Time.deltaTime * (MoveData.RotateSpeed * myInput.x));
    }
    public void Roll(Vector2 myInput)
    {
        if (myInput.x != 0f)
            MyTransform.Rotate(Vector3.back * Time.deltaTime * (MoveData.RotateSpeed * myInput.x));
    }
    public void Pitch(Vector2 myInput)
    {
        if (myInput.y != 0f)
            MyTransform.Rotate(Vector3.right * Time.deltaTime * (MoveData.RotateSpeed * myInput.y));
    }
    void Flight()
    {
        if (MoveData.Speed <= 0f)
            return;

        _audioInstance.PlayThruster();

        MyRigidbody.AddForce(MyTransform.forward * MoveData.Speed, ForceMode.VelocityChange);
        if (MyRigidbody.velocity.magnitude > MoveData.Speed)
            MyRigidbody.velocity = MyRigidbody.velocity.normalized * MoveData.Speed;        
    }
    #endregion    
}