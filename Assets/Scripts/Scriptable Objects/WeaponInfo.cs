using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "Scriptable Objects/WeaponInfo")]
public class WeaponInfo : ScriptableObject
{
    [MinMaxSlider(0f, 100f)]
    public Vector2 Damage;

    [Space(10f)]
    [Header("Animator")]
    public WeaponAnimatorLayer[] AnimatorLayers;

    [System.Serializable]
    public class WeaponAnimatorLayer
    {
        public Character.AnimatorLayer Layer;
        public float Weight;
        public float Damping;
    }
}
