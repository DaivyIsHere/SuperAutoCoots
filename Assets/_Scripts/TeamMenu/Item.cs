using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public bool isSelected;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        canvas = TeamBuildManager.instance.mainCanvas;
        if (weaponData)
            weaponDisplay.sprite = weaponData.weaponSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
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
    }

    public void OnDeselect()
    {
        selectIndicator.gameObject.SetActive(false);
        isSelected = false;
    }
}
