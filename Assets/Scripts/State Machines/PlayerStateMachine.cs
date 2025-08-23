using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : CharacterStateMachine
{
    public override void Awake()
    {
        base.Awake();
        EnterState(CharacterState.State.Run);
    }

    public override void EnterState(CharacterState.State _state)
    {
        base.EnterState(_state);
    }
}
