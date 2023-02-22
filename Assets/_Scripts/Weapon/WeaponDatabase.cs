using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponDatabase", menuName = "Coots/WeaponDatabase", order = 1)]
public class WeaponDatabase : ScriptableObject
{
    public Dictionary<string, WeaponData> weaponDic = new Dictionary<string, WeaponData>();

    private void OnValidate() 
    {
        weaponDic.Clear();
        WeaponData[] allWeaponData = Resources.LoadAll<WeaponData>("_SO/WeaponData");
        for (int i = 0; i < allWeaponData.Length; i++)
        {
            weaponDic.Add(allWeaponData[i].name, allWeaponData[i]);
        }
    }
}
