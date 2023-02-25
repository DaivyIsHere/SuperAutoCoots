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

    void Start()
    {
        //SetUpPlayerTeam();//Left
        //SetUpEnemyTeam();//Right
        TestBattle();

        //? Disable On Webgl and just paste the component data to it, and hit build.
        //Save Player Team to StageData
        //StageManager.instance.CollectPlayerTeamData();//We collect here because we dont want to fight ourself

    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.N))
        //     SceneManager.LoadScene("GameOver");
    }

    public void TestBattle()
    {
        leftUnit.allWeapons.Add(new TeamWeaponData("Fish", 0).GetWeaponDataNewInstance());
        leftUnit.allWeapons.Add(new TeamWeaponData("Lopunny's Costume", 0).GetWeaponDataNewInstance());
        leftUnit.allWeapons.Add(new TeamWeaponData("King's Boot", 1).GetWeaponDataNewInstance());
        leftUnit.allWeapons.Add(new TeamWeaponData("Magicbook but you can't read", 1).GetWeaponDataNewInstance());
        leftUnit.currentWeapon = leftUnit.allWeapons[0];
        leftUnit.unitWeaponDisplay.IniAllWeapons();

        rightUnit.allWeapons.Add(new TeamWeaponData("Shield", 0).GetWeaponDataNewInstance());
        rightUnit.allWeapons.Add(new TeamWeaponData("Fist", 1).GetWeaponDataNewInstance());
        rightUnit.currentWeapon = rightUnit.allWeapons[0];
        rightUnit.unitWeaponDisplay.IniAllWeapons();
    }

    public void StartBattle()
    {
        isRunning = true;
        OnTurnChange?.Invoke(BattleSide.Left);
    }

    public void SetUpPlayerTeam()
    {
        foreach (var w in PlayerDataManager.instance.teamData.weapons)
            leftUnit.allWeapons.Add(w.GetWeaponDataNewInstance());
        leftUnit.currentWeapon = leftUnit.allWeapons[0];
        leftUnit.unitWeaponDisplay.IniAllWeapons();
    }

    public void SetUpEnemyTeam()
    {
        StageData stageData = StageManager.instance.allStageData.GetStageDataByStage(PlayerDataManager.instance.stage);
        if (stageData == null)
        {
            Debug.LogError("NO STAGE DATA FOR STAGE : " + PlayerDataManager.instance.stage);
            return;
        }
        TeamData randomTeam = stageData.GetRandomTeamData();
        foreach (var w in randomTeam.weapons)
            rightUnit.allWeapons.Add(w.GetWeaponDataNewInstance());
        rightUnit.currentWeapon = rightUnit.allWeapons[0];
        rightUnit.unitWeaponDisplay.IniAllWeapons();
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
            //Debug.LogWarning("YOU LOSE!!");
            PlayerDataManager.instance.lives -= 1;
        }
        else if (side == BattleSide.Right)
        {
            //player win
            //Debug.LogWarning("YOU WIN!!");
        }

        StartCoroutine(EndBattleAnimation(side));
    }

    private IEnumerator EndBattleAnimation(BattleSide side)
    {
        yield return new WaitForSeconds(1f);
        if (side == BattleSide.Left)
            Popup.instance.DisplayMsg("YOU LOSE", 1, 2);
        else if (side == BattleSide.Right)
            Popup.instance.DisplayMsg("YOU WIN", 1, 2);
        yield return new WaitForSeconds(2.5f);


        if (PlayerDataManager.instance.lives <= 0)
        {
            BlackFade.instance.FadeTransition(() => SceneManager.LoadScene("GameOver"));
        }
        else
        {
            if (PlayerDataManager.instance.stage >= 10)
            {
                PlayerDataManager.instance.stage += 1;
                BlackFade.instance.FadeTransition(() => SceneManager.LoadScene("GameOver"));
            }
            else
            {
                PlayerDataManager.instance.stage += 1;
                BlackFade.instance.FadeTransition(() => SceneManager.LoadScene("TeamBuilding"));
            }
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

