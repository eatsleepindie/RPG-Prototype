using UnityEngine;

public class Weapon : MonoBehaviour
{
    MeshRenderer[] rends;

    float lastCutTime;
    bool canCut;

    private void Awake()
    {
        rends = GetComponentsInChildren<MeshRenderer>();

        DisableCut();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanCut || !other.CompareTag("Target")) return;
        lastCutTime = Time.time;
    }

    public void EnableCut()
    {
        if (rends[0].enabled && rends[0].shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly)
            canCut = true;
    }

    public void DisableCut()
    {
        canCut = false;
    }

    public void Show(bool _shadows = false)
    {
        foreach (MeshRenderer _rend in rends)
        {
            if (!_shadows)
                _rend.enabled = true;
            else
                _rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    public void Hide(bool _shadows = false)
    {
        foreach (MeshRenderer _rend in rends)
        {
            if (!_shadows)
                _rend.enabled = false;
            else
                _rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    public bool CanCut
    {
        get
        {
            return canCut && Time.time - lastCutTime > 1f;
        }
    }
}
