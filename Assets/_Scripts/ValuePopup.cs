using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ValuePopup : MonoBehaviour
{
    public TextMeshPro textDisplay;

    [Header("Values")]
    [SerializeField] private Vector3 punchScale = Vector3.one * 1.5f;
    [SerializeField] private float punchDuration = 0.5f;
    [SerializeField] private int punchVibrato = 5;
    [SerializeField] private float punchElast = 0.5f;
    [SerializeField] private float endPosY = 1.5f;
    [SerializeField] private float moveDuration = 1;

    void Start()
    {
        //StartPopup();
        DisplayPopup();
    }

    private void StartPopup()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(textDisplay.transform.DORotate(new Vector3(0, 0, -20f), 0.3f));
        sequence.Join(textDisplay.DOFade(1f, 0.3f));
        sequence.AppendInterval(1.5f);
        sequence.Append(textDisplay.transform.DORotate(new Vector3(0, 0, 20f), 0.3f));
        sequence.Join(textDisplay.DOFade(0f, 0.3f)).OnComplete(() => Destroy(gameObject));

        //textDisplay.DOFade(0,1f);
        //textDisplay.transform.DOLocalMoveY(0.5f, 1f).OnComplete(() => Destroy(gameObject)).SetLink(gameObject);
    }

    private void DisplayPopup()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(textDisplay.transform.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElast));
        sequence.Join(textDisplay.transform.DOLocalMoveY(endPosY, moveDuration).SetEase(Ease.OutQuint));
        sequence.Join(textDisplay.DOFade(0,0.3f).SetEase(Ease.InQuint));
        sequence.OnComplete(() => SelfDestroy());
    }

    private void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
