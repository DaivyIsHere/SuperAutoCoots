using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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
            leftUnit.allWeapons.Add(w.GetWeaponDataNewInstance());
        leftUnit.currentWeapon = leftUnit.allWeapons[0];
        leftUnit.unitWeaponDisplay.IniAllWeapons();

        //Right //TODO enemy weapons
        TeamWeaponData teamWeaponData = new TeamWeaponData("Spear", 0);
        rightUnit.allWeapons.Add(Instantiate(teamWeaponData.GetWeaponDataOriginal()));
        // foreach (var w in PlayerTeamManager.instance.weapons) 
        //     rightUnit.allWeapons.Add(Instantiate(w.GetWeaponData()));
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

    public void GameOver(BattleSide side)
    {
        isRunning = false;
        //Debug.LogWarning("GAME OVER!!");

        if (side == BattleSide.Left)
        {
            //player lose
            Debug.LogWarning("YOU LOSE!!");
            PlayerTeamManager.instance.lives -= 1;
        }
        else if (side == BattleSide.Right)
        {
            //player win
            Debug.LogWarning("YOU WIN!!");
        }

        StartCoroutine(EndBattleAnimation());
    }

    private IEnumerator EndBattleAnimation()
    {
        if (PlayerTeamManager.instance.lives <= 0)
        {
            yield return new WaitForSeconds(1f);
            Popup.instance.DisplayMsg("YOU LOSE", 1, 2);
            yield return new WaitForSeconds(2.5f);
            BlackFade.instance.FadeTransition(() => SceneManager.LoadScene("GameOver"));
        }
        else
        {
            yield return new WaitForSeconds(1f);
            Popup.instance.DisplayMsg("YOU WIN", 1, 2);
            yield return new WaitForSeconds(2.5f);
            PlayerTeamManager.instance.level += 1;
            BlackFade.instance.FadeTransition(() => SceneManager.LoadScene("TeamBuilding"));
        }

        yield return 0;
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

