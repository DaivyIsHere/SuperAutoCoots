using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameResultDisplay : MonoBehaviour
{
    public TMP_Text textDisplay;

    void Start()
    {
        if (PlayerDataManager.instance.stage > 10)
        {
            textDisplay.text = "Pog You actually win!";
        }
        else
        {
            textDisplay.text = "GAME OVER <br>GG BOIS";
        }
    }
}
