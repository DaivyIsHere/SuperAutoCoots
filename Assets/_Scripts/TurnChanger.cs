using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnChanger : MonoBehaviour
{
    public RectTransform indicator;
    public float moveDistance;
    public float prepareDuration = 2f;
    public float turnInterval = 1.5f;

    public RectTransform leftBar;
    public RectTransform rightBar;

    void Start()
    {
        BattleManager.instance.OnTurnChange += BarPunches;
        TurnPrepare();
    }

    private void TurnPrepare()
    {
        indicator.DOLocalMoveX(-moveDistance, prepareDuration).OnComplete(() => StartBattle()).SetLink(indicator.gameObject);
    }

    private void StartBattle()
    {
        //First turn
        BattleManager.instance.StartBattle();
        PunchAnimation(leftBar);

        StartCoroutine(TurnChange());
        //DisplayTurnChange();
    }

    private IEnumerator TurnChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(turnInterval);
            BattleManager.instance.ChangeTurn();
        }
    }

    private void DisplayTurnChange()
    {
        //indicator.DOLocalMoveX(moveDistance, turnInterval).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(indicator.gameObject).SetUpdate(true);
    }

    private void BarPunches(BattleSide currentSide)
    {
        if (currentSide == BattleSide.Left)
        {
            indicator.DOLocalMoveX(moveDistance, turnInterval).SetEase(Ease.Linear).SetLink(indicator.gameObject);
            PunchAnimation(leftBar);//leftBar.DOPunchScale(Vector2.one * 0.5f, 0.5f, 5);
        }
        else
        {
            indicator.DOLocalMoveX(-moveDistance, turnInterval).SetEase(Ease.Linear).SetLink(indicator.gameObject);
            PunchAnimation(rightBar);//rightBar.DOPunchScale(Vector2.one * 0.5f, 0.5f, 5);
        }
    }

    private void PunchAnimation(Transform target)
    {
        target.DOPunchScale(Vector2.one * 0.5f, 0.5f, 5).SetLink(target.gameObject);
    }
}
