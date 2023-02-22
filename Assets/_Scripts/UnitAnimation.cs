using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    public UnitController unitController;
    public SpriteRenderer unitSprite;

    public Sprite idleSprite;
    public Sprite attackSprite;
    public Sprite parrySprite;
    public Sprite knockdownSprite;

    void Update()
    {
        switch (unitController.stateManager.currentState)
        {
            case IdleState:
                unitSprite.sprite = idleSprite;
                break;
            case AttackState:
                unitSprite.sprite = attackSprite;
                break;
            case ParryState:
                unitSprite.sprite = parrySprite;
                break;
            case KnockdownState:
                unitSprite.sprite = knockdownSprite;
                break;
        }
    }
}
