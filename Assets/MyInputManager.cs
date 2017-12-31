using UnityEngine;


public class MyInputManager : MonoBehaviour
{

    [SerializeField] bool isOculusTouchConnected;

    ////  Oculus Touch Properties
    ////  Axis
    //private Vector2 leftThumbstick;
    //private Vector2 rightThumbstick;
    ////  Buttons


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

    #region Oculus Input
    private void OculusInput()
    {
        // returns true if the primary button was pressed this frame.
        bool a = OVRInput.GetDown(OVRInput.Button.One);     //  A
        bool b = OVRInput.GetDown(OVRInput.Button.Two);     //  B
        bool x = OVRInput.GetDown(OVRInput.RawButton.X);    //  X
        bool y = OVRInput.GetDown(OVRInput.RawButton.Y);    //  Y

        // returns a Vector2 of the thumbstick’s current state. 
        // (X/Y range of -1.0f to 1.0f)
        Vector2 primaryThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 secondaryThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // returns true if the primary thumbstick is currently pressed (clicked as a button)
        bool primaryStickClick = OVRInput.Get(OVRInput.Button.PrimaryThumbstick);
        bool secondaryStickClick = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);

        // returns a float of the secondary (typically the Right) index finger trigger’s current state.  
        // (range of 0.0f to 1.0f)
        float leftIndexTrigger = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
        float secondaryIndexTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);

        // returns a float of the Hand Trigger’s current state on the Left Oculus Touch controller.
        float leftHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);

        // returns a float of the Hand Trigger’s current state on the Right Oculus Touch controller.
        float rightHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
    }
    #endregion

    #region Xbox Input
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