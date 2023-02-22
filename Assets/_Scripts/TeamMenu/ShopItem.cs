using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : Item
{
    public GameObject LockImage;
    public bool isLocked = false;
    
    public override void OnClickSellOrLock()
    {
        isLocked = !isLocked;
        UpdateLockDisplay();
    }

    public void UpdateLockDisplay()
    {
        LockImage.SetActive(isLocked);
    }
}
