using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Turn info")]
    public bool isRunning;
    public BattleSide currentSide;
    public event Action<BattleSide> OnTurnChange;

    [Header("CurrentUnit")]
    public UnitController leftUnit;
    public UnitController rightUnit;

    //[Header("Weapons")]
    //public PlayerTeamData leftTeamData;
    //public PlayerTeamData rightTeamData;
    //public List<WeaponData> leftDefaultWeapons;
    //public List<WeaponData> rightDefaultWeapons;

    void Start() 
    {
        //Left
        foreach (var w in PlayerTeamManager.instance.weapons)
            leftUnit.allWeapons.Add(Instantiate(w.GetWeaponData()));
        leftUnit.currentWeapon = leftUnit.allWeapons[0];
        leftUnit.unitWeaponDisplay.IniAllWeapons();

        //Right
        foreach (var w in PlayerTeamManager.instance.weapons) //TODO enemy weapons
            rightUnit.allWeapons.Add(Instantiate(w.GetWeaponData()));
        rightUnit.currentWeapon = rightUnit.allWeapons[0];
        rightUnit.unitWeaponDisplay.IniAllWeapons();
    }

    public void StartBattle()
    {
        isRunning = true;
        OnTurnChange?.Invoke(BattleSide.Left);
    }
    
    public void ChangeTurn()
    {
        if (!isRunning)
            return;

        currentSide = currentSide.Opposite();
        OnTurnChange?.Invoke(currentSide);
    }

    public void WeaponBroke(WeaponData weaponData, UnitController unit)
    {
        //print(weaponData.name+" Broke. "+ unit.name);
    }

    public UnitController GetOpponentUnit(BattleSide side)
    {
        if (side == BattleSide.Left)
            return rightUnit;
        else
            return leftUnit;
    }
}

public enum BattleSide
{
    Left,
    Right
}

