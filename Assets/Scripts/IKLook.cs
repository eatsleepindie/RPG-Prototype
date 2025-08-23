using UnityEngine;

public class IKLook : MonoBehaviour
{
    [SerializeField] Transform target;
    [Range(0f, 1f)]
    [SerializeField] float weight;
    [Range(0f, 1f)]
    [SerializeField] float bodyWeight;
    [Range(0f, 1f)]
    [SerializeField] float headWeight;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (target == null) return;
        anim.SetLookAtPosition(target.position);
        anim.SetLookAtWeight(weight, bodyWeight, headWeight);
    }

    public Transform Target { set { target = value; } }
}
