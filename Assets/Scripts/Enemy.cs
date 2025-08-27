using UnityEngine;

public class Enemy : Character
{
    [SerializeField] EnemyInfo info;

    [SerializeField] float forceAmount = 250f;
    [SerializeField] float upwardsForce = 100f;
    [SerializeField] float kickOffset = 15f;

    [SerializeField] EnemyBodyPart[] bodyParts;

    public override void Awake()
    {
        base.Awake();

        foreach (GameObject _part in info.AvatarParts)
            Instantiate(_part, transform);
    }

    public override void Start()
    {
        base.Start();

        base.Anim.avatar = info.Rig;
        base.Anim.runtimeAnimatorController = info.Controller;
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

    void DealBodyPartDamage(EnemyBodyPart _bodyPart)
    {
        base.ApplyDamage(_bodyPart.Health);
    }

    public void ApplyCutDamage()
    {
        
    }

    public enum BodyPart
    {
        Hips = 0,
        LeftUpLeg = 1,
        LeftLeg = 2,
        LeftFoot = 3,
        RightUpLeg = 4,
        RightLeg = 5,
        RightFoot = 6,
        Spine1 = 7,
        LeftArm = 8,
        LeftForeArm = 9,
        LeftHand = 10,
        Head = 11,
        RightArm = 12,
        RightForeArm = 13,
        RightHand = 14
    }

    [System.Serializable]
    public class EnemyBodyPart
    {
        public BodyPart BodyPart;
        [Range(0f, 250f)]
        public float Health = 100f;
    }
}
