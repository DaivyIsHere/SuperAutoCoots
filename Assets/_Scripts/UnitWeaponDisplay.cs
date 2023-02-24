using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnitWeaponDisplay : MonoBehaviour
{
    public UnitController unitController;
    public Transform firstWeaponLocation;
    public float weaponSpacingY;
    public List<WeaponController> weaponControllers;
    public GameObject weaponPref;
    //public Transform weaponIndicator;
    [Header("UI")]
    public Image totalHealthDisplay;
    public TMP_Text totalHealthText;
    public TMP_Text currentWeaponDamageText;

    public int totalMaxDurability = 0;

    private void Start()
    {
        //unitController.OnWeaponChange += UpdateWeaponIndicator;
        unitController.OnWeaponRemove += RemoveWeaponController;
        unitController.OnWeaponAttack += UpdateCurrentWeaponInfo;
    }

    void Update()
    {
        UpdateAvatarDisplay();
    }

    public void AlignAllWeapons()
    {
        for (int i = 0; i < weaponControllers.Count; i++)
        {
            Vector2 pos = GetWeaponPositionByIndex(i);
            weaponControllers[i].targetPos = pos;
            weaponControllers[i].UpdatePosition();
        }
    }

    public Vector2 GetWeaponPositionByIndex(int index)
    {
        return (Vector2)firstWeaponLocation.position + new Vector2(0, weaponSpacingY * index);
    }

    public void IniAllWeapons()
    {
        for (int i = 0; i < unitController.allWeapons.Count; i++)
        {
            WeaponController newWeapon = Instantiate(weaponPref, GetWeaponPositionByIndex(i), Quaternion.identity, transform).GetComponent<WeaponController>();
            newWeapon.weaponData = unitController.allWeapons[i];
            weaponControllers.Add(newWeapon);
        }
        AlignAllWeapons();
        //UpdateWeaponIndicator();
        UpdateCurrentWeaponInfo();
        foreach (var w in weaponControllers)
        {
            totalMaxDurability += w.weaponData.maxDurability;
        }
    }

    public void RemoveWeaponController(int index)
    {
        GameObject weaponToRemove = weaponControllers[index].gameObject;
        weaponControllers.Remove(weaponControllers[index]);
        Destroy(weaponToRemove);
        AlignAllWeapons();
    }

    public void UpdateAvatarDisplay()
    {
        int totalHealth = 0;
        foreach (var w in weaponControllers)
        {
            totalHealth += w.weaponData.durability;
        }

        totalHealthDisplay.fillAmount = (float)totalHealth / (float)totalMaxDurability;
        totalHealthText.text = totalHealth.ToString() + " / " + totalMaxDurability.ToString();
        currentWeaponDamageText.text = unitController.currentWeapon ? unitController.currentWeapon.damage.ToString() : "";
    }

    // public void UpdateWeaponIndicator()
    // {
    //     int currentIndex = unitController.GetCurrentWeaponIndex();
    //     weaponIndicator.DOMove(GetWeaponPositionByIndex(currentIndex), 0.5f).SetEase(Ease.InOutSine).SetLink(weaponIndicator.gameObject);
    // }

    public void UpdateCurrentWeaponInfo()
    {
        int currentIndex = unitController.GetCurrentWeaponIndex();
        int nextIndex = unitController.GetNextWeaponIndex();

        weaponControllers[currentIndex].UpdateTurnUnit(unitController.currentWeaponTurn);

        if (unitController.currentWeaponTurn >= unitController.currentWeapon.useTurn)
            weaponControllers[nextIndex].UpdateTurnUnit(0);
    }
}
