using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void OnEnterState(StateManager stateManager);

    public abstract void OnUpdateState(StateManager stateManager);

    public abstract void OnExitState(StateManager stateManager);

    public abstract void OnAttackReady(StateManager stateManager);

    public abstract void OnTakeHit(StateManager stateManager);
}
