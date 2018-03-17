using UnityEngine;
using System.Collections;

public class EnemyTrail : MonoBehaviour
{
    private int numTrails;
    private TrailRenderer[] trails;
    [SerializeField] HealthProperties HealthInfo;


    // Use this for initialization
    void Start()
    {
        Invoke("FindEnemyData", 3f);
    }

    void FindEnemyData()
    {
        trails = GetComponentsInChildren<TrailRenderer>();
        numTrails = trails.Length;    
        HealthInfo = GetComponent<EnemyMaster>().GetHealthData();
        CheckHealth(HealthInfo);
    }    

    private IEnumerator CheckHealth(HealthProperties _health)
    {       
        while (true)
        {
            int col = 0;
            float _hp = HealthInfo.health / HealthInfo.maxHealth;            

            if (_hp > .75f)
                col = 0;            
            else if (_hp <= .75f && _hp > .25f)
                col = 1;            
            else
                col = 2;

            if (trails[col] != null)
            {
                for (int x = 0; x < numTrails; x++)
                {
                    trails[x].time = _hp * 25f;
                    if (x == col)
                        trails[x].gameObject.SetActive(true);
                    else
                        trails[x].gameObject.SetActive(false);
                }
            }

            yield return new WaitForSeconds(.25f);
        }
    }

    public void Kill()
    {
        //TODO   kill coroutine       CheckHealth(HealthInfo);
        Destroy(gameObject);
    }
}
