///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;


public class PlayerMovement : MonoBehaviour {


	#region Properties
	[SerializeField] MyInputManager myInput;
	[SerializeField] float currentSpeedPercentage;

	public bool boostActive;

	//  Cached Properties
	private ShipMaster moveRef;
	private Transform MyTransform;
	private AudioManager _audioInstance;
	private Rigidbody MyRigidbody;
	#endregion


	void Start()
	{
		boostActive = false;
		currentSpeedPercentage = 0f;

		MyTransform = transform;
		moveRef = GetComponent<ShipMaster>();
		_audioInstance = AudioManager.instance;
		MyRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		moveRef.GetMoveData().SetSpeed(myInput.GetLeftTriggerAxis(), false);
		moveRef.GetMoveData().SetSpeed(-(myInput.GetRightTriggerAxis()), false);

		Yaw(myInput.GetLeftJoystickAxis());
		Pitch(myInput.GetLeftJoystickAxis());
		Roll(myInput.GetRightJoystickAxis());

		Flight();
	}

	#region Movement    
	public void StopMovement()
	{
		moveRef.GetMoveData().SetSpeed(0f, true);
	}

	float GetSpeedPercentage()
	{
		currentSpeedPercentage = moveRef.GetMoveData().speed / moveRef.GetMoveData().maxSpeed;
		return currentSpeedPercentage;
	}

	void Yaw(Vector2 input)
	{
		MyTransform.Rotate(Vector3.up * Time.fixedDeltaTime * (moveRef.GetMoveData().rotateSpeed * input.x));
	}
	void Pitch(Vector2 input)
	{
		MyTransform.Rotate(Vector3.right * Time.fixedDeltaTime * (moveRef.GetMoveData().rotateSpeed * input.y));
	}
	void Roll(Vector2 input)
	{
		MyTransform.Rotate(Vector3.back * Time.fixedDeltaTime * (moveRef.GetMoveData().rotateSpeed * input.x));
	}	

	void Flight()
	{
		_audioInstance.ThrusterVolume(GetSpeedPercentage());
		MyRigidbody.AddForce(MyTransform.forward * moveRef.GetMoveData().speed, ForceMode.VelocityChange);
		if (MyRigidbody.velocity.magnitude > moveRef.GetMoveData().speed)
			MyRigidbody.velocity = MyRigidbody.velocity.normalized * moveRef.GetMoveData().speed;
	}
	#endregion
}