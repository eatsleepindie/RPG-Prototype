using UnityEngine;

public class Avatar : MonoBehaviour
{
    Transform head;
    Transform spine;

    void Start()
    {
        foreach(BodyPart _part in GetComponentsInChildren<BodyPart>())
        {
            switch(_part.Part)
            {
                case BodyPart.BodyPartType.Head:
                    head = _part.transform;
                    break;
                case BodyPart.BodyPartType.Spine:
                    spine = _part.transform;
                    break;
            }
        }
    }

    public Transform Head { get { return head; } }

    public Transform Spine { get { return spine; } }
}
