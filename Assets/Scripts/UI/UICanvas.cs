using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICanvas : MonoBehaviour
{
    Canvas canvas;

    public virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public virtual void Start()
    {

    }

    public bool IsOpen { get { return canvas.enabled; } }

    public Canvas Canvas { get { return canvas; } }
}
