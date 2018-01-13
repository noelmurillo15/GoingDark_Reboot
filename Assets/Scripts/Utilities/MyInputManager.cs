using UnityEngine;


public class MyInputManager : MonoBehaviour {


    [SerializeField] bool isOculusTouchConnected;


    void Awake()
    {
        bool leftController, rightController;
        leftController = OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote);
        rightController = OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote);

        if (leftController && rightController)
        {
            isOculusTouchConnected = true;
            Debug.Log("Oculus Touch Controllers are Connected");
        }
    }

    public bool IsOculusTouchConnected()
    {
        return isOculusTouchConnected;
    }

    #region Input
    public Vector2 GetLeftJoystickAxis()
    {
        Vector2 axis = Vector2.zero;
        axis.x = Input.GetAxis("LeftJoystickX");
        axis.y = Input.GetAxis("LeftJoystickY");
        return axis;
    }
    public Vector2 GetRightJoystickAxis()
    {
        Vector2 axis = Vector2.zero;
        axis.x = Input.GetAxis("RightJoystickX");
        axis.y = Input.GetAxis("RightJoystickY");
        return axis;
    }
    public Vector2 GetDPadAxis()
    {
        Vector2 axis = Vector2.zero;
        axis.x = Input.GetAxis("DPadX");
        axis.y = Input.GetAxis("DPadY");
        return axis;
    }
    public float GetLeftTriggerAxis()
    {
        return Input.GetAxis("LTrigger");
    }
    public float GetRightTriggerAxis()
    {
        return Input.GetAxis("RTrigger");
    }
    public float GetOculusLeftBumper()
    {
        return Input.GetAxis("LOculusBumper");
    }
    public float GetOculusRightBumper()
    {
        return Input.GetAxis("ROculusBumper");
    }

    //  Buttons
    public bool GetStartButton()
    {
        return Input.GetButtonDown("Start");
    }
    public bool GetBackButton()
    {
        return Input.GetButtonDown("Back");
    }
    public bool GetXButton()
    {
        return Input.GetButtonDown("X");
    }
    public bool GetYButton()
    {
        return Input.GetButtonDown("Y");
    }
    public bool GetAButton()
    {
        return Input.GetButtonDown("A");
    }
    public bool GetBButton()
    {
        return Input.GetButtonDown("B");
    }
    public bool GetLeftBumper()
    {
        return Input.GetButtonDown("LBumper");
    }
    public bool GetRightBumper()
    {
        return Input.GetButton("RBumper");
    }
    public bool GetLeftClick()
    {
        return Input.GetButtonDown("LeftClick");
    }
    public bool GetRightClick()
    {
        return Input.GetButtonDown("RightClick");
    }
    #endregion
}