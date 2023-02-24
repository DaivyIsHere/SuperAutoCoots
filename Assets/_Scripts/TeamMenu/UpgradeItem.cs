using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItem : ShopItem
{
    public UpgradeData upgradeData;

    protected override void Start()
    {
        base.Start();
        weaponDisplay.sprite = upgradeData.sprite;
        damageDisplay.transform.parent.gameObject.SetActive(false);
        durabilityDisplay.transform.parent.gameObject.SetActive(false);
    }
}
