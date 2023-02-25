using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public Button mainMenuBtn;
    public Button optionBtn;
    public Button closeBtn;
    public Slider volumeSlider;
    public GameObject optionMenuPanel;

    private void Awake()
    {
        optionBtn.onClick.AddListener(ToggleMenu);
        closeBtn.onClick.AddListener(ToggleMenu);
        mainMenuBtn.onClick.AddListener(GoMainMenu);

    }

    private void Start()
    {
        if (AudioManager.instance)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.3f);
            volumeSlider.onValueChanged.AddListener(SetVolumeSetting);
            AudioManager.instance.GetComponent<AudioSource>().volume = volumeSlider.value;
        }
    }

    public void SetVolumeSetting(float value)
    {
        AudioManager.instance.GetComponent<AudioSource>().volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }

    public void ToggleMenu()
    {
        optionMenuPanel.SetActive(!optionMenuPanel.activeSelf);
    }

    public void GoMainMenu()
    {
        BlackFade.instance.FadeTransition(() => SceneManager.LoadScene("MainMenu"));
    }
}
