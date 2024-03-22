using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPauseMenuManager : MonoBehaviour
{
    #region DETAIL_OPTIONS
    public GameObject OptionMenu;
    public GameObject RestartCheck;
    public GameObject QuitCheck;
    #endregion

    #region OPTION
    public Slider MusicVolumeSlider;
    public Slider SoundVolumeSlider;
    public Slider MouseSensitiveSlider;
    public Toggle FullScreenToggle;
    #endregion
    public bool IsPaused;
    public GameObject PauseMenu;
    public GameObject DetailMenu;
    public GameObject BlackBackground;

    private void Update()
    {
        EscapeKeyDown();
        OptionSet();
    }

    private void EscapeKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !UIDialogControl.Instance.InConversation)
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0.0f;
                BlackBackground.SetActive(true);
                PauseMenu.SetActive(true);
            }
            else
            {
                if (DetailMenu != null)
                {
                    PauseMenu.SetActive(true);
                    DetailMenu.SetActive(false);
                    DetailMenu = null;
                }
                else
                {
                    Time.timeScale = 1.0f;
                    BlackBackground.SetActive(false);
                    PauseMenu.SetActive(false);
                }
            }
        }
    }

    private void OptionSet()
    {
        if(DetailMenu == OptionMenu)
        {
            GameSystem.GlobalMusicVolume = MusicVolumeSlider.value;
            GameSystem.GlobalSoundVolume = SoundVolumeSlider.value;
            GameSystem.MouseSensitive = MouseSensitiveSlider.value;
            GameSystem.IsFullScreen = FullScreenToggle.isOn == true ? 1 : 0;
            GameSystem.SaveOption();

            if (GameSystem.IsFullScreen == 1)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;
            }
        }
    }
    

    public void ResumeButtonClick()
    {
        if (DetailMenu == null)
        {
            Time.timeScale = 1.0f;
            BlackBackground.SetActive(false);
            PauseMenu.SetActive(false);
        }
    }
    public void OptionButtonClick()
    {
        if (DetailMenu == null)
        {
            PauseMenu.SetActive(false);
            MusicVolumeSlider.value = GameSystem.GlobalMusicVolume;
            SoundVolumeSlider.value = GameSystem.GlobalSoundVolume;
            MouseSensitiveSlider.value = GameSystem.MouseSensitive;
            FullScreenToggle.isOn = GameSystem.IsFullScreen == 1 ? true : false;
            DetailMenu = OptionMenu;
            DetailMenu.SetActive(true);
        }
    }
    public void RestartButtonClick()
    {
        if (DetailMenu == null)
        {
            PauseMenu.SetActive(false);
            DetailMenu = RestartCheck;
            DetailMenu.SetActive(true);
        }
    }
    public void QuitButtonClick()
    {
        if (DetailMenu == null)
        {
            PauseMenu.SetActive(false);
            DetailMenu = QuitCheck;
            DetailMenu.SetActive(true);
        }
    }

    public void CheckYes()
    {
        if(DetailMenu == RestartCheck)
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(0);
        }
        else if(DetailMenu == QuitCheck)
        {
            Application.Quit();
        }
    }

    public void CheckNoOrBack()
    {
        DetailMenu.SetActive(false);
        PauseMenu.SetActive(true);
        DetailMenu = null;
    }
}
