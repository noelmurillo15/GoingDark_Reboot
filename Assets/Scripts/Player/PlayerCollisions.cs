///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;


public class PlayerCollisions : MonoBehaviour
{

    private float padding;
    private PlayerStats stats;
    private MovementProperties movedata;

    void Awake()
    {
        padding = 0f;
        stats = GetComponent<PlayerStats>();
		movedata = stats.GetMoveData();
    }

    void LateUpdate()
    {
        if (padding > 0f)
            padding -= Time.deltaTime;
    }

	void OnCollisionEnter(Collision hit)
    {
        if (padding <= 0f)
        {
            if (hit.transform.CompareTag("Asteroid"))
            {
				Debug.Log("Crashed with Asteroid");
				//stats.CrashHit(movedata.speed / movedata.maxSpeed);
                padding = 1f;
            }
            if (hit.transform.CompareTag("Enemy"))
            {
				Debug.Log("Crashed with Enemy");
                //stats.CrashHit(movedata.speed / movedata.maxSpeed);
                padding = 1f;
            }
            if (hit.transform.CompareTag("Meteor"))
            {
				Debug.Log("Crashed with Meteor");
				padding = 1f;
                stats.Kill();
            }            
        }
    }
}