using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Coots/ItemDatabase", order = 1)]
public class ItemDatabase : ScriptableObject
{
    public Dictionary<string, WeaponData> weaponDic = new Dictionary<string, WeaponData>();
    public Dictionary<string, UpgradeData> upgradeDic = new Dictionary<string, UpgradeData>();

    private void OnValidate() 
    {
        weaponDic.Clear();
        WeaponData[] allWeaponData = Resources.LoadAll<WeaponData>("_SO/WeaponData");
        for (int i = 0; i < allWeaponData.Length; i++)
        {
            weaponDic.Add(allWeaponData[i].name, allWeaponData[i]);
        }

        upgradeDic.Clear();
        UpgradeData[] allUpgradeData = Resources.LoadAll<UpgradeData>("_SO/UpgradeData");
        for (int i = 0; i < allUpgradeData.Length; i++)
        {
            upgradeDic.Add(allUpgradeData[i].name, allUpgradeData[i]);
        }
    }
}
