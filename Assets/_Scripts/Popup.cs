using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Popup : Singleton<Popup>
{
    public RectTransform popupObject;
    public Image popupBG;
    public TMP_Text popupText;

    public float endImageAlpha = 0.3f;

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.T))
        //     DisplayResult("YOU LOSE");
    }

    //*This stays on screen, for battle result
    public void DisplayMsg(string msg, float fadeInTime, float showTime)
    {
        popupBG.DOFade(0f, 0f);
        popupText.DOFade(0f, 0f);
        popupObject.DOScale(1f, 0f);

        popupBG.DOFade(endImageAlpha, fadeInTime);
        popupText.DOFade(1f, fadeInTime);
        popupText.text = msg;
        popupObject.DOScale(1.2f, showTime);
    }

    //*This will show and hide automatically
    public void DisplayPopup(string msg, float fadeInTime, float showTime)
    {
        popupBG.DOFade(0f, 0f);
        popupText.DOFade(0f, 0f);
        popupObject.DOScale(1f, 0f);

        popupBG.DOFade(endImageAlpha, fadeInTime);
        popupText.DOFade(1f, fadeInTime);
        popupText.text = msg;
        popupObject.DOScale(1.2f, showTime).OnComplete(() => HideAllPopup(fadeInTime));
    }

    public void HideAllPopup(float fadeOutTime)
    {
        popupBG.DOFade(0, fadeOutTime);
        popupText.DOFade(0f, fadeOutTime);
    }
}
