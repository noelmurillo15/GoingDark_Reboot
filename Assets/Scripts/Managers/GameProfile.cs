// GoingDarkReboot.V1
//  Allan Noel Murillo

using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameProfile : MonoBehaviour {

    public static GameProfile gControl;


    void Awake()
    {
        //  Singleton Design Pattern
        if (gControl == null)
        {
            DontDestroyOnLoad(gameObject);
            gControl = this;
        }
        else if (gControl != this)
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.playerName = "Link";
        data.difficulty = "";
        data.currentLevel = 4;
        data.saveSlot = 0;
        data.credits = 10000;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
    }
}

[Serializable]
class PlayerData
{
    public string playerName;
    public string difficulty;
    public int currentLevel;
    public int saveSlot;
    public int credits;
}