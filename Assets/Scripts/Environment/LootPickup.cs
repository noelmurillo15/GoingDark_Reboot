using UnityEngine;

public class LootPickup : MonoBehaviour
{

    private PlayerMaster playerStats;

    // Use this for initialization
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMaster>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
			//	TODO Credits
            //playerStats.UpdateCredits(100);
            AudioManager.instance.PlayCollect();
            Destroy(gameObject);
        }
    }
}