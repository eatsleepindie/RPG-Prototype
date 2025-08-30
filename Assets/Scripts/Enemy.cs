using UnityEditor;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] float forceAmount = 250f;
    [SerializeField] float upwardsForce = 100f;
    [SerializeField] float kickOffset = 15f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        base.Anim.avatar = base.Info.Rig;
        base.Anim.runtimeAnimatorController = base.Info.Controller;
        base.IKLook.Target = GameManager.Instance.Players[0].Characters[0].Avatar.Head;

        foreach (Rigidbody _rb in base.Rbs)
            _rb.GetComponent<Collider>().isTrigger = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            Ragdoll();
    }

    public override void OnAnimatorMove()
    {
        base.OnAnimatorMove();
        if(base.Controller != null && base.Controller.enabled)
            base.Controller.Move(base.Anim.deltaPosition);
    }

    public override void Ragdoll()
    {
        foreach(Rigidbody _rb in base.Rbs)
        {
            _rb.isKinematic = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.GetComponent<Collider>().isTrigger = false;
        }
        Anim.enabled = false;
    }

    public void ApplyKickDamage()
    {
        // temporary
        Ragdoll();

        Rigidbody _spine = base.Avatar.Spine.GetComponent<Rigidbody>();
        Vector3 _direction = GameManager.Instance.Players[0].transform.forward;
        foreach (Collider _collider in base.Colliders)
            _collider.enabled = false;
        _direction.y = 0f;
        _direction = Quaternion.AngleAxis(Random.Range(-kickOffset, kickOffset), Vector3.up) * _direction.normalized;
        _spine.AddForce(
            _direction * forceAmount + Vector3.up * upwardsForce,
            ForceMode.Impulse
            );
    }

    /*
    void DealBodyPartDamage(EnemyBodyPart _bodyPart)
    {
        base.ApplyDamage(_bodyPart.Health);
    }
    */

    public void ApplyCutDamage()
    {
        
    }

    public EnemyInfo EnemyInfo { get { return (EnemyInfo)base.Info; } }
}
