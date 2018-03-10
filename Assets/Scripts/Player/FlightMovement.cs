using UnityEngine;


public class FlightMovement : MonoBehaviour
{

    #region Properties
    public bool boostActive;
    public ShipMaster shipMaster;


    private Transform MyTransform;
    private AudioManager _audioInstance;
    private Rigidbody MyRigidbody;
    #endregion


    void Start()
    {
        OVRInput.GetActiveController();

        boostActive = false;
        MyTransform = transform;        
        _audioInstance = AudioManager.instance;
        MyRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {

        float lefttrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);

        Vector2 primaryindextrigger = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 secndaryindextrigger = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (lefttrigger > 0f)
			GetMovementRef().ChangeSpeed(lefttrigger);
        else
			GetMovementRef().DecreaseSpeed();

        Yaw(primaryindextrigger);
        Roll(secndaryindextrigger);
        Pitch(primaryindextrigger);
        Flight();
    }

    #region Accessors
    public MovementProperties GetMovementRef()
    {
        return shipMaster.GetMoveData();
    }
    #endregion

    #region Movement    
    public void StopMovement()
    {
		GetMovementRef().SetSpeed(0f, true);
    }
    public void Yaw(Vector2 myInput)
    {
        if (myInput .x != 0f)
            MyTransform.Rotate(Vector3.up * Time.deltaTime * (GetMovementRef().rotateSpeed * myInput.x));
    }
    public void Roll(Vector2 myInput)
    {
        if (myInput.x != 0f)
            MyTransform.Rotate(Vector3.back * Time.deltaTime * (GetMovementRef().rotateSpeed * myInput.x));
    }
    public void Pitch(Vector2 myInput)
    {
        if (myInput.y != 0f)
            MyTransform.Rotate(Vector3.right * Time.deltaTime * (GetMovementRef().rotateSpeed * myInput.y));
    }
    void Flight()
    {
        if (GetMovementRef().speed <= 0f)
            return;

        _audioInstance.PlayThruster();

        MyRigidbody.AddForce(MyTransform.forward * GetMovementRef().speed, ForceMode.VelocityChange);
        if (MyRigidbody.velocity.magnitude > GetMovementRef().speed)
            MyRigidbody.velocity = MyRigidbody.velocity.normalized * GetMovementRef().speed;        
    }
    #endregion    
}