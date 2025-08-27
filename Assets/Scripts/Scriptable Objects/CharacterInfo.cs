using UnityEngine;

public class CharacterInfo : ScriptableObject
{
    public GameObject Prefab;
    public Avatar Rig;
    public RuntimeAnimatorController Controller;
    public CharacterAvatarPart[] AvatarParts;

    [System.Serializable]
    public class CharacterAvatarPart
    {
        public CharacterAvatarPartType Type;
        public CharacterAvatarPartSide Side;
        public GameObject Mesh;

        [Space(5f)]
        [Header("Ragdoll")]
        public ColliderType ColliderType;
        public Mesh ColliderMesh;
        public Vector3 ColliderOffset;
        public Vector3 ColliderSize = Vector3.one;
        public float ColliderRadius = 1f;
        public float ColliderHeight = 1f;

        [Space(5f)]
        [Header("Debug")]
        public Color DebugColor;
    }

    public enum CharacterAvatarPartType
    {
        Head,
        Torso,
        Hips,
        Thigh,
        Leg,
        Calf,
        Arm,
        Hand
    }

    public enum CharacterAvatarPartSide
    {
        Left,
        Right
    }

    public enum ColliderType
    {
        None,
        Box,
        Sphere,
        Capsule,
        Mesh
    }
}
