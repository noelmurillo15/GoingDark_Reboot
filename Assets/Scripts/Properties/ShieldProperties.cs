///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using System;
using UnityEngine;


[Serializable]
public class ShieldProperties {


	#region Variables
	public GameObject ShieldGameobject;
	public bool active;
	public float health;
	public float maxHealth;
    #endregion



	/// <summary>
	/// Constructor : Enemy
	/// </summary>
	/// <param name="_shield"></param>
	/// <param name="shieldHP"></param>
    public ShieldProperties(float shieldHP, GameObject _shield)
    {
        active = true;
        health = shieldHP;
        maxHealth = shieldHP;
        ShieldGameobject = _shield;
	}

    #region Accessors
    public bool GetShieldActive()
    {
        return active;
    }
    #endregion

    #region Modifiers
    public void Heal(float _val)
    {
		health = Mathf.Clamp(health + _val, 0f, maxHealth);
    }

    public void FullRestore()
    {
        active = true;
        health = maxHealth;
        if(ShieldGameobject != null)
            ShieldGameobject.SetActive(true);
    }

    public void SetShieldActive(bool flip)
    {
        if (!flip)
            health = 0f;                    
        else        
            health = maxHealth;

        active = flip;
        ShieldGameobject.SetActive(flip);
    }

    public float ShieldHealthPercentage()
    {
        return (health / maxHealth) * .5f;
    }

    public void Damage(float _val)
    {
        if (active)
        {
            health -= _val;
            if (health <= 0f)
            {
                health = 0f;
                active = false;

                if (ShieldGameobject != null)
                    ShieldGameobject.SetActive(false);
            }
        }
    }
    #endregion
}