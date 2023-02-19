using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Component")]
    public GameObject UnitPrefabs;

    [Header("Turn info")]
    public bool isRunning = false;
    public BattleSide currentSide = BattleSide.Left;
    public event Action<BattleSide> OnTurnChange;

    [Header("CurrentUnit")]
    public UnitController currentLeftUnit;
    public UnitController currentRightUnit;

    [Header("DefaultTeam")]
    public List<UnitData> defualtLeftUnits;
    public List<UnitData> defaultRightUnits;

    [Header("CurrentTeam")]
    public List<UnitData> leftUnits;
    public List<UnitData> rightUnits;

    [Header("Row")]
    public List<UnitDisplay> leftUnitDisplay;
    public List<UnitDisplay> rightUnitDisplay;

    [Header("PlayerColor")]
    public Color leftColor;
    public Color rightColor;

    [Header("StartLocation")]
    public Transform startLocationCentor;
    public float StartDistance;
    private Vector2 startPosLeft;
    private Vector2 startPosRight;

    [Header("Battle UI Display")]
    public TMP_Text leftAttack;
    public TMP_Text leftHealth;
    public TMP_Text rightAttack;
    public TMP_Text rightHealth;

    private int unitIDCounter = 0;

    void Start()
    {
        IniPlayerUnits();
        DeployPlayerRow();

        startPosLeft = (Vector2)startLocationCentor.position - new Vector2(StartDistance, 0);
        startPosRight = (Vector2)startLocationCentor.position + new Vector2(StartDistance, 0);

        GoNextUnit(BattleSide.Left);
        GoNextUnit(BattleSide.Right);
    }

    void Update()
    {
        if (currentLeftUnit)
        {
            leftHealth.text = currentLeftUnit.unitData.currentHealth.ToString();
            leftAttack.text = currentLeftUnit.unitData.attack.ToString();
        }
        else
        {
            leftHealth.text = "";
            leftAttack.text = "";
        }

        if (currentRightUnit)
        {

            rightHealth.text = currentRightUnit.unitData.currentHealth.ToString();
            rightAttack.text = currentRightUnit.unitData.attack.ToString();
        }
        else
        {
            rightHealth.text = "";
            rightAttack.text = "";
        }
    }

    private void IniPlayerUnits()
    {
        foreach (var u in defualtLeftUnits)
        {
            leftUnits.Add(u);
        }

        foreach (var u in defaultRightUnits)
        {
            rightUnits.Add(u);
        }
    }

    private void DeployPlayerRow()
    {
        //Clear All unitData
        foreach (var ud in leftUnitDisplay)
        {
            ud.unitData = null;
        }
        foreach (var ud in rightUnitDisplay)
        {
            ud.unitData = null;
        }

        //Deploy
        for (int i = 1; i < leftUnits.Count; i++)
        {
            leftUnitDisplay[i - 1].unitData = leftUnits[i];
        }
        for (int i = 1; i < rightUnits.Count; i++)
        {
            rightUnitDisplay[i - 1].unitData = rightUnits[i];
        }
    }

    private void GoNextUnit(BattleSide side)
    {
        if (side == BattleSide.Left)
        {
            currentLeftUnit = Instantiate(UnitPrefabs, startPosLeft, Quaternion.identity).GetComponent<UnitController>();
            currentLeftUnit.IniUnitData(leftUnits[0]);
            currentLeftUnit.side = BattleSide.Left;
            currentLeftUnit.gameObject.name = "LeftUnit";
            currentLeftUnit.GetComponentInChildren<SpriteRenderer>().color = leftColor;
        }
        else
        {
            currentRightUnit = Instantiate(UnitPrefabs, startPosRight, Quaternion.identity).GetComponent<UnitController>();
            currentRightUnit.IniUnitData(rightUnits[0]);
            currentRightUnit.side = BattleSide.Right;
            currentRightUnit.gameObject.name = "RightUnit";
            currentRightUnit.GetComponentInChildren<SpriteRenderer>().color = rightColor;
        }
    }

    private bool CheckGameOver()
    {
        if (leftUnits.Count == 0 || rightUnits.Count == 0)
        {
            isRunning = false;

            if (leftUnits.Count == 0)
                print("GameOver, Right Win");
            else if (rightUnits.Count == 0)
                print("GameOver, Left Win");

            return true;
        }
        else
            return false;
    }

    public void UnitDefeated(UnitController unit)
    {
        if (unit.side == BattleSide.Left)
        {
            startPosLeft = unit.transform.position;
            leftUnits.RemoveAt(0);
            Destroy(unit.gameObject);

            if (CheckGameOver())
                return;

            DeployPlayerRow();
            StartCoroutine(NextUnitWithDelay(unit.side));
        }
        else
        {
            startPosRight = unit.transform.position;
            rightUnits.RemoveAt(0);
            Destroy(unit.gameObject);

            if (CheckGameOver())
                return;

            DeployPlayerRow();
            StartCoroutine(NextUnitWithDelay(unit.side));
        }
    }

    private IEnumerator NextUnitWithDelay(BattleSide side)
    {
        if (side == BattleSide.Left)
            currentLeftUnit = null;
        else
            currentRightUnit = null;
        yield return new WaitForSeconds(1f);
        print("goNext");
        GoNextUnit(side);
    }

    public UnitController GetOpponentUnit(BattleSide side)
    {
        if (side == BattleSide.Left)
            return currentRightUnit;
        else
            return currentLeftUnit;
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

    private int GetNewUnitID()
    {
        unitIDCounter++;
        return unitIDCounter;
    }

}

public enum BattleSide
{
    Left,
    Right
}
