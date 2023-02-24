using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WeaponController : MonoBehaviour
{
    [Header("Component")]
    public WeaponData weaponData;
    public SpriteRenderer weaponSprite;
    public TextMeshPro damageText;
    public TextMeshPro durabilityText;
    public Image durabilityDisplay;
    public Transform turnDisplay;
    public GameObject turnUnitPref;

    public List<GameObject> allTurnUnits;

    public Vector2 targetPos;

    private void Start()
    {

    }

    void Update()
    {
        if (!weaponData)
        {
            damageText.transform.parent.gameObject.SetActive(false);
            durabilityText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            UpdateDisplay();
            damageText.transform.parent.gameObject.SetActive(true);
            durabilityText.transform.parent.gameObject.SetActive(true);
        }
    }

    public void UpdateDisplay()
    {
        weaponSprite.sprite = weaponData.weaponSprite;
        damageText.text = weaponData.damage.ToString();
        durabilityText.text = weaponData.durability.ToString();
        durabilityDisplay.fillAmount = ((float)weaponData.durability / (float)weaponData.maxDurability) * 0.75f;
    }

    public void UpdatePosition()
    {
        //print(transform.position + " to " + targetPos);
        transform.DOMove(targetPos, 1f).SetEase(Ease.InOutSine).SetLink(gameObject);
    }

    public void IniTurnDisplay()
    {
        foreach (var turnUnit in allTurnUnits)
            Destroy(turnUnit);
        allTurnUnits.Clear();

        for (int i = 0; i < weaponData.useTurn; i++)
        {
            GameObject turnUnit = Instantiate(turnUnitPref, turnDisplay);
            allTurnUnits.Add(turnUnit);
        }
    }

    public int CurrentTurnUnitCount()
    {
        int count = 0;
        foreach (var tu in allTurnUnits)
        {
            if (tu.GetComponent<Image>().enabled)
                count += 1;
        }
        return count;
    }

    public void UpdateTurnUnit(int usedTurn)
    {
        IniTurnDisplay();
        int turnLeft = weaponData.useTurn - usedTurn;
        int currentTurnUnitCount = CurrentTurnUnitCount();

        if (turnLeft < currentTurnUnitCount)
        {
            int diff = (currentTurnUnitCount - turnLeft);
            for (int i = 0; i < diff; i++)
            {
                allTurnUnits[i].GetComponent<Image>().enabled = false;
            }
        }
        else if (turnLeft > currentTurnUnitCount)
        {
            int diff = (turnLeft - currentTurnUnitCount);
            for (int i = 0; i < diff; i++)
            {
                allTurnUnits[i].GetComponent<Image>().enabled = true;
            }
        }

    }

}
