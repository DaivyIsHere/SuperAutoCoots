using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Coots/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    //*Static
    [Header("Ability")]
    public UnitAbilityBase ability;

    [Header("Stats")]
    public int attack = 1;
    public int maxhealth = 5;
    public float size = 1f;

    [Header("Attack")]
    public float attackVelocity = 10f;//magnitude
    public float attackDuraing = 0.5f;
    [Header("Knockback")]

    [Range(0f,1f)] public float knockBackResis = 0f;

    //*dynamic
    public int currentHealth;

    private void OnEnable() 
    {
        currentHealth = maxhealth;
        // if(BattleManager.instance)
        //     ability.IniAbility();
    }
}
