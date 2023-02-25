using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerItem : Item, IDropHandler
{
    public override void OnClickSellOrLock()
    {
        ((PlayerItemSlot)slot).item = null;
        Destroy(gameObject);
        TeamBuildManager.instance.playerGold += 1;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<PlayerItem>())
        {
            //print("drop on other item");
            PlayerItem itemDropped = eventData.pointerDrag.GetComponent<PlayerItem>();
            if (itemDropped.weaponData.GetOriginalName() == weaponData.GetOriginalName())
            {
                MergeItem(itemDropped);
            }
            else
            {
                //Swap
                SwapSlotWithItem((PlayerItemSlot)this.slot, (PlayerItemSlot)itemDropped.slot);
            }
        }
        else if (eventData.pointerDrag.GetComponent<UpgradeItem>())//need to check before shop item cause upgradeItem inherits from shopItem
        {
            //Check can buy
            if (TeamBuildManager.instance.BuyWeapon(eventData.pointerDrag.GetComponent<UpgradeItem>()))
            {
                UpgradeItem(eventData.pointerDrag.GetComponent<UpgradeItem>());
            }
        }
        else if (eventData.pointerDrag.GetComponent<ShopItem>())
        {
            //Upgrade
            ShopItem itemDropped = eventData.pointerDrag.GetComponent<ShopItem>();
            if (itemDropped.weaponData.GetOriginalName() == weaponData.GetOriginalName())
            {
                //Check can buy
                if (TeamBuildManager.instance.BuyWeapon(itemDropped))
                {
                    MergeItem(itemDropped);
                }
            }
        }
    }

    public void SwapSlotWithItem(PlayerItemSlot selfItemSlot, PlayerItemSlot targetItemSlot)
    {
        PlayerItem targetItem = targetItemSlot.item;
        PlayerItem selfItem = selfItemSlot.item;

        PlayerItemSlot targetItemOldSlot = targetItemSlot;
        PlayerItemSlot selfOldSlot = selfItemSlot;

        if (targetItem == null)
        {
            selfOldSlot.item = null;
            targetItemOldSlot.item = selfItem;
            selfItem.slot = targetItemOldSlot;
            selfItem.SnapBackToSlot();
        }
        else
        {
            targetItemOldSlot.item = selfItem;
            selfOldSlot.item = targetItem;

            targetItem.slot = selfOldSlot;
            selfItem.slot = targetItemOldSlot;

            targetItem.SnapBackToSlot();
            selfItem.SnapBackToSlot();
        }

        if(AudioManager.instance)
            AudioManager.instance.PlayItem();
    }

    public void MergeItem(Item mergeItem)
    {
        weaponData.totalExp += mergeItem.weaponData.totalExp + 1;
        weaponData.additionalDamage += 1;
        weaponData.additionalDurability += 1;
        if (mergeItem is PlayerItem)
        {
            ((PlayerItemSlot)mergeItem.slot).item = null;
        }
        else if (mergeItem is ShopItem)
        {
            ((ShopItemSlot)mergeItem.slot).item = null;
        }
        Destroy(mergeItem.gameObject);
        UpdateStatsDisplay();

        if(AudioManager.instance)
            AudioManager.instance.PlayItem();
    }

    public void UpgradeItem(UpgradeItem upgradeItem)
    {
        weaponData.additionalDamage += upgradeItem.upgradeData.bonusDamage;
        weaponData.additionalDurability += upgradeItem.upgradeData.bonusDurability;
        weaponData.additionalVelocity += upgradeItem.upgradeData.bonusVelocity;
        Destroy(upgradeItem.gameObject);
        UpdateStatsDisplay();

        if(AudioManager.instance)
            AudioManager.instance.PlayItem();
    }

}
