using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    public void Play()
    {
        if(PlayerDataManager.instance)
            PlayerDataManager.instance.ResetData();
        BlackFade.instance.FadeTransition(() => SceneManager.LoadScene("TeamBuilding"));
    }

    public void Quit()
    {
        print("quit");
        BlackFade.instance.FadeTransition(() => Application.Quit());
    }
}
