using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameResultDisplay : MonoBehaviour
{
    public TMP_Text textDisplay;
    public Image coots;
    public Sprite loseCoot;
    public Sprite winCoot;

    void Start()
    {
        if (PlayerDataManager.instance.stage > 10)
        {
            textDisplay.text = "Pog You actually win!";
            coots.sprite = winCoot;
        }
        else
        {
            textDisplay.text = "GAME OVER <br>GG BOIS";
            coots.sprite = loseCoot;
        }
    }
}
