using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] BodyPartType part;

    public BodyPartType Part { get { return part; } }

    public enum BodyPartType {
        Head = 0,
        Spine = 1
    }
}
