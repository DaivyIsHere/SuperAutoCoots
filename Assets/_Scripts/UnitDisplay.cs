using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UnitDisplay : MonoBehaviour
{
    [Header("Data")]
    public UnitData unitData;

    [Header("Display")]
    public SpriteRenderer avatarDisplay;
    public GameObject attackDisplay;
    public GameObject healthDisplay;
    public TextMeshPro attackText;
    public TextMeshPro healthText;

    void Start()
    {

    }

    void Update()
    {
        if (unitData)
        {
            avatarDisplay.DOFade(1f, 0);
            attackDisplay.SetActive(true);
            healthDisplay.SetActive(true);
            attackText.text = unitData.attack.ToString();
            healthText.text = unitData.currentHealth.ToString();
        }
        else
        {
            avatarDisplay.DOFade(0.3f, 0);
            attackDisplay.SetActive(false);
            healthDisplay.SetActive(false);
        }
    }
}
