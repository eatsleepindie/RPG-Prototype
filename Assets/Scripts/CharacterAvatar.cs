using System.Collections.Generic;
using UnityEngine;

public class CharacterAvatar : MonoBehaviour
{
    [SerializeField] CombineMode combineMode;

    [Space(5f)]
    [Header("Debug")]
    [SerializeField] bool debug;

    Character character;
    Transform head;
    Transform spine;

    void Awake()
    {
        character = GetComponentInParent<Character>();

        if(combineMode != CombineMode.None)
            CombineRenderers();

        foreach (BodyPart _part in GetComponentsInChildren<BodyPart>())
        {
            switch (_part.Type)
            {
                case CharacterInfo.CharacterAvatarPartType.Head:
                    head = _part.transform;
                    break;
                case CharacterInfo.CharacterAvatarPartType.Torso:
                    spine = _part.transform;
                    break;
            }
        }
    }

    void CombineRenderers()
    {

        foreach (CharacterInfo.CharacterAvatarPart _part in character.Info.AvatarParts)
        {
            if (_part.Side == CharacterInfo.CharacterAvatarPartSide.Left && combineMode == CombineMode.Right) continue;
            if (_part.Side == CharacterInfo.CharacterAvatarPartSide.Right && combineMode == CombineMode.Left) continue;
            GameObject _obj = Instantiate(_part.Mesh, transform);
            SkinnedMeshRenderer _rend = _obj.GetComponentInChildren<SkinnedMeshRenderer>();
            BodyPart _bodyPart = _rend.gameObject.AddComponent(typeof(BodyPart)) as BodyPart;
            _bodyPart.Type = _part.Type;
            _bodyPart.Side = _part.Side;

            if (debug)
            {
                foreach (Material _mat in _rend.materials)
                    _mat.color = _part.DebugColor;
            }
        }

        GameObject _prefabObj = Instantiate(character.Info.Prefabs[((int)combineMode) - 1], transform);

        SkinnedMeshRenderer _mainRend = _prefabObj.GetComponentInChildren<SkinnedMeshRenderer>();
        _mainRend.enabled = false;

        foreach(SkinnedMeshRenderer _rend in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            _rend.updateWhenOffscreen = true;
            if (_rend == _mainRend) continue;
            CopyBonesWithDictionary(_mainRend, _rend);
        }
    }

    public static void CopyBonesWithDictionary(SkinnedMeshRenderer _input, SkinnedMeshRenderer _output)
    {
        Transform _outputParent = _output.transform.parent;

        Dictionary<string, Transform> _boneMap = new Dictionary<string, Transform>();
        foreach (Transform _bone in _input.bones)
            _boneMap[_bone.name] = _bone;

        Transform[] _boneArray = _output.bones;
        for (int idx = 0; idx < _boneArray.Length; ++idx)
        {
            string boneName = _boneArray[idx].name;
            if (false == _boneMap.TryGetValue(boneName, out _boneArray[idx]))
            {
                Debug.LogError("failed to get bone: " + boneName);
            }
        }
        _output.bones = _boneArray;

        _output.transform.SetParent(_input.transform.parent);
        _output.rootBone = _input.rootBone;
        Destroy(_outputParent.gameObject);
    }

    public Transform Head { get { return head; } }

    public Transform Spine { get { return spine; } }

    public enum CombineMode
    {
        None,
        Full,
        Left,
        Right
    }
}
