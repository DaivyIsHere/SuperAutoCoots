using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Coots/UpgradeData", order = 1)]
public class UpgradeData : ScriptableObject
{
    public Sprite sprite;
    public string description;

    public int bonusDamage = 0;
    public int bonusDurability = 0;
    public int bonusVelocity = 0;
}
