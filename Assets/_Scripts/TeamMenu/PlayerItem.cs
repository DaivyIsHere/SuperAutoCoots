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
        //TODO MERGE
        
        //switch
        if (eventData.pointerDrag.GetComponent<PlayerItem>())
        {
            print("drop on other item");
            PlayerItem itemDropped = eventData.pointerDrag.GetComponent<PlayerItem>();
            ItemSlot itemDroppedSlot = itemDropped.slot;

            ((PlayerItemSlot)this.slot).item = itemDropped;
            itemDropped.slot = this.slot;

            ((PlayerItemSlot)itemDroppedSlot).item = this;
            slot = itemDroppedSlot;
            SnapBackToSlot();
        }
    }
}
