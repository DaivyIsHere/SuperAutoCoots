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
    public Transform weaponIndicator;
    [Header("UI")]
    public Image totalHealthDisplay;
    public TMP_Text totalHealthText;
    public TMP_Text currentWeaponDamageText;

    private void Start()
    {
        unitController.OnWeaponChange += UpdateWeaponIndicator;
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
        UpdateWeaponIndicator();
        UpdateCurrentWeaponInfo();
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
        int totalMaxHealth = 0;
        foreach (var w in weaponControllers)
        {
            totalHealth += w.weaponData.durability;
            totalMaxHealth += w.weaponData.maxDurability;
        }

        totalHealthDisplay.fillAmount = (float)totalHealth / (float)totalMaxHealth;
        totalHealthText.text = totalHealth.ToString() + " / " + totalMaxHealth.ToString();
        currentWeaponDamageText.text = unitController.currentWeapon.damage.ToString();
    }

    public void UpdateWeaponIndicator()
    {
        int currentIndex = unitController.GetCurrentWeaponIndex();
        weaponIndicator.DOMove(GetWeaponPositionByIndex(currentIndex), 0.5f).SetEase(Ease.InOutSine);
    }

    public void UpdateCurrentWeaponInfo()
    {
        int currentIndex = unitController.GetCurrentWeaponIndex();
        for (int i = 0; i < weaponControllers.Count; i++)
        {
            if (i == currentIndex)
                weaponControllers[i].UpdateCurrentWeaponInfo(true, unitController.currentWeaponTurn);
            else
                weaponControllers[i].UpdateCurrentWeaponInfo(false, unitController.currentWeaponTurn);
        }
    }
}
