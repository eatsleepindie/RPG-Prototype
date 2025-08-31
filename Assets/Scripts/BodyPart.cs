using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public CharacterInfo.CharacterAvatarPartType Type;
    public CharacterInfo.CharacterAvatarPartSide Side;

    public float Health;

    public delegate void BodyPartDamage();
    public BodyPartDamage onDamage;

    float lastDamageTime;

    Character character;

    private void Awake()
    {
        character = GetComponentInParent<Character>();

        lastDamageTime = -999f;
    }

    public void TakeDamage(float _damage)
    {
        if (Time.time - lastDamageTime < 1f) return;
        Health = Mathf.Clamp(Health - _damage, 0f, 100f);
        if (Health <= 0f)
        {
            switch(Type)
            {
                default:
                    break;
                case CharacterInfo.CharacterAvatarPartType.Head:
                    character.Death();
                    break;
                case CharacterInfo.CharacterAvatarPartType.Torso:
                    character.Death();
                    break;
                case CharacterInfo.CharacterAvatarPartType.Hips:
                    character.Death();
                    break;
            }
        }
            
        onDamage?.Invoke();
        lastDamageTime = Time.time;
    }
}
