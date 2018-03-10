using UnityEngine;
using System.Collections;


public class BossStats : MonoBehaviour {


    private EnemyMaster enemyMaster;

    public int numOrbsActive;
    private int maxOrbs = 4;

    [SerializeField] private GameObject[] Orbs;
    [SerializeField] private GameObject[] burns;


    void Start()
    {
        enemyMaster = GetComponent<EnemyMaster>();
        burns[0].SetActive(false);
        burns[1].SetActive(false);
        burns[2].SetActive(false);

        numOrbsActive = maxOrbs;

        CheckHealth();
    }

    public void DecreaseOrbCount()
    {
        numOrbsActive--;
        if (numOrbsActive <= 0)
        {
            enemyMaster.GetShieldData().SetShieldActive(false);
            numOrbsActive = maxOrbs;        
            Invoke("ShieldRegen", 10f);
        }
    }
    void ShieldRegen()
    {
        for (int x = 0; x < numOrbsActive; x++)
            Orbs[x].SetActive(true);

        enemyMaster.GetShieldData().SetShieldActive(true);
    }

    private IEnumerator CheckHealth()
    {
        while (true)
        {
            if (enemyMaster.GetHealthData() != null)
            {
                float hp = enemyMaster.GetHealthData().health / enemyMaster.GetHealthData().maxHealth;

                if (hp <= .75f)
                    burns[0].SetActive(true);
                else if (hp <= .5f)
                    burns[1].SetActive(true);
                else if (hp <= .25f)
                    burns[2].SetActive(true);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}