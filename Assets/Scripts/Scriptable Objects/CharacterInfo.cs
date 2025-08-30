using UnityEngine;

public class CharacterInfo : ScriptableObject
{
    public GameObject[] Prefabs;
    public Avatar Rig;
    public RuntimeAnimatorController Controller;
    public CharacterAvatarPart[] AvatarParts;

    [System.Serializable]
    public class CharacterAvatarPart
    {
        public CharacterAvatarPartType Type;
        public CharacterAvatarPartSide Side;
        public GameObject Mesh;
        [Range(0f, 100f)]
        public float Health = 100f;

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
        Head = 0,
        Torso = 10,
        Hips= 11,
        Thigh = 20,
        Leg = 21,
        Calf = 22,
        Shoulder = 30,
        Arm = 31,
        Forearm = 32,
        Hand = 33
    }

    public enum CharacterAvatarPartSide
    {
        Full = 0,
        Left = 1,
        Right = 2
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
