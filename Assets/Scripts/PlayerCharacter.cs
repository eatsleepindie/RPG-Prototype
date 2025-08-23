using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerCharacter : Character
{
    [SerializeField] float jumpForce = 10f;
    [SerializeField] PlayerCharacterSwapMode swapMode;

    LocalPlayer player;

    SkinnedMeshRenderer[] renderers;

    bool isCurrent;

    public override void Awake()
    {
        isCurrent = transform.GetSiblingIndex() == 0;
        base.Awake();
        player = GetComponentInParent<LocalPlayer>();
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        foreach (Collider _collider in GetComponentsInChildren<Collider>())
        {
            if (_collider.attachedRigidbody != null)
                _collider.isTrigger = true;
        }
    }

    public override void OnAnimatorMove()
    {
        if (!isCurrent) return;
        base.Controller.Move(Anim.deltaPosition);
        base.Controller.transform.rotation *= Anim.deltaRotation;
    }

    public void EnableCut()
    {
        if (Weapon == null) return;
        Weapon.EnableCut();
    }

    public void DisableCut()
    {
        if (Weapon == null) return;
        Weapon.DisableCut();
    }

    public void EndAttack()
    {
        player.EndAttack();
    }

    public void ApplyKickForce()
    {
        player.ApplyKickForce();
    }

    public void Show()
    {
        foreach (SkinnedMeshRenderer _renderer in renderers)
        {
            if (swapMode == PlayerCharacterSwapMode.ShowHide)
                _renderer.enabled = true;
            else
                _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }


        if (base.Weapon != null)
            base.Weapon.Show(swapMode == PlayerCharacterSwapMode.Shadows);
    }

    public void Hide()
    {
        foreach (SkinnedMeshRenderer _renderer in renderers)
        {
            if (swapMode == PlayerCharacterSwapMode.ShowHide)
                _renderer.enabled = false;
            else
                _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

        if (base.Weapon != null)
            base.Weapon.Hide(swapMode == PlayerCharacterSwapMode.Shadows);
    }

    public enum PlayerCharacterSwapMode
    {
        ShowHide,
        Shadows
    }
}
