using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerItemSlot : ItemSlot, IDropHandler
{
    public PlayerItem item;
    public GameObject playerItemPref;
    
    public void OnDrop(PointerEventData eventData)
    {
        //TODO MERGE
        
        //Is shop item, buy
        if (eventData.pointerDrag.GetComponent<ShopItem>())
        {
            ShopItem itemDropped = eventData.pointerDrag.GetComponent<ShopItem>();
            if (TeamBuildManager.instance.BuyWeapon(itemDropped))
            {
                WeaponData droppedWeaponData = itemDropped.weaponData;
                //Destroy the shopItem
                Destroy(itemDropped.gameObject);
                //Spawn the playerItem
                PlayerItem newPlayerItem = Instantiate(playerItemPref, transform.position, Quaternion.identity, TeamBuildManager.instance.itemContainer).GetComponent<PlayerItem>();
                newPlayerItem.slot = this;
                newPlayerItem.weaponData = droppedWeaponData;
                item = newPlayerItem;
            }
        }

        //Is weapon, means this is an empty slot //otherwise it will be dropped on other playerItem instead
        if (eventData.pointerDrag.GetComponent<PlayerItem>())
        {
            if(eventData.pointerDrag.GetComponent<PlayerItem>().slot == this)
                return;
            PlayerItem itemDropped = eventData.pointerDrag.GetComponent<PlayerItem>();
            //print("drop on slot");
            //print("itemDropped : "+ itemDropped.weaponData.name);
            if(item)
            {
                Item originalItem = item;
                //print("oldItem old slot : "+ originalItem.slot);
                //print("newItem old slot : "+ itemDropped.slot);
                originalItem.slot = itemDropped.slot;//assign new slot to old item
                originalItem.SnapBackToSlot();
                //print("oldItem new slot : "+ originalItem.slot);
                ((PlayerItemSlot)itemDropped.slot).item = (PlayerItem)originalItem;//assign old item for newitem's previous slot

                item = itemDropped;
                itemDropped.slot = this;
                //print("newItem new slot : "+ itemDropped.slot);
            }
            else //this is an empty slot
            {
                itemDropped.slot = this;
            }
        }
    }
}
