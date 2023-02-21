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

    public Vector2 targetPos;

    void Start()
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
        damageText.text = weaponData.damage.ToString();
        durabilityText.text = weaponData.durability.ToString();
        durabilityDisplay.fillAmount = ((float)weaponData.durability / (float)weaponData.maxDurability) * 0.75f;
    }

    public void UpdatePosition()
    {
        //print(transform.position + " to " + targetPos);
        transform.DOMove(targetPos, 1f).SetEase(Ease.InOutSine).SetLink(gameObject);
    }

    public void UpdateIsCurrentWeapon(bool isCurrentWeapon)
    {
        if (isCurrentWeapon)
            weaponSprite.DOFade(1f, 0.5f);
        else
            weaponSprite.DOFade(0.2f, 0.5f);
    }
}
