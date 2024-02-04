using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameSystem : Singleton<GameSystem>
{
    private float mouseSensitive = 3.0f;
    public float MouseSensitive { get => mouseSensitive; }
    private float playerRotDampSmoothTimeValue = 0.025f;
    public float PlayerRotDampSmoothTimeValue { get => playerRotDampSmoothTimeValue; }

    public static bool GetKey(KeyInputs key)
    {
        switch(key)
        {
            case KeyInputs.MoveRight: return Input.GetKey(KeyCode.D);
            case KeyInputs.MoveLeft: return Input.GetKey(KeyCode.A);
            case KeyInputs.MoveFoward: return Input.GetKey(KeyCode.W);
            case KeyInputs.MoveBack: return Input.GetKey(KeyCode.S);
            case KeyInputs.Jump: return Input.GetKey(KeyCode.Space);
            case KeyInputs.Dash: return Input.GetKey(KeyCode.LeftShift);
            case KeyInputs.Reload: return Input.GetKey(KeyCode.R);
            case KeyInputs.Fire: return Input.GetMouseButton(0);
            case KeyInputs.ZoomIn: return Input.GetMouseButton(1);
            case KeyInputs.FreeView: return Input.GetMouseButton(2);
            default : return false;
        }
    }

    public static bool GetKeyPressed(KeyInputs key)
    {
        switch (key)
        {
            case KeyInputs.MoveRight: return Input.GetKeyDown(KeyCode.D);
            case KeyInputs.MoveLeft: return Input.GetKeyDown(KeyCode.A);
            case KeyInputs.MoveFoward: return Input.GetKeyDown(KeyCode.W);
            case KeyInputs.MoveBack: return Input.GetKeyDown(KeyCode.S);
            case KeyInputs.Jump: return Input.GetKeyDown(KeyCode.Space);
            case KeyInputs.Dash: return Input.GetKeyDown(KeyCode.LeftShift);
            case KeyInputs.Reload: return Input.GetKeyDown(KeyCode.R);
            case KeyInputs.Fire: return Input.GetMouseButtonDown(0);
            case KeyInputs.ZoomIn: return Input.GetMouseButtonDown(1);
            case KeyInputs.FreeView: return Input.GetMouseButtonDown(2);
            default: return false;
        }
    }

}
