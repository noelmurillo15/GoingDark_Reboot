using UnityEngine;


public class MyInputManager : MonoBehaviour
{

    [SerializeField] bool isOculusTouchConnected;

    //  Xbox controller properties
    //  Axis
    [SerializeField] float LeftJoystickX = 0f;
    [SerializeField] float LeftJoystickY = 0f;
    [SerializeField] float RightJoystickX = 0f;
    [SerializeField] float RightJoystickY = 0f;
    [SerializeField] float LeftTrigger = 0f;
    [SerializeField] float RightTrigger = 0f;
    [SerializeField] float DPadX = 0f;
    [SerializeField] float DPadY = 0f;

    ////  Oculus Touch Properties
    ////  Axis
    //private Vector2 leftThumbstick;
    //private Vector2 rightThumbstick;
    ////  Buttons


    void Awake()
    {
        isOculusTouchConnected = OVRInput.IsControllerConnected(OVRInput.Controller.Active);
        Debug.Log("Oculus Touch Controllers Connected : " + isOculusTouchConnected);
    }

    public bool IsOculusTouchConnected() {
        return isOculusTouchConnected;
    }

    public Vector2 GetLeftJoystickAxis() {
        Vector2 axis = Vector2.zero;
        LeftJoystickX = Input.GetAxis("LeftJoystickX");
        LeftJoystickY = Input.GetAxis("LeftJoystickY");    
        axis.x = LeftJoystickX;
        axis.y = LeftJoystickY;
        return axis;
    }
    public Vector2 GetRightJoystickAxis() {
        Vector2 axis = Vector2.zero;
        RightJoystickX = Input.GetAxis("RightJoystickX");
        RightJoystickY = Input.GetAxis("RightJoystickY");
        axis.x = RightJoystickX;
        axis.y = RightJoystickY;
        return axis;
    }
    public Vector2 GetDPadAxis() {
        Vector2 axis = Vector2.zero;
        DPadX = Input.GetAxis("DPadX");
        DPadY = Input.GetAxis("DPadY");
        axis.x = DPadX;
        axis.y = DPadY;
        return axis;
    }

    public float GetLeftTriggerAxis() {
        LeftTrigger = Input.GetAxis("LTrigger");
        return LeftTrigger;
    }
    public float GetRightTriggerAxis()
    {
        RightTrigger = Input.GetAxis("RTrigger");
        return RightTrigger;
    }

    public bool GetStartButton()
    {
        if (Input.GetButtonDown("Start"))
            Debug.Log("Start Worked");
        return Input.GetButtonDown("Start");
    }
    public bool GetBackButton()
    {
        if (Input.GetButtonDown("Back"))
            Debug.Log("Back Worked");
        return Input.GetButtonDown("Back");
    }
    public bool GetXButton()
    {
        if (Input.GetButtonDown("X"))
            Debug.Log("X Worked");
        return Input.GetButtonDown("X");
    }
    public bool GetYButton()
    {
        if (Input.GetButtonDown("Y"))
            Debug.Log("Y Worked");
        return Input.GetButtonDown("Y");
    }
    public bool GetAButton()
    {
        if (Input.GetButtonDown("A"))
            Debug.Log("A Worked");
        return Input.GetButtonDown("A");
    }
    public bool GetBButton()
    {
        if (Input.GetButtonDown("B"))
            Debug.Log("B Worked");
        return Input.GetButtonDown("B");
    }
    public bool GetLeftBumper()
    {
        if (Input.GetButtonDown("LBumper"))
            Debug.Log("LBumper Worked");
        return Input.GetButtonDown("LBumper");
    }
    public bool GetRightBumper()
    {
        if (Input.GetButtonDown("RBumper"))
            Debug.Log("RBumper Worked");
        return Input.GetButton("RBumper");
    }
    public bool GetLeftClick()
    {
        if (Input.GetButtonDown("LeftClick"))
            Debug.Log("LeftClick Worked");
        return Input.GetButtonDown("LeftClick");
    }
    public bool GetRightClick()
    {
        if (Input.GetButtonDown("RightClick"))
            Debug.Log("RightClick Worked");
        return Input.GetButtonDown("RightClick");
    }
}