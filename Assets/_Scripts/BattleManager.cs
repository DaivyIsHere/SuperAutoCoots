using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : Singleton<BattleManager>
{
    public BattleSide currentSide = BattleSide.Left;

    public UnitController currentLeftUnit;
    public UnitController currentRightUnit;

    public List<UnitData> leftUnits;
    public List<UnitData> rightUnits;

    private int unitIDCounter = 0;

    [Space]
    public Color leftColor;
    public Color rightColor;

    public GameObject UnitPrefabs;

    public Transform startLocationCentor;
    public Transform leftStartLocation;
    public Transform rightStartLocation;
    public float StartDistance;


    [Header("UI Display")]
    public TMP_Text leftAttack;
    public TMP_Text leftHealth;
    public TMP_Text rightAttack;
    public TMP_Text rightHealth;
    

    void Start()
    {

        Vector2 startPosLeft = (Vector2)startLocationCentor.position - new Vector2(StartDistance, 0);
        Vector2 startPosRight = (Vector2)startLocationCentor.position + new Vector2(StartDistance, 0);
        //Vector2 startPosLeft = leftStartLocation.position;
        //Vector2 startPosRight = rightStartLocation.position;

        currentLeftUnit = Instantiate(UnitPrefabs, startPosLeft, Quaternion.identity).GetComponent<UnitController>();
        currentRightUnit = Instantiate(UnitPrefabs, startPosRight, Quaternion.identity).GetComponent<UnitController>();

        currentLeftUnit.IniUnitData(leftUnits[0]);
        currentRightUnit.IniUnitData(rightUnits[0]);

        currentLeftUnit.side = BattleSide.Left;
        currentRightUnit.side = BattleSide.Right;

        currentLeftUnit.gameObject.name = "LeftUnit";
        currentRightUnit.gameObject.name = "RightUnit";

        currentLeftUnit.GetComponentInChildren<SpriteRenderer>().color = leftColor;
        currentRightUnit.GetComponentInChildren<SpriteRenderer>().color = rightColor;

        currentLeftUnit.currentOpponent = currentRightUnit;
        currentRightUnit.currentOpponent = currentLeftUnit;

        
    }

    void Update()
    {
        if(!currentLeftUnit || !currentRightUnit)
            return;

        leftHealth.text = currentLeftUnit.currentHealth.ToString();
        leftAttack.text = currentLeftUnit.unitData.attack.ToString();
        rightHealth.text = currentRightUnit.currentHealth.ToString();
        rightAttack.text = currentRightUnit.unitData.attack.ToString();
    }

    private int GetNewUnitID()
    {
        unitIDCounter++;
        return unitIDCounter;
    }

    public void EndTurn()
    {
        StartCoroutine(SwapTurn());
    }

    private IEnumerator SwapTurn()
    {
        yield return new WaitForEndOfFrame();
        currentSide = currentSide.Opposite();
    }
}

public enum BattleSide
{
    Left,
    Right
}
