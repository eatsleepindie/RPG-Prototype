using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterCanvas : UICanvas
{
    public static CharacterCanvas Instance;

    [SerializeField] BodyPartHealthPanel bodyPartPanel;
    [SerializeField] RectTransform bodyPartPanelsContainer;

    List<BodyPartHealthPanel> healthPanels = new List<BodyPartHealthPanel>();

    Character character;

    public override void Awake()
    {
        base.Awake();

        Instance = this;

        //character = GetComponentInParent<Character>();
    }

    public override void Start()
    {
        base.Start();

        //character.onDamageApplied += UpdateHealth;
    }

    void UpdateHealth()
    {
    }

    public Character Character
    { 
        get 
        { 
            return character; 
        } 
        set 
        { 
            for(int _i=healthPanels.Count - 1; _i>= 0; _i--)
            {
                healthPanels[_i].transform.SetParent(null);
                Destroy(healthPanels[_i].gameObject);
            }

            healthPanels.Clear();

            character = value;
            Debug.Log(character);
            Debug.Log(character.Avatar);
            Debug.Log(character.Avatar.GetBodyPartByType.Count);
            
            foreach(CharacterInfo.CharacterAvatarPart _part in character.Avatar.GetBodyPartByType.Keys)
            {
                BodyPartHealthPanel _panel = Instantiate(bodyPartPanel, bodyPartPanelsContainer);
                _panel.Part = character.Avatar.GetBodyPartByType[_part];
                healthPanels.Add(_panel);
            }
        } 
    }
}
