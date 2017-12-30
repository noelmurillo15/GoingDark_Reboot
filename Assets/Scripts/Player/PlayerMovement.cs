using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    #region Properties
    [SerializeField] MyInputManager myInput;
    [SerializeField] float currentSpeedPercentage;
    [SerializeField] MovementProperties MoveData;

    public bool boostActive;

    //  Cached Properties
    private Transform MyTransform;
    //private ParticleSystem particles;
    private AudioManager _audioInstance;
    private Rigidbody MyRigidbody;
    #endregion


    void Start()
    {
        MoveData.Set(0f, 1f, 100f, 60f, 20f);
        currentSpeedPercentage = 0f;
        boostActive = false;
        MyTransform = transform;

        _audioInstance = AudioManager.instance;
        MyRigidbody = GetComponent<Rigidbody>();
        //particles = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        //  Xbox 360 Controllers
        MoveData.SetSpeed(myInput.GetLeftTriggerAxis(), false);
        MoveData.SetSpeed(-(myInput.GetRightTriggerAxis()), false);

        Yaw(myInput.GetLeftJoystickAxis());
        Pitch(myInput.GetLeftJoystickAxis());
        Roll(myInput.GetRightJoystickAxis());


        if (boostActive)
        {
            MoveData.Boost = 5f;
            MoveData.Acceleration = 50f;
        }
        
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
        MoveData.SetSpeed(0f, true);
    }
    public void Yaw(Vector2 input)
    {
        MyTransform.Rotate(Vector3.up * Time.fixedDeltaTime * (MoveData.RotateSpeed * input.x));        
    }
    public void Pitch(Vector2 input)
    {
        MyTransform.Rotate(Vector3.right * Time.fixedDeltaTime * (MoveData.RotateSpeed * -1 * input.y));
    }
    public void Roll(Vector2 input)
    {
        MyTransform.Rotate(Vector3.back * Time.fixedDeltaTime * (MoveData.RotateSpeed * input.x));
    }
    public void Yaw(float xinput, float yinput)
    {
        MyTransform.Rotate(Vector3.up * Time.fixedDeltaTime * (MoveData.RotateSpeed * xinput));
    }
    public void Pitch(float xinput, float yinput)
    {
        MyTransform.Rotate(Vector3.right * Time.fixedDeltaTime * (MoveData.RotateSpeed * -1 * yinput));
    }
    public void Roll(float xinput, float yinput)
    {
        MyTransform.Rotate(Vector3.back * Time.fixedDeltaTime * (MoveData.RotateSpeed * xinput));
    }

    void Flight()
    {
        if (MoveData.MaxSpeed > 0f)
            currentSpeedPercentage = MoveData.Speed / MoveData.MaxSpeed;
        else
            currentSpeedPercentage = 0f;

        _audioInstance.ThrusterVolume(currentSpeedPercentage);
        //particles.startSpeed = -(currentSpeedPercentage * .1f);

        MyRigidbody.AddForce(MyTransform.forward * MoveData.Speed, ForceMode.VelocityChange);
        if (MyRigidbody.velocity.magnitude > MoveData.Speed)
            MyRigidbody.velocity = MyRigidbody.velocity.normalized * MoveData.Speed;        
    }
    #endregion    
}