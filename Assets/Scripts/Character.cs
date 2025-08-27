using JetBrains.Annotations;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] CharacterInfo info;

    CharacterStats stats;

    CharacterAvatar avatar;
    Animator anim;
    Weapon weapon;
    IKLook ikLook;
    Collider[] colliders;

    CharacterCanvas canvas;

    Rigidbody[] rbs;
    CharacterController controller;

    public delegate void OnDamageApplied();
    public OnDamageApplied onDamageApplied;

    public virtual void Awake()
    {

        if(stats == null)
            stats = new CharacterStats();
    }

    public virtual void Start()
    {
        avatar = GetComponentInChildren<CharacterAvatar>();
        anim = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<Weapon>();
        rbs = GetComponentsInChildren<Rigidbody>();
        controller = GetComponentInParent<CharacterController>();
        canvas = GetComponentInChildren<CharacterCanvas>();
        ikLook = GetComponentInChildren<IKLook>();
        colliders = GetComponents<Collider>();
    }

    public virtual void OnAnimatorMove()
    {

    }

    public virtual float ApplyDamage(float _damage)
    {
        Stats.Health -= _damage;

        //Debug.Log(Stats.Health);

        onDamageApplied?.Invoke();

        if (Stats.IsDead)
            Ragdoll();

        return Stats.Health;
    }

    public virtual void Ragdoll() { }

    public enum AnimatorLayer
    {
        Base = 0
    }

    public CharacterAvatar Avatar { get { return avatar; } }

    public Animator Anim { get { return anim; } }

    public Weapon Weapon { get { return weapon; } }

    public Rigidbody[] Rbs { get { return rbs; } }

    public CharacterController Controller { get { return controller; } }

    public CharacterStats Stats { get { return stats; } }

    public IKLook IKLook { get { return ikLook; } }

    public Collider[] Colliders { get { return colliders; } }

    public CharacterInfo Info { get { return info; } }

    public class CharacterStats
    {
        public float Health = 100f;

        public bool IsDead { get { return Health <= 0f; } }
    }
}
