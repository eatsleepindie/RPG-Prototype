using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BodyPartHealthPanel : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text partNameLabel;

    CharacterCanvas canvas;
    BodyPart part;

    private void Start()
    {
        canvas = GetComponentInParent<CharacterCanvas>();
    }

    public void UpdatePanel()
    {
        healthSlider.value = part.Health;
    }

    public BodyPart Part
    {
        get { return part; }
        set
        {
            part = value;
            partNameLabel.text = string.Format(
                "{0} {1}",
                value.Type.ToString(),
                value.Side == CharacterInfo.CharacterAvatarPartSide.Full ? "" : value.Side.ToString()
                );
            value.onDamage += UpdatePanel;
        }
    }
}
