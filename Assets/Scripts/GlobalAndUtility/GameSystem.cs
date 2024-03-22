using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameSystem : Singleton<GameSystem>
{
    #region OPTIONS
    public static float MouseSensitive = 0.5f;
    public static float GlobalSoundVolume = 0.5f;
    public static float GlobalMusicVolume = 0.5f;
    public static int IsFullScreen = 1;
    #endregion

    private float playerRotDampSmoothTimeValue = 0.025f;
    public float PlayerRotDampSmoothTimeValue { get => playerRotDampSmoothTimeValue; }

    protected override void Awake()
    {
        base.Awake();
        MouseSensitive = 0.5f;
        GlobalSoundVolume = 0.5f;
        GlobalMusicVolume = 0.5f;
        IsFullScreen = 1;
        LoadOptionValues();
    }

    private static void LoadOptionValues()
    {
        if(PlayerPrefs.HasKey("MouseSensitive"))
        {
            MouseSensitive = PlayerPrefs.GetFloat("MouseSensitive");
            GlobalSoundVolume = PlayerPrefs.GetFloat("SoundVolume");
            GlobalMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            IsFullScreen = PlayerPrefs.GetInt("FullScreen");
        }
        else
        {
            PlayerPrefs.SetFloat("MouseSensitive", MouseSensitive);
            PlayerPrefs.SetFloat("SoundVolume", GlobalSoundVolume);
            PlayerPrefs.SetFloat("MusicVolume", GlobalMusicVolume);
            PlayerPrefs.SetInt("FullScreen", IsFullScreen);
        }
    }

    public static void SaveOption()
    {
        PlayerPrefs.SetFloat("MouseSensitive", MouseSensitive);
        PlayerPrefs.SetFloat("SoundVolume", GlobalSoundVolume);
        PlayerPrefs.SetFloat("MusicVolume", GlobalMusicVolume);
        PlayerPrefs.SetInt("FullScreen", IsFullScreen);
    }

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
            case KeyInputs.Interact: return Input.GetKey(KeyCode.F);
            case KeyInputs.Fire: return Input.GetMouseButton(0);
            case KeyInputs.ZoomIn: return Input.GetMouseButton(1);
            case KeyInputs.FreeView: return Input.GetMouseButton(2);
            case KeyInputs.Escape: return Input.GetKey(KeyCode.Escape);
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
            case KeyInputs.Interact: return Input.GetKeyDown(KeyCode.F);
            case KeyInputs.Fire: return Input.GetMouseButtonDown(0);
            case KeyInputs.ZoomIn: return Input.GetMouseButtonDown(1);
            case KeyInputs.FreeView: return Input.GetMouseButtonDown(2);
            case KeyInputs.Escape: return Input.GetKeyDown(KeyCode.Escape);
            default: return false;
        }
    }
}

