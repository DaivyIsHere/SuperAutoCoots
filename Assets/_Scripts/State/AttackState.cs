using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private float endAttackThreshold = 0.5f;//velocity magnitude
    //public float attackDuration = 0.5f;
    //public float durationCountDown;
    
    public override void OnEnterState(StateManager stateManager)
    {
        //perform attack
        if(!stateManager.unitController.currentWeapon)
        {
            Debug.LogWarning("No Current Weapon for "+stateManager.unitController.name);
            return;
        }
        stateManager.unitController.currentWeapon.PerformAttack(stateManager.unitController);
        //durationCountDown = stateManager.unitController.weaponData.movementDuration;
    }

    public override void OnUpdateState(StateManager stateManager)
    {
        if(stateManager.unitController.rb2d.velocity.magnitude < endAttackThreshold)
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

    public override void OnLanded(StateManager stateManager)
    {
        
    }
}
