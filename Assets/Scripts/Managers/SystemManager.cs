using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;


public class SystemManager : MonoBehaviour {

    #region Properties
    private string sceneName;
    private MessageScript messages;
    private CloakSystem cloaking;   
    private Dictionary<SystemType, ShipSystem> MainDevices;
    private Dictionary<SystemType, GameObject> SecondaryDevices;    
    #endregion


    void Awake()
    {
        MainDevices = new Dictionary<SystemType, ShipSystem>();
        SecondaryDevices = new Dictionary<SystemType, GameObject>();

        messages = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();

        InitializeDevice(SystemType.Decoy);
        InitializeDevice(SystemType.Hyperdrive);
        InitializeDevice(SystemType.Laser);
        InitializeDevice(SystemType.Cloak);
        InitializeDevice(SystemType.Shield);
        InitializeDevice(SystemType.Missile);
    }

    #region Public Methods
    public void ActivateSystem(SystemType key)
    {
        if (key == SystemType.Cloak && cloaking != null && cloaking.GetCloaked())
        {
            Debug.Log("Cloak was activated while Cloak was already active");
            cloaking.UnCloakShip();
            return;
        }

        if (MainDevices.ContainsKey(key))   //  If System is installed
        {
            if (MainDevices[key].GetSystemReady())  //  If system is online and not on cooldown
            {
                if (cloaking.GetCloaked())  
                    cloaking.UnCloakShip();

                MainDevices[key].Activate();    //  Activate System                
            }
        }
    }


    public MonoBehaviour GetSystemScript(SystemType key)
    {
        ShipSystem sdev = null;
        if (MainDevices.TryGetValue(key, out sdev))
            return sdev;

        Debug.LogError("GetSystemScript did not find " + key.ToString());
        return null;        
    }

    public void SystemDamaged()
    {
        List<SystemType> keylist = new List<SystemType>(MainDevices.Keys);
        int rand = Random.Range(0, keylist.Count);
        SystemType type = keylist[rand];
        if (MainDevices.ContainsKey(type) && type != SystemType.Laser)
        {
            MainDevices[type].SetStatus(SystemStatus.Offline);
            messages.SystemReport(type);
        }
    }
    public void FullSystemRepair()
    {
        List<SystemType> keylist = new List<SystemType>(MainDevices.Keys);
        for (int i = 0; i < keylist.Count; i++)
            MainDevices[keylist[i]].Repair();                         
    }

    public int GetSystemCooldown(SystemType key)
    {
        ShipSystem dev = null;
        if (MainDevices.TryGetValue(key, out dev))
        {
            if (dev.Status == SystemStatus.Online)
            {
                if (key == SystemType.Cloak && cloaking.GetCloaked())
                    return -2;

                return (int)dev.GetCooldown();
            }
            else
            {
                return -10;
            }
        }
        else
        {
            return -1;
        }
    }
    #endregion

    #region Private Methods
    public void InitializeDevice(SystemType key)
    {
        if (MainDevices.ContainsKey(key) || SecondaryDevices.ContainsKey(key))
        {
            Debug.Log("Device is already initialized : " + key.ToString());
            return;
        }

        GameObject system = null;
        switch (key)
        {
            case SystemType.Cloak:
                system = Resources.Load<GameObject>("Devices/Cloak");
                break;
            case SystemType.Decoy:
                system = Resources.Load<GameObject>("Devices/Decoy");
                break;
            case SystemType.Laser:
                system = Resources.Load<GameObject>("Devices/Lasers");
                break;
            case SystemType.Shield:
                system = Resources.Load<GameObject>("Devices/Shield");
                break;
            case SystemType.Missile:
                system = Resources.Load<GameObject>("Devices/Missiles");
                break;
            case SystemType.Hyperdrive:
                system = Resources.Load<GameObject>("Devices/HyperDrive");
                break;
        }

        if (system != null)
        {
            GameObject go = Instantiate(system, Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.parent = transform;
            go.transform.localPosition = system.transform.localPosition;
            go.transform.localRotation = system.transform.localRotation;
            AddDevice(key, go);
            return;
        }
        Debug.Log(key.ToString() + " Device was not Initialized");
    }
    private void AddDevice(SystemType key, GameObject dev)
    {
        switch (key)
        {
            #region Main Devices
            case SystemType.Decoy:
                MainDevices.Add(key, dev.GetComponent<DecoySystem>() as ShipSystem);
                break;

            case SystemType.Hyperdrive:
                MainDevices.Add(key, dev.GetComponent<HyperdriveSystem>() as ShipSystem);
                break;

            case SystemType.Missile:
                MainDevices.Add(key, dev.GetComponent<MissileSystem>() as ShipSystem);
                break;

            case SystemType.Cloak:
                cloaking = dev.GetComponent<CloakSystem>();
                MainDevices.Add(key, cloaking as ShipSystem);
                break;

            case SystemType.Laser:
                MainDevices.Add(key, dev.GetComponent<LaserSystem>() as ShipSystem);
                break;
            #endregion

            #region Secondary Devices
            case SystemType.Shield:
                SecondaryDevices.Add(key, dev);
                break;
                #endregion
        }
    }
    
    #endregion
}