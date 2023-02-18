using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public float attackDuration = 0.5f;
    public float durationCountDown;
    
    public override void OnEnterState(StateManager stateManager)
    {
        //perform attack
        stateManager.unitController.PerformAttack();
        durationCountDown = attackDuration;
    }

    public override void OnUpdateState(StateManager stateManager)
    {
        if(durationCountDown > 0)
        {
            durationCountDown -= Time.deltaTime;
        }
        else
        {
            //End attack
            stateManager.SwitchState(stateManager.idleState);
        }
    }

    public override void OnExitState(StateManager stateManager)
    {
       
    }

    public override void OnAttackReady(StateManager stateManager)
    {
        Debug.Log("you just attacked twice in a short time, what?");
    }

    public override void OnTakeHit(StateManager stateManager)
    {
        stateManager.SwitchState(stateManager.parryState);
    }
}
