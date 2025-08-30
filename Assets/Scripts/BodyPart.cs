using UnityEngine;

public class BodyPart : MonoBehaviour
{
    CharacterAvatar avatar;

    public CharacterInfo.CharacterAvatarPartType Type;
    public CharacterInfo.CharacterAvatarPartSide Side;

    private void Awake()
    {
        avatar = GetComponentInParent<CharacterAvatar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Weapon")) return;

        Debug.Log(name);
    }
}
