using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryState : State
{
    private float endParryThreshold = 1f;//velocity magnitude
    //private float knockdownThreshold = 2f;

    public override void OnEnterState(StateManager stateManager)
    {

    }

    public override void OnUpdateState(StateManager stateManager)
    {
        //slow enough to end parry
        if (stateManager.unitController.rb2d.velocity.magnitude < endParryThreshold)
            stateManager.SwitchState(stateManager.idleState);
    }

    public override void OnExitState(StateManager stateManager)
    {

    }

    public override void OnAttackReady(StateManager stateManager)
    {
        //if (stateManager.unitController.lastAirHeight < knockdownThreshold)
            stateManager.SwitchState(stateManager.attackState);
    }

    public override void OnTakeHit(StateManager stateManager)
    {
        //already in parry, dont care
    }

    public override void OnLanded(StateManager stateManager)
    {
        //if (stateManager.unitController.lastAirHeight > knockdownThreshold)
        //    stateManager.SwitchState(stateManager.knockdownState);
    }
}
