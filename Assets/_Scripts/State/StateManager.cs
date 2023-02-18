using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public UnitController unitController;
    
    public State currentState;

    [Header("States")]
    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();
    public ParryState parryState = new ParryState();
    public KnockdownState knockdownState = new KnockdownState();


    void Start()
    {
        currentState = idleState;
    }

    void Update()
    {
        currentState.OnUpdateState(this);
    }

    public void SwitchState(State newState)
    {
        currentState.OnExitState(this);
        
        currentState = newState;

        currentState.OnEnterState(this);
    }


    public string GetCurrentStateName()
    {
        string stateName = currentState.GetType().ToString();
        stateName = stateName.Remove(stateName.Length - 5);
        return stateName;
    }
}
