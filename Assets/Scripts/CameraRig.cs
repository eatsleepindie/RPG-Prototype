using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraRig : MonoBehaviour
{
    [SerializeField] CameraState[] states;
    [Range(0f, 0.5f)]
    [SerializeField] float distanceDamping = 0.1f;
    [Range(0f, 15f)]
    [SerializeField] float yawSpeed = 5f;
    [Range(0f, 0.5f)]
    [SerializeField] float yawDamping = 0.1f;
    [SerializeField] bool pitchInverted;
    [Range(0f, 15f)]
    [SerializeField] float pitchSpeed = 5f;
    [Range(0f, 0.5f)]
    [SerializeField] float pitchDamping = 0.1f;
    [Range(0f, 0.5f)]
    [SerializeField] float offsetDamping = 0.1f;
    [Range(0f, 0.5f)]
    [SerializeField] float fieldOfViewDamping = 0.1f;

    [Space(10f)]
    [Header("UI")]
    [SerializeField] Slider[] mouseSensitivitySliders;

    Camera cam;
    LocalPlayer player;

    Transform pitchAxis;
    float targetYaw;
    float targetPitch;
    bool isRotating;

    CameraState currentState;

    public delegate void OnRotating(bool _value);
    public OnRotating onRotating;

    private void Awake()
    {
        pitchAxis = transform.GetChild(0);

        cam = GetComponentInChildren<Camera>();
        player = GetComponentInParent<LocalPlayer>();

        // ui listeners
        mouseSensitivitySliders[0].onValueChanged.AddListener((value) => OnHorizontalSensitivityChanged(value)); 
        mouseSensitivitySliders[1].onValueChanged.AddListener((value) => OnVerticalSensitivityChanged(value));
    }

    private void Start()
    {
        transform.SetParent(null);

        currentState = states[1];
        ProcessZoomInput();

        if(PlayerPrefs.HasKey("mouse horizontal"))
        {
            mouseSensitivitySliders[0].value = PlayerPrefs.GetFloat("mouse horizontal");
            mouseSensitivitySliders[1].value = PlayerPrefs.GetFloat("mouse vertical");
        }
        else
        {
            mouseSensitivitySliders[0].value = yawSpeed;
            mouseSensitivitySliders[1].value = pitchSpeed;
        }
    }

    void Update()
    {
        pitchAxis.transform.localPosition = Vector3.Lerp(pitchAxis.transform.localPosition, currentState.Offset, Time.deltaTime / offsetDamping);
        cam.transform.localPosition = Vector3.Lerp(
            cam.transform.localPosition, 
            Vector3.back * currentState.Distance + Vector3.up * currentState.CameraHeight, 
            Time.deltaTime / offsetDamping
            );
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentState.FieldOfView, Time.deltaTime / fieldOfViewDamping);
        if (player == null) return;
        transform.position = Vector3.Lerp(transform.position, (currentState.Target == CameraState.CameraTarget.Root) ? player.transform.position : player.Characters[0].Head.position, Time.deltaTime / currentState.PanDamping);

        if (!isRotating) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetYaw * Vector3.up), Time.deltaTime / yawDamping);
        pitchAxis.transform.localRotation = Quaternion.Slerp(pitchAxis.transform.localRotation, Quaternion.Euler(targetPitch * Vector3.right), Time.deltaTime / pitchDamping);
    }

    public void ProcessRotatingInput(float _value)
    {
        isRotating = _value > 0f && !EventSystem.current.IsPointerOverGameObject();
        Cursor.lockState = (isRotating) ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isRotating;
        onRotating?.Invoke(isRotating);
    }

    public void ProcessRotateInput(Vector2 _value)
    {
        targetYaw += _value.x * yawSpeed * Time.deltaTime;
        targetPitch = ClampPitch(targetPitch + _value.y * pitchSpeed * Time.deltaTime * ((pitchInverted) ? 1f : -1f));
    }

    public void ProcessZoomInput()
    {
        currentState = states[(currentState == states[0] ? 1 : 0)];
        if(currentState == states[0])
        {
            player.Characters[0].Show();
            player.Characters[1].Hide();
            player.Characters[2].Hide();
        }
        else
        {
            player.Characters[0].Hide();
            player.Characters[1].Show();
            player.Characters[2].Show();
        }
        pitchAxis.transform.localPosition = currentState.Offset;
        cam.transform.localPosition = Vector3.back * currentState.Distance;
        transform.position = (currentState.Target == CameraState.CameraTarget.Root) ? player.transform.position : player.Characters[0].Head.transform.position;
    }

    float ClampPitch(float _value)
    {
        return Mathf.Clamp(_value, currentState.PitchLimits.x, currentState.PitchLimits.y);
    }

    public bool IsRotating { get { return isRotating; } }

    #region UI Methods
    void OnHorizontalSensitivityChanged(float _value)
    {
        yawSpeed = _value;
        PlayerPrefs.SetFloat("mouse horizontal", _value);
        PlayerPrefs.Save();
    }

    void OnVerticalSensitivityChanged(float _value)
    {
        pitchSpeed = _value;
        PlayerPrefs.SetFloat("mouse vertical", _value);
        PlayerPrefs.Save();
    }
    #endregion
}
