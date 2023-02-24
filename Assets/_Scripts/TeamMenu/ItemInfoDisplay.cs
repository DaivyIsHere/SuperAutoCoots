using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoDisplay : Singleton<ItemInfoDisplay>
{
    public TMP_Text nameDisplay;
    public TMP_Text infoDisplay;
    public TMP_Text extraInfoLeftDisplay;
    public TMP_Text extraInfoRightDisplay;
    public Image itemSprite;

    public void ShowInfo(Item item)
    {
        itemSprite.color = Color.white;
        if (item is PlayerItem)
        {
            itemSprite.sprite = item.weaponData.weaponSprite;
            nameDisplay.text = item.weaponData.GetOriginalName();
            infoDisplay.text = item.weaponData.description;
            extraInfoLeftDisplay.text = "LV : " + (item.weaponData.level + 1).ToString();
            if (item.weaponData.level == 0)
                extraInfoRightDisplay.text = "Level up exp : " + (2 - item.weaponData.totalExp).ToString();
            else if (item.weaponData.level == 1)
                extraInfoRightDisplay.text = "Level up exp : " + (5 - item.weaponData.totalExp).ToString();
            else if (item.weaponData.level == 2)
                extraInfoRightDisplay.text = "Level Maxed";
        }
        else if (item is UpgradeItem)
        {
            UpgradeItem upgradeItem = (UpgradeItem)item;
            itemSprite.sprite = upgradeItem.upgradeData.sprite;
            infoDisplay.text = upgradeItem.upgradeData.description;
            extraInfoLeftDisplay.text = "";
            extraInfoRightDisplay.text = "Buy : 3 gold";
        }
        else if (item is ShopItem)
        {
            itemSprite.sprite = item.weaponData.weaponSprite;
            nameDisplay.text = item.weaponData.name;
            infoDisplay.text = item.weaponData.description;
            extraInfoLeftDisplay.text = "";
            extraInfoRightDisplay.text = "Buy : 3 gold";
        }
    }
}
