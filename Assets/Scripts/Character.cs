using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform spine;

    CharacterStats stats;

    Animator anim;
    Weapon weapon;
    IKLook ikLook;

    CharacterCanvas canvas;

    Rigidbody[] rbs;
    CharacterController controller;

    public delegate void OnDamageApplied();
    public OnDamageApplied onDamageApplied;

    public virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<Weapon>();
        rbs = GetComponentsInChildren<Rigidbody>();
        controller = GetComponentInParent<CharacterController>();
        canvas = GetComponentInChildren<CharacterCanvas>();

        if(stats == null)
            stats = new CharacterStats();
    }

    public virtual void Start() 
    {
        ikLook = GetComponentInChildren<IKLook>();
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

    public Animator Anim { get { return anim; } }

    public Transform Head { get { return head; } }

    public Transform Spine { get { return spine; } }

    public Weapon Weapon { get { return weapon; } }

    public Rigidbody[] Rbs { get { return rbs; } }

    public CharacterController Controller { get { return controller; } }

    public CharacterStats Stats { get { return stats; } }

    public IKLook IKLook { get { return ikLook; } }

    public class CharacterStats
    {
        public float Health = 100f;

        public bool IsDead { get { return Health <= 0f; } }
    }
}
