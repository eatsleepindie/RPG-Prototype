using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayerInput : MonoBehaviour
{
    PlayerInput input;
    LocalPlayer player;

    InputAction moveAction;
    InputAction sprintAction;
    InputAction jumpAction;
    InputAction dodgeAction;

    InputAction attackLightAction;
    InputAction attackHeavyAction;
    InputAction kickAction;
    InputAction blockAction;

    InputAction rotatingAction;
    InputAction rotateAction;
    InputAction zoomAction;

    InputAction ragdollAction;
    InputAction timeAction;
    InputAction spawnTargetAction;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        player = GetComponent<LocalPlayer>();

        moveAction = input.actions.FindAction("Move");
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;

        sprintAction = input.actions.FindAction("Sprint");
        sprintAction.performed += OnSprintPerformed;
        sprintAction.canceled += OnSprintCanceled;

        jumpAction = input.actions.FindAction("Jump");
        jumpAction.performed += OnJumpPerformed;
        jumpAction.canceled += OnJumpCanceled;

        dodgeAction = input.actions.FindAction("Dodge");
        dodgeAction.performed += OnDodgePerformed;

        #region Weapon Actions
        /*
        equipAction = input.actions.FindAction("Equip");
        equipAction.performed += OnEquipPerformed;

        dodgeAction = input.actions.FindAction("Dodge");
        dodgeAction.performed += OnDodgePerformed;
        */

        attackLightAction = input.actions.FindAction("Attack Light");
        attackLightAction.started += OnAttackStarted;
        attackLightAction.performed += OnAttackLightPerformed;
        attackHeavyAction = input.actions.FindAction("Attack Heavy");
        attackHeavyAction.performed += OnAttackHeavyPerformed;

        kickAction = input.actions.FindAction("Kick");
        kickAction.performed += OnKickPerformed;

        blockAction = input.actions.FindAction("Block");
        blockAction.performed += OnBlockPerformed;
        blockAction.canceled += OnBlockCanceled;
        #endregion

        #region Camera Actions
        rotatingAction = input.actions.FindAction("Rotating");
        rotatingAction.performed += OnRotatingPerformed;

        rotateAction = input.actions.FindAction("Rotate");
        rotateAction.performed += OnRotatePerformed;

        zoomAction = input.actions.FindAction("Zoom");
        zoomAction.performed += OnZoomPerformed;
        #endregion

        #region Development Actions
        ragdollAction = input.actions.FindAction("Ragdoll Test");
        ragdollAction.performed += OnRagdollPerformed;

        timeAction = input.actions.FindAction("Warp Time");
        timeAction.performed += OnTimePerformed;

        spawnTargetAction = input.actions.FindAction("Spawn Target");
        spawnTargetAction.performed += OnSpawnTargetPerformed;
        #endregion
    }

    #region Movement Callbacks
    void OnMovePerformed(InputAction.CallbackContext _context)
    {
        player.ProcessMoveInput(_context.ReadValue<Vector2>());
    }

    void OnMoveCanceled(InputAction.CallbackContext _context)
    {
        player.ProcessMoveInput(_context.ReadValue<Vector2>());
    }

    void OnSprintPerformed(InputAction.CallbackContext _context)
    {
        player.ProcessSprintInput(true);
    }

    void OnSprintCanceled(InputAction.CallbackContext _context)
    {
        player.ProcessSprintInput(false);
    }

    void OnJumpPerformed(InputAction.CallbackContext _context)
    {
        player.ProcessJumpInput(true);
    }

    void OnJumpCanceled(InputAction.CallbackContext _context)
    {
        player.ProcessJumpInput(false);
    }

    void OnDodgePerformed(InputAction.CallbackContext _context)
    {
        player.ProcessDodgeInput();
    }
    #endregion

    #region Weapon Callbacks
    void OnBlockPerformed(InputAction.CallbackContext _context)
    {
        player.ProcessBlockInput(true);
    }
    void OnBlockCanceled(InputAction.CallbackContext _context)
    {
        player.ProcessBlockInput(false);
    }
    void OnAttackStarted(InputAction.CallbackContext _context)
    {
        player.ProcessAttackInput(-1);
    }
    void OnAttackLightPerformed(InputAction.CallbackContext _context)
    {
        if(_context.duration < 0.5f)
            player.ProcessAttackInput(0);
    }
    void OnAttackHeavyPerformed(InputAction.CallbackContext _context)
    {
        player.ProcessAttackInput(1);
    }
    void OnKickPerformed(InputAction.CallbackContext _context)
    {
        player.ProcessKickInput();
    }
    #endregion

    #region Camera Callbacks
    void OnRotatingPerformed(InputAction.CallbackContext _context)
    {
        player.CameraRig.ProcessRotatingInput(_context.ReadValue<float>());
    }

    void OnRotatePerformed(InputAction.CallbackContext _context)
    {
        player.CameraRig.ProcessRotateInput(_context.ReadValue<Vector2>());
    }

    void OnZoomPerformed(InputAction.CallbackContext _context)
    {
        player.CameraRig.ProcessZoomInput();
    }
    #endregion

    #region Development Callbacks
    void OnRagdollPerformed(InputAction.CallbackContext _context)
    {
        GameManager.Instance.Enemy.Ragdoll();
    }

    void OnTimePerformed(InputAction.CallbackContext _context)
    {
        GameManager.Instance.ToggleTime();
    }

    void OnSpawnTargetPerformed(InputAction.CallbackContext _context)
    {
        GameManager.Instance.SpawnTarget();
    }
    #endregion
}
