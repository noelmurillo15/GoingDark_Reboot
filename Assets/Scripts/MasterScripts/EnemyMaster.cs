///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;
using ANM.Utilities;
using GoingDark.Core.Enums;


public class EnemyMaster : ShipMaster {


	#region Variables
	[Header("Enemy")]
	[SerializeField] EnemyTypes myType;
    [SerializeField] float baseHealth;
    [SerializeField] bool hasShield;
    

	float multiplier;
    float collisionTimer;
    EnemyStatePattern myStatePattern;
	#endregion


	#region Unity Functions
	void Awake()
    {
		EventDeath += Kill;
        myStatePattern = GetComponent<EnemyStatePattern>();
        LoadEnemyDifficulty();
    }

	void OnDisable()
	{
		EventDeath -= Kill;
	}

	void Update()
	{		
		myMoveData.IncreaseSpeed();
	}
	#endregion

	#region Accessors      
	public float GetDifficultyMultiplier() { return multiplier; }

    public EnemyTypes GetEnemyType() { return myType; }

    public EnemyStatePattern GetStateManager() { return myStatePattern; }
    #endregion

	#region Private Methods
	void LoadEnemyData()
	{
		base.Initialize(baseHealth * multiplier, hasShield);
	}

	void LoadEnemyDifficulty()
    {
        switch (PlayerPrefs.GetString("Difficulty"))
        {
            case "Easy":
                multiplier = 1f;
                break;
            case "Medium":
                multiplier = 1.5f;
                break;
            case "Hard":
                multiplier = 2f;
                break;
            case "Nightmare":
                multiplier = 3f;
                break;
        }
		LoadEnemyData();
	}

    void Kill()
	{
		if (GetComponent<EnemyTrail>() != null)
			GetComponent<EnemyTrail>().Kill();

		GameObject go = ObjectPoolManager.Instance.GetEnemyExplosion();
		go.transform.position = transform.position;
		go.transform.rotation = transform.rotation;
		go.SetActive(true);		
		Destroy(gameObject, 1f);
	}
    #endregion 
}