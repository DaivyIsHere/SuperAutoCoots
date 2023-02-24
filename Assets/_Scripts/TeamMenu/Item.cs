using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public WeaponData weaponData;
    public ItemSlot slot;

    [Header("Component")]
    protected Canvas canvas;
    protected CanvasGroup canvasGroup;
    protected RectTransform rect;
    [SerializeField] protected Image weaponDisplay;
    [SerializeField] protected RectTransform selectIndicator;
    [SerializeField] protected TMP_Text damageDisplay;
    [SerializeField] protected TMP_Text durabilityDisplay;

    public bool isSelected;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        canvas = TeamBuildManager.instance.mainCanvas;
        if (weaponData)
        {
            weaponDisplay.sprite = weaponData.weaponSprite;
            UpdateStatsDisplay();
        }
    }

    public virtual void UpdateStatsDisplay()
    {
        damageDisplay.text = weaponData.damage.ToString();
        durabilityDisplay.text = weaponData.maxDurability.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapBackToSlot();
    }

    public virtual void SnapBackToSlot()
    {
        canvasGroup.blocksRaycasts = true;
        transform.position = slot.transform.position;
    }

    public virtual void OnClickSellOrLock()
    {
        print("Click sell or lock");
    }

    public void OnSelected()
    {
        selectIndicator.gameObject.SetActive(true);
        isSelected = true;
        ItemInfoDisplay.instance.ShowInfo(this);
    }

    public void OnDeselect()
    {
        selectIndicator.gameObject.SetActive(false);
        isSelected = false;
    }

}
