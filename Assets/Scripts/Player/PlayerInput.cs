using UnityEngine;
using GoingDark.Core.Enums;

public class PlayerInput : MonoBehaviour
{
    #region Properties
    [SerializeField] MyInputManager myInput;

    private SystemManager systems;
    private LaserSystem lasers;
    private MissileSystem missiles;
    private MovementProperties movement;
    #endregion


    void Start()
    {
        if (myInput == null)
            Debug.LogError("InputManager Script is not attached");

        systems = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        movement = GetComponent<PlayerMovement>().GetMoveData();
        Invoke("FindSystems", 2f);
    }

    void FindSystems()
    {
        lasers = systems.GetSystemScript(SystemType.Laser) as LaserSystem;
        missiles = systems.GetSystemScript(SystemType.Missile) as MissileSystem;
    }

    void Update() {

        if (myInput.GetRightClick())
            lasers.WeaponSwap();

        if (myInput.GetLeftClick())
        {
            if(missiles == null)
                missiles = systems.GetSystemScript(SystemType.Missile) as MissileSystem;

            missiles.WeaponSwap();
        }

        if (myInput.GetAButton())
            systems.ActivateSystem(SystemType.Cloak);

        if (myInput.GetBButton())
            systems.ActivateSystem(SystemType.Decoy);

        if (myInput.GetYButton())
            systems.ActivateSystem(SystemType.Hyperdrive);

        if (myInput.GetLeftBumper())
            systems.ActivateSystem(SystemType.Missile);

        if (myInput.GetRightBumper())
            systems.ActivateSystem(SystemType.Laser);
    }
}