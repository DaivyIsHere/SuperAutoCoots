using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTeamManager : PersistentSingleton<PlayerTeamManager>
{
    public List<TeamWeaponData> weapons;
    public int lives = 3;
    public int level = 1;
}

[Serializable]
public class TeamWeaponData
{
    public string weaponName;//used to get the weaponData SO
    public int totalExp = 0;
    public int additionalDamage = 0;
    public int additionalDurability = 0;
    public int additionalVelocity = 0;

    public int currentSlotIndex = 0;

    public TeamWeaponData(string weaponName, int currentSlotIndex)
    {
        this.weaponName = weaponName;
        this.currentSlotIndex = currentSlotIndex;
    }

    public TeamWeaponData(WeaponData weaponData, int currentSlotIndex)
    {
        weaponName = weaponData.GetOriginalName();
        totalExp = weaponData.totalExp;
        additionalDamage= weaponData.additionalDamage;
        additionalDurability = weaponData.additionalDurability;
        additionalVelocity = weaponData.additionalVelocity;
        this.currentSlotIndex = currentSlotIndex;
    }

    public WeaponData GetWeaponDataOriginal()
    {
        WeaponDatabase database = Resources.Load<WeaponDatabase>("_SO/WeaponDatabase");
        return database.weaponDic[weaponName];
    }

    public WeaponData GetWeaponDataNewInstance()
    {
        WeaponDatabase database = Resources.Load<WeaponDatabase>("_SO/WeaponDatabase");
        WeaponData weaponData = MonoBehaviour.Instantiate(database.weaponDic[weaponName]);
        weaponData.totalExp = totalExp;
        weaponData.additionalDamage = additionalDamage;
        weaponData.additionalDurability = additionalDurability;
        weaponData.additionalVelocity = additionalVelocity;
        weaponData.durability = weaponData.maxDurability;
        return weaponData;
    }
}

