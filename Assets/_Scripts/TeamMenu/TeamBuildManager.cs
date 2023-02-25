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
    public TMP_Text playerLivesText;
    public TMP_Text playerStageText;
    public Button sellLockBtn;
    public TMP_Text sellLockText;
    public Button rerollBtn;
    public Button nextBattleBtn;

    [Header("ItemSlots")]
    public List<PlayerItemSlot> playerSlots;
    public List<ShopItemSlot> shopSlots;
    public List<ShopItemSlot> upgradeSlots;
    public GameObject playerItemPref;
    public GameObject shopItemPref;
    public GameObject upgradeItemPref;
    public Transform itemContainer;

    [Header("PlayerData")]
    public int playerGold;

    [Header("Const")]
    public int startGold = 10;
    public int weaponPrice = 3;
    public int upgradePrice = 3;
    public int rollPrice = 1;

    [Header("AllWeapon")]
    public List<WeaponData> allWeapon;
    public List<UpgradeData> allUpgrades;

    public List<Item> allSelectedItem;


    void Start()
    {
        rerollBtn.onClick.AddListener(OnClickReroll);
        nextBattleBtn.onClick.AddListener(OnClickNextBattle);
        sellLockBtn.onClick.AddListener(OnClickSellLockButton);

        playerGold = startGold;
        playerLivesText.text = "Lives : " + PlayerDataManager.instance.lives.ToString();
        playerStageText.text = "Stage : " + PlayerDataManager.instance.stage.ToString();
        allWeapon.AddRange(Resources.LoadAll<WeaponData>("_SO/WeaponData"));
        allUpgrades.AddRange(Resources.LoadAll<UpgradeData>("_SO/UpgradeData"));
        IniAllPlayerWeapon();
        IniAllPlayerLockedShopItem();
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

    public void IniAllPlayerWeapon()
    {
        Canvas.ForceUpdateCanvases();
        foreach (var w in PlayerDataManager.instance.teamData.weapons)
        {
            PlayerItem newItem = Instantiate(playerItemPref, playerSlots[w.currentSlotIndex].transform.position, Quaternion.identity, itemContainer).GetComponent<PlayerItem>();
            newItem.weaponData = w.GetWeaponDataNewInstance();
            newItem.slot = playerSlots[w.currentSlotIndex];
            playerSlots[w.currentSlotIndex].item = newItem;
        }
    }

    public void IniAllPlayerLockedShopItem()
    {
        Canvas.ForceUpdateCanvases();
        ItemDatabase database = Resources.Load<ItemDatabase>("_SO/ItemDatabase");
        foreach (KeyValuePair<int, string> entry in PlayerDataManager.instance.lockedShopWeapon)
        {
            ShopItem newItem = Instantiate(shopItemPref, shopSlots[entry.Key].transform.position, Quaternion.identity, itemContainer).GetComponent<ShopItem>();
            newItem.slot =  shopSlots[entry.Key];
            shopSlots[entry.Key].item = newItem;
            newItem.weaponData = database.weaponDic[entry.Value];
            newItem.isLocked = true;
            newItem.UpdateLockDisplay();
        }
        foreach (KeyValuePair<int, string> entry in PlayerDataManager.instance.lockedShopUpgrade)
        {
            UpgradeItem newItem = Instantiate(upgradeItemPref, upgradeSlots[entry.Key].transform.position, Quaternion.identity, itemContainer).GetComponent<UpgradeItem>();
            newItem.slot =  upgradeSlots[entry.Key];
            upgradeSlots[entry.Key].item = newItem;
            newItem.upgradeData = database.upgradeDic[entry.Value];
            newItem.isLocked = true;
            newItem.UpdateLockDisplay();
        }
    }

    public void RerollShopItems()
    {
        Canvas.ForceUpdateCanvases();
        foreach (var slot in shopSlots)
        {
            //Check Lock
            if (slot.item && slot.item.isLocked)
                continue;

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
        foreach (var slot in upgradeSlots)
        {
            //Check Lock
            if (slot.item && slot.item.isLocked)
                continue;

            //Clear
            if (slot.item)
            {
                Destroy(slot.item.gameObject);
                slot.item = null;
            }
            //Add
            UpgradeItem newItem = Instantiate(upgradeItemPref, slot.transform.position, Quaternion.identity, itemContainer).GetComponent<UpgradeItem>();
            newItem.slot = slot;
            slot.item = newItem;

            //Random upgradeData
            int rng = Random.Range(0, allUpgrades.Count);
            newItem.upgradeData = allUpgrades[rng];
        }
    }

    public bool PlayerHasWeapon()
    {
        int totalWeaponCount = 0;
        foreach (var slot in playerSlots)
        {
            if (slot.item)
                totalWeaponCount += 1;
        }

        if (totalWeaponCount <= 0)
            return false;
        else
            return true;
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

    public bool BuyUpgrade(UpgradeItem item)
    {
        if (playerGold >= upgradePrice)
        {
            playerGold -= upgradePrice;
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

    public void SavePlayerTeam()
    {
        PlayerDataManager.instance.teamData.weapons.Clear();
        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (playerSlots[i].item)
            {
                TeamWeaponData teamWeaponData = new TeamWeaponData(playerSlots[i].item.weaponData, i);//playerSlots[i].item.weaponData.GetOriginalName(), i);
                PlayerDataManager.instance.teamData.weapons.Add(teamWeaponData);
            }
        }
    }

    public void SavePlayerLockedShopItem()
    {
        PlayerDataManager.instance.lockedShopWeapon.Clear();
        PlayerDataManager.instance.lockedShopUpgrade.Clear();

        for (int i = 0; i < shopSlots.Count; i++)
        {
            if (shopSlots[i].item && shopSlots[i].item.isLocked)
            {
                PlayerDataManager.instance.lockedShopWeapon.Add(i, shopSlots[i].item.weaponData.name);
            }
        }
        for (int i = 0; i < upgradeSlots.Count; i++)
        {
            if (upgradeSlots[i].item && upgradeSlots[i].item.isLocked)
            {
                PlayerDataManager.instance.lockedShopUpgrade.Add(i, ((UpgradeItem)upgradeSlots[i].item).upgradeData.name);
            }
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
        //Check has weapon
        if (!PlayerHasWeapon())
        {
            Popup.instance.DisplayPopup("Drag a weapon from the shop to your team.", 1f, 1f);
            return;
        }

        //Save Player team
        SavePlayerTeam();

        //Save Locked ShopItems
        SavePlayerLockedShopItem();

        //To Battle Scene
        nextBattleBtn.interactable = false;
        BlackFade.instance.FadeTransition(() => SceneManager.LoadScene("Battle"));
    }

}
