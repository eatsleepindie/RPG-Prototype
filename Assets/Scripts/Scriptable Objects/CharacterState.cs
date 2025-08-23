using UnityEngine;

[CreateAssetMenu(fileName = "CharacterState", menuName = "Scriptable Objects/CharacterState")]
public class CharacterState : ScriptableObject
{
    public RotationRelativeTo RotationSpace;
    public AnimatorVariable[] AnimatorVariables;
    [Range(0f, 1f)]
    public float MovementDamping = 0.1f;

    public virtual void Enter(Character _character)
    {
        foreach (AnimatorVariable _variable in AnimatorVariables)
        {
            switch (_variable.AnimationVariableType)
            {
                case AnimatorVariableType.Boolean:
                    _character.Anim.SetBool(_variable.AnimationVariableName, true);
                    break;
                case AnimatorVariableType.Trigger:
                    _character.Anim.SetTrigger(_variable.AnimationVariableName);
                    break;
            }
        }
    }

    public virtual void Exit(Character _character)
    {
        foreach (AnimatorVariable _variable in AnimatorVariables)
        {
            switch (_variable.AnimationVariableType)
            {
                case AnimatorVariableType.Boolean:
                    _character.Anim.SetBool(_variable.AnimationVariableName, false);
                    break;
            }
        }
    }

    public enum State
    {
        Walk = 0,
        Run = 1,
        Sprint = 2,
        Jump = 3,
        Dodge = 4,
        Block = 5,
        Attack = 6
    }

    [System.Serializable]
    public class AnimatorVariable
    {
        public AnimatorVariableType AnimationVariableType;
        public string AnimationVariableName;
        public bool AnimatorVaraibleBool;
        public int AnimationVariableInt;
        public float AnimationVariableFloat;
    }

    public enum AnimatorVariableType
    {
        None,
        Boolean,
        Trigger,
        Integer,
        Float
    }

    public enum RotationRelativeTo
    {
        Camera,
        Movement
    }
}
