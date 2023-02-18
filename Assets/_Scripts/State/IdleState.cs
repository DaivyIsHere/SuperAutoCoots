using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public override void OnEnterState(StateManager stateManager)
    {
        
    }

    public override void OnUpdateState(StateManager stateManager)
    {
        
    }

    public override void OnExitState(StateManager stateManager)
    {
        
    }

    public override void OnAttackReady(StateManager stateManager)
    {
        //if your turn start, and can attack
        stateManager.SwitchState(stateManager.attackState);
    }

    public override void OnTakeHit(StateManager stateManager)
    {
        stateManager.SwitchState(stateManager.parryState);
    }
}
