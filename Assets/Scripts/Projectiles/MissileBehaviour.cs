using UnityEngine;


public class MissileBehaviour : ProjectileMaster {


	#region Properties  
	[Header("Missile Behaviour")]
	public float rotateSpeed = 50f;

	bool tracking;
	bool deflected;
	Quaternion targetRotation;

	Transform myTarget;
	Rigidbody myRigidbody;
	#endregion



	void OnEnable()
	{
		if (!init)
		{
			init = true;
			myTarget = null;
			tracking = false;
			deflected = false;

			myTransform = transform;
			targetRotation = Quaternion.identity;
			myRigidbody = GetComponent<Rigidbody>();
			gameObject.SetActive(false);
		}
		else
		{
			deflected = false;
			tracking = false;
			myTarget = null;
			Invoke("Kill", GetAliveTimer());
		}
	}

	void FixedUpdate()
	{
		if (tracking)
		{
			LookAt();
			myTransform.Translate(0f, 0f, GetSpeed() * Time.fixedDeltaTime);
		}
		else
		{
			if (!deflected)
				myRigidbody.MovePosition(myTransform.position + myTransform.forward * Time.fixedDeltaTime * GetSpeed());
			else
				myRigidbody.MovePosition(myTransform.position + -myTransform.forward * Time.fixedDeltaTime * GetSpeed());
		}
	}

	#region Tracking
	void LookAt()
	{
		if (myTarget != null)
		{
			targetRotation = Quaternion.LookRotation(myTarget.position - myTransform.position);
			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
		}
	}

	void LockedOn(Transform _target)
	{
		if (_target != null)
		{
			myTarget = _target;
			InitializeTracking();
		}
	}

	void InitializeTracking()
	{
		float rand = Random.Range(1f, 100f);
		if (rand > 10f)
		{
			tracking = true;
			if (myTarget == null)
			{
				Debug.LogError("Enemy Missile Target was not found");
				tracking = false;
			}
		}
	}

	public void Deflect()
	{
		CancelInvoke();
		myTarget = null;
		tracking = false;
		deflected = true;
		Invoke("Kill", Random.Range(.5f, 2f));
	}
	#endregion
}