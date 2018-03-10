using UnityEngine;
using ANM.Utilities;
using GoingDark.Core.Enums;
using System.Collections.Generic;


public class EnemyManager : MonoBehaviour {


    #region Properties
    [SerializeField] GameDifficulty Difficulty;
    [SerializeField] List<EnemyMaster> enemies = new List<EnemyMaster>();

    private int creditMultiplier;
    private Transform PlayerPosition;
    #endregion



    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        switch (PlayerPrefs.GetString("Difficulty"))
        {
            case "Easy":
                Difficulty = GameDifficulty.Easy;
                creditMultiplier = 1;
                break;
            case "Medium":
                Difficulty = GameDifficulty.Normal;
                creditMultiplier = 2;
                break;
            case "Hard":
                Difficulty = GameDifficulty.Hard;
                creditMultiplier = 3;
                break;
            case "Nightmare":
                Difficulty = GameDifficulty.Nightmare;
                creditMultiplier = 4;
                break;
            default:
                Debug.Log("Enemy Manager could not get Level Difficulty");
                Difficulty = GameDifficulty.Easy;
                creditMultiplier = 1;
                break;
        }
        Debug.Log("Game Difficulty : " + Difficulty.ToString());

        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    #region Accessors
    public GameDifficulty GetGameDifficulty()
    {
        return Difficulty;
    }
    public Transform GetPlayerTransform()
    {
        return PlayerPosition;
    }
    #endregion    

    #region Modifiers
    public void AddEnemy(EnemyMaster enemy)
    {
        enemies.Add(enemy);
    }

    public void AllEnemiesPatrol()
    {
        for (int x = 0; x < enemies.Count; x++)
            enemies[x].AttackTarget = null;
    }

    void RandomAmmoDrop(Vector3 _pos)
    {
        if (Random.Range(1, 2) == 1)
        {
            GameObject go = ObjectPoolManager.Instance.GetAmmoDrop();
            go.transform.position = _pos;
            go.transform.rotation = Quaternion.identity;
            go.SetActive(true);
        }
    }

    public void RemoveEnemy(EnemyMaster enemy)
    {
        GameObject explosive = ObjectPoolManager.Instance.GetEnemyExplosion();

        if (explosive != null)
        {
            explosive.transform.position = enemy.transform.position;
            explosive.SetActive(true);
            RandomAmmoDrop(enemy.transform.position);
        }


        int creds = 0;         
        switch (enemy.GetEnemyType())
        {
            case EnemyTypes.Basic:
                creds += 15 * creditMultiplier;
                break;
            case EnemyTypes.Droid:
                creds += 2 * creditMultiplier;
                break;
            case EnemyTypes.SquadLead:
                creds += 25 * creditMultiplier;
                break;
            case EnemyTypes.JetFighter:
                creds += 5 * creditMultiplier;
                break;
            case EnemyTypes.Transport:
                creds += 200 * creditMultiplier;
                break;
            case EnemyTypes.Trident:
                creds += 12 * creditMultiplier;
                break;
            case EnemyTypes.Tank:
                creds += 150 * creditMultiplier;
                break;
            case EnemyTypes.FinalBoss:
                creds += 300 * creditMultiplier;
                break;
        }
        PlayerPosition.SendMessage("UpdateCredits", creds);
        enemies.Remove(enemy);
    }
    #endregion
}