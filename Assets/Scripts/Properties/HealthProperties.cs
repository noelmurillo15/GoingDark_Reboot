using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HealthProperties
{
    #region Properties
    public float Health;
    public float MaxHealth;

    //  Enemy Data
    private EnemyMaster enemyMaster;
    private Transform baseRef;

    //  Player Data
    public bool isPlayer;
    public bool isPlayerDead;
    private Image HealthBar;
    #endregion

    /// <summary>
    /// Contructor : Enemy
    /// </summary>
    /// <param name="_hp"></param>
    /// <param name="_ref"></param>
    public HealthProperties(float _hp, EnemyMaster _ref)
    {
        MaxHealth = _hp;
        Health = MaxHealth;

        enemyMaster = _ref;
        isPlayer = false;
        isPlayerDead = false;
    }

    /// <summary>
    /// Contructor : Player
    /// </summary>
    /// <param name="_hp"></param>
    /// <param name="_player"></param>
    public HealthProperties(float _hp, bool _player)
    {
        Health = _hp;
        MaxHealth = _hp;
        isPlayerDead = false;
        isPlayer = _player;
        HealthBar = GameObject.Find("PlayerHealth").GetComponent<Image>();
    }

    #region Modifiers
    public void FullRestore()
    {
        isPlayerDead = false;
        Health = MaxHealth;
        UpdateHPBar();
    }  
    
    public void UpdateHPBar()
    {
        HealthBar.fillAmount = (Health / MaxHealth) * .5f;
    }  

    public void Damage(float _dmg)
    {
        if (_dmg <= 0)
        {
            Debug.LogError("No Damage!");
            return;
        }

        Health -= _dmg;        
        if (isPlayer)
        {
            UpdateHPBar();
            AudioManager.instance.PlayHit();
            if (!isPlayerDead && Health <= 0f)
            {
                isPlayerDead = true;
                //  TODO : Death
            }         
        }
        else
        {
            if (Health <= 0f)
            {
                enemyMaster.CallEventEnemyDie();
                return;
            }

            enemyMaster.CallEventEnemyGetHit();

            if(Health <= (MaxHealth * .2f) && !enemyMaster.GetIsHealthCritical())
            {
                enemyMaster.CallEventEnemyLowHealth();
                return;
            }
            if(enemyMaster.GetIsHealthCritical())
            {
                enemyMaster.CallEventEnemyHealthRecovered();
            }
        }
    }
    #endregion
}