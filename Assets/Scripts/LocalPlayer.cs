using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalPlayer : MonoBehaviour
{
    [Range(0f, 0.5f)]
    [SerializeField] float idleDamping = 0.1f;
    [Range(0f, 0.5f)]
    [SerializeField] float movementDamping = 0.1f;
    [Range(0f, 0.5f)]
    [SerializeField] float rotationDamping = 0.1f;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpMomentum = 5f;
    [SerializeField] float kickRayRadius = 0.25f;
    [SerializeField] float kickRayDistance = 3f;

    CharacterController controller;
    PlayerCharacter[] characters;
    PlayerStateMachine stateMachine;
    CameraRig cameraRig;

    Vector2 moveInput;
    Vector3 velocity;

    bool isSprinting;
    bool isAttacking;
    bool isInitiatingAttack;
    bool isJumping;
    bool isDodging;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        characters = GetComponentsInChildren<PlayerCharacter>();
        stateMachine = GetComponent<PlayerStateMachine>();
        cameraRig = GetComponentInChildren<CameraRig>();
        characters[1].transform.SetParent(cameraRig.transform);
        Vector3 _position = characters[1].transform.localPosition;
        _position.z = 0f;
        //characters[1].transform.localPosition = _position;
    }

    private void Update()
    {
        CalculateGravity();
        if (characters[0].Anim.GetBool("Is Jumping") && IsGrounded)
        {
            velocity = Vector3.up * velocity.y;
            foreach (Character _character in characters)
                _character.Anim.SetBool("Is Jumping", false);
        }    

        foreach (PlayerCharacter _character in characters)
        {
            _character.Anim.SetLayerWeight(1, 
                Mathf.Lerp(_character.Anim.GetLayerWeight(1), isAttacking ? 1f : 0f, Time.deltaTime / 0.1f)
                );
            _character.Anim.SetBool("Sprint", isSprinting);
            _character.Anim.SetFloat("Horizontal", moveInput.x, movementDamping, Time.deltaTime);
            _character.Anim.SetFloat("Vertical", moveInput.y, movementDamping, Time.deltaTime);
        }

        if (moveInput.magnitude == 0f)
        {
            foreach (PlayerCharacter _character in characters)
                _character.Anim.SetFloat("Idle Time", Mathf.Lerp(_character.Anim.GetFloat("Idle Time"), 1f, Time.deltaTime / idleDamping));
            return;
        }

        foreach (PlayerCharacter _character in characters)
            _character.Anim.SetFloat("Idle Time", 0f);
        if (isSprinting)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraRig.transform.TransformDirection(MoveInputToVector3), Vector3.up), Time.deltaTime / rotationDamping);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRig.transform.rotation.eulerAngles.y * Vector3.up), Time.deltaTime / rotationDamping);
    }

    void CalculateGravity()
    {
        velocity += Physics.gravity.y * Vector3.up * Time.deltaTime;
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = 0f;
        controller.Move(velocity * Time.deltaTime);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void ApplyKickForce()
    {
        Ray _ray = new Ray(transform.TransformPoint(Vector3.up), transform.forward);
        Debug.DrawRay(_ray.origin, _ray.direction * kickRayDistance, Color.red, 5f);
        RaycastHit _hit;
        if (Physics.SphereCast(_ray, kickRayRadius, out _hit, kickRayDistance, GameManager.Instance.EnemyMask))
            _hit.transform.root.GetComponent<Enemy>().ApplyKickDamage();
    }

    #region Input Processing
    public void ProcessMoveInput(Vector2 _input)
    {
        moveInput = _input;
    }

    public void ProcessSprintInput(bool _value)
    {
        isSprinting = _value;
        if(_value)
            stateMachine.EnterState(CharacterState.State.Sprint);
        else
            stateMachine.ExitState();
    }

    public void ProcessJumpInput(bool _value)
    {
        if(_value && IsGrounded)
        {
            velocity += transform.TransformDirection(MoveInputToVector3 * jumpMomentum);
            velocity.y = jumpForce;
            controller.Move(Vector3.up * 0.2f);
            foreach (Character _character in characters)
                _character.Anim.SetTrigger("Jump");
        }

        foreach (Character _character in characters)
            _character.Anim.SetBool("Is Jumping", true);
    }

    public void ProcessBlockInput(bool _value)
    {
        if (_value)
        {
            foreach(Character _character in characters)
                _character.Anim.SetTrigger("Block");
        }
        foreach (Character _character in characters)
            _character.Anim.SetBool("Is Blocking", _value);
    }

    public void ProcessDodgeInput()
    {
        foreach (Character _character in characters)
            _character.Anim.SetTrigger("Dodge");
    }

    public void ProcessAttackInput(int _type)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (isAttacking) return;
        if (isSprinting)
            ProcessSprintInput(false);
        if (_type < 0)
        {
            isInitiatingAttack = true;
            foreach (Character _character in characters)
                _character.Anim.SetInteger("Attack Direction", Mathf.RoundToInt(moveInput.x));
            return;
        }
        else
        {
            isInitiatingAttack = false;
        }
        foreach (Character _character in characters)
        {
            isAttacking = true;
            _character.Anim.SetInteger("Attack Type", _type);
            _character.Anim.SetTrigger("Attack");
        }
    }

    public void ProcessKickInput()
    {
        foreach (Character _character in characters)
            _character.Anim.SetTrigger("Kick");
    }
    #endregion

    bool IsGrounded
    {
        get
        {
            return transform.position.y <= 0.1f;
        }
    }

    public PlayerCharacter[] Characters { get { return characters; } }
    public CameraRig CameraRig { get { return cameraRig; } }

    public Vector3 MoveInputToVector3 { get { return new Vector3(moveInput.x, 0f, moveInput.y); } }

    public bool IsJumping { get { return isJumping; } }
}
