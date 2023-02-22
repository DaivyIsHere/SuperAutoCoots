using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TeamBuildManager : Singleton<TeamBuildManager>
{
    [Header("Component")]
    public Canvas mainCanvas;
    public TMP_Text playerGoldText;
    public Button sellLockBtn;
    public TMP_Text sellLockText;
    public Button rerollBtn;
    public Button nextBattleBtn;

    [Header("ItemSlots")]
    public List<PlayerItemSlot> playerSlots;
    public List<ShopItemSlot> shopSlots;
    public GameObject shopItemPref;
    public Transform itemContainer;

    [Header("PlayerData")]
    public int playerGold;

    [Header("Const")]
    public int startGold = 10;
    public int weaponPrice = 3;
    public int rollPrice = 1;

    [Header("AllWeapon")]
    public List<WeaponData> allWeapon;

    public List<Item> allSelectedItem;


    void Start()
    {
        rerollBtn.onClick.AddListener(OnClickReroll);
        nextBattleBtn.onClick.AddListener(OnClickNextBattle);
        sellLockBtn.onClick.AddListener(OnClickSellLockButton);

        playerGold = startGold;
        allWeapon.AddRange(Resources.LoadAll<WeaponData>("_SO/WeaponData"));
        RerollShopItems();
    }

    void Update()
    {
        playerGoldText.text = "Gold : " + playerGold.ToString();

        //Update reroll btn
        if (playerGold >= rollPrice)
            rerollBtn.interactable = true;
        else
            rerollBtn.interactable = false;

        UpdateLockSellButton();
    }

    public void RerollShopItems()
    {
        Canvas.ForceUpdateCanvases();
        foreach (var slot in shopSlots)
        {
            //Check Lock
            if (slot.item && slot.item.isLocked)
            {
                continue;
            }

            //Clear
            if (slot.item)
            {
                Destroy(slot.item.gameObject);
                slot.item = null;
            }
            //Add
            ShopItem newItem = Instantiate(shopItemPref, slot.transform.position, Quaternion.identity, itemContainer).GetComponent<ShopItem>();
            newItem.slot = slot;
            slot.item = newItem;

            //Random weaponData
            int rng = Random.Range(0, allWeapon.Count);
            newItem.weaponData = allWeapon[rng];
        }
    }

    public bool BuyWeapon(ShopItem item)
    {
        if (playerGold >= weaponPrice)
        {
            playerGold -= weaponPrice;
            return true;
        }
        return false;
    }

    public void DeselectAllSelectedItems()
    {
        for (int i = allSelectedItem.Count - 1; i >= 0; i--)
        {
            allSelectedItem[i].OnDeselect();
            allSelectedItem.Remove(allSelectedItem[i]);
        }
    }

    public void UpdateLockSellButton()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DeselectAllSelectedItems();

            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            bool isOverItem = false;
            foreach (var r in raycastResults)
            {
                if (r.gameObject.GetComponent<Item>())
                {
                    Item clickedItem = r.gameObject.GetComponent<Item>();
                    if (clickedItem is PlayerItem)
                        sellLockText.text = "Sell";
                    else if (clickedItem is ShopItem)
                        sellLockText.text = "Lock";

                    clickedItem.OnSelected();
                    sellLockBtn.interactable = true;
                    isOverItem = true;
                    allSelectedItem.Add(clickedItem);
                }
            }

            if (!isOverItem)
            {
                sellLockText.text = "";
                sellLockBtn.interactable = false;
            }
        }
    }

    public void OnClickSellLockButton()
    {
        if (allSelectedItem.Count > 0)
            allSelectedItem[0].OnClickSellOrLock();
    }

    public void OnClickReroll()
    {
        if (playerGold >= rollPrice)
        {
            playerGold -= rollPrice;
            RerollShopItems();
        }
    }

    public void OnClickNextBattle()
    {
        PlayerTeamManager.instance.weapons.Clear();
        foreach (var slot in playerSlots)
        {
            if (slot.item)
            {
                TeamWeaponData teamWeaponData = new TeamWeaponData(slot.item.weaponData.name);
                PlayerTeamManager.instance.weapons.Add(teamWeaponData);
            }
        }
        nextBattleBtn.interactable = false;
        BlackFade.instance.FadeTransition(() => SceneManager.LoadScene("Battle"));
    }
}
