using UnityEngine;


//[System.Serializable]
public class LaserBehaviour : ProjectileMaster {



	void OnEnable()
	{
		if (!init)
		{
			init = true;

			myTransform = transform;
			gameObject.SetActive(false);
		}
		else
		{
			Invoke("Kill", GetAliveTimer());
		}
	}

	void FixedUpdate()
	{
		myTransform.Translate(0f, 0f, GetSpeed() * Time.fixedDeltaTime);
	}
}