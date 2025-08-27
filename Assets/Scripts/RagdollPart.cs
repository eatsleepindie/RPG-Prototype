using UnityEngine;

public class RagdollPart : MonoBehaviour
{
    CharacterAvatar avatar;

    public CharacterInfo.CharacterAvatarPartType Type;

    private void Awake()
    {
        avatar = GetComponentInParent<CharacterAvatar>();
    }
}
