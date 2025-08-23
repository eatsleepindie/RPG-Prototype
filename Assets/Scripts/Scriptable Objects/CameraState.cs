using UnityEngine;

[CreateAssetMenu(fileName = "CameraState", menuName = "Scriptable Objects/CameraState")]
public class CameraState : ScriptableObject
{
    [Range(0f, 0.5f)]
    public float PanDamping = 0.1f;
    public CameraTarget Target;
    [Range(0f, 10f)]
    public float Distance;
    [Range(0f, 1.5f)]
    public float CameraHeight;
    [Range(0f, 100f)]
    public float FieldOfView;
    public Vector3 Offset;
    [NaughtyAttributes.MinMaxSlider(-90f, 90f)]
    public Vector2 PitchLimits;

    public enum CameraTarget
    {
        Root,
        Head
    }
}
