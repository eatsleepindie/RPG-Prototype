using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCanvas : UICanvas
{
    [SerializeField] Slider healthBar;

    Character character;

    public override void Awake()
    {
        base.Awake();

        character = GetComponentInParent<Character>();
    }

    public override void Start()
    {
        base.Start();

        Debug.Log("START");
        character.onDamageApplied += UpdateHealth;
    }

    void UpdateHealth()
    {
        //Debug.Log("UP HEALTH");
        healthBar.value = character.Stats.Health;
    }
}
