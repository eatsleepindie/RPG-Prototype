using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    [SerializeField] CharacterState[] states;

    public Dictionary<CharacterState, int> GetStateID = new Dictionary<CharacterState, int>();

    CharacterState currentState;
    CharacterState previousState;
    Character character;

    public virtual void Awake()
    {
        character = GetComponentInChildren<Character>();

        for (int _i = 0; _i < states.Length; _i++)
            GetStateID.Add(states[_i], _i);
    }

    public virtual void EnterState(CharacterState.State _state)
    {
        if (currentState != null)
        {
            previousState = currentState;
            currentState.Exit(character);
        }

        currentState = states[(int)_state];
        currentState.Enter(character);
    }

    public virtual void ExitState()
    {
        Debug.Log("Exit State: " +  currentState);
        foreach(CharacterState.AnimatorVariable _variable in currentState.AnimatorVariables)
        {
            if (_variable.AnimationVariableType == CharacterState.AnimatorVariableType.Boolean)
                character.Anim.SetBool(_variable.AnimationVariableName, false);
        }
        Debug.Log((CharacterState.State)GetStateID[previousState]);
        EnterState((CharacterState.State)GetStateID[previousState]);
    }

    public CharacterState CurrentState { get { return currentState; } }

    public bool IsWalking { get { return currentState == states[(int)CharacterState.State.Walk]; } }
    public bool IsSprinting { get { return currentState == states[(int)CharacterState.State.Sprint]; } }
    public bool IsAttacking { get { return currentState == states[(int)CharacterState.State.Attack]; } }
    public bool IsDodging { get { return currentState == states[(int)CharacterState.State.Dodge]; } }
    public bool IsBlocking { get { return currentState == states[(int)CharacterState.State.Block]; } }

    public bool CanAction
    {
        get
        {
            return GetStateID[currentState] < 2 && character.Anim.GetLayerWeight(1) >= 0.75f;
        }
    }
}
