using UnityEngine;

public class RagdollPart : MonoBehaviour
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

        float _damage = Random.Range(10f, 33f);

        foreach(CharacterInfo.CharacterAvatarPart _part in avatar.GetBodyPartByType.Keys)
        {
            switch(Side)
            {
                default:
                    if(_part.Type == Type && _part.Side == Side)
                        avatar.GetBodyPartByType[_part].TakeDamage(_damage);
                    break;
                case CharacterInfo.CharacterAvatarPartSide.Full:
                    if(_part.Type == Type)
                        avatar.GetBodyPartByType[_part].TakeDamage(_damage);
                    break;
            }
        }
    }
}
