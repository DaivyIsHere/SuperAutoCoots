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

    public void WeaponBroke(WeaponData weaponData)
    {
        print(weaponData.name+" Broke");
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

