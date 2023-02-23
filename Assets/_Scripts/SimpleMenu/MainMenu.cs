using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        BlackFade.instance.FadeTransition(() =>  SceneManager.LoadScene("TeamBuilding"));
    }
    
    public void Quit()
    {
        print("quit");
        BlackFade.instance.FadeTransition(() =>  Application.Quit());
    }
}
