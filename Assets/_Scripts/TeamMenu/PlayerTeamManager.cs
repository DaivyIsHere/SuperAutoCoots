using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTeamManager : PersistentSingleton<PlayerTeamManager>
{
    public List<TeamWeaponData> weapons;
    public int lives = 3;
}

[Serializable]
public class TeamWeaponData
{
    public string weaponName;//used to get the weaponData SO
    public int totalExp = 0;
    public int additionalAttack = 0;
    public int additionalDurability = 0;
    public int additionalVelocity = 0;

    public TeamWeaponData(string weaponName)
    {
        this.weaponName = weaponName;
    }

    public WeaponData GetWeaponData()
    {
        WeaponDatabase database = Resources.Load<WeaponDatabase>("_SO/WeaponDatabase");
        return database.weaponDic[weaponName];
    }
}

