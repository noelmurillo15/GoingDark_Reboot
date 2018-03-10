///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using System;
using UnityEngine;


[Serializable]
public class MovementProperties {


	#region Variables
	public float speed;
	public float boost;
	public float maxSpeed;
	public float rotateSpeed;
	public float acceleration;
	#endregion



	/// <summary>
	/// Constructor : Default
	/// </summary>
	public MovementProperties()
    {
		Reset();
    }
	/// <summary>
	/// Constructor : Custom
	/// </summary>
	/// <param name="_speed"></param>
	/// <param name="_boost"></param>
	/// <param name="_maxspeed"></param>
	/// <param name="_rotatespeed"></param>
	/// <param name="_accel"></param>
	public MovementProperties(float _speed, float _boost, float _maxspeed, float _rotatespeed, float _accel)
    {
        speed = _speed;
        boost = _boost;
        maxSpeed = _maxspeed;
        acceleration = _accel;
        rotateSpeed = _rotatespeed;
    }

	#region Modifiers
	public void Reset()
	{
		speed = 0f;
		boost = 0f;
		maxSpeed = 0f;
		rotateSpeed = 0f;
		acceleration = 0f;
	}
	
	public void SetSpeed(float val, bool overwrite)
    {
        if(overwrite)
            speed = val;
        else
        {
            speed += val;

            if (speed > 100f * boost)
                speed = 100f * boost;

            if (speed < 0f)
                speed = 0f;
        }
    }

    public void SetSpeed(Vector2 _val)
    {
        speed += _val.x * acceleration;
        speed += _val.y * acceleration;

        if (speed > 100f * boost)
            speed = 100f * boost;

        if (speed < 0f)
            speed = 0f;
    }      

    public void ChangeSpeed(float triggerVal)
    {
        if (speed < (maxSpeed * boost * triggerVal))
            speed += Time.fixedDeltaTime * acceleration;
        else if (speed > (maxSpeed * boost * triggerVal) + .5f)
            DecreaseSpeed();
    }
    public void IncreaseSpeed()
    {
        if (speed < (maxSpeed * boost))
            speed += Time.fixedDeltaTime * acceleration;
        else
            DecreaseSpeed();
    }
    public void DecreaseSpeed()
    {
        if (speed > 1f)
            speed = Mathf.Lerp(speed, 0f, Time.fixedDeltaTime * .5f);
        else
            speed = 0f;
    }
	#endregion
}