using System.Collections.Generic;
using UnityEngine;

public class CharacterAvatar : MonoBehaviour
{
    [SerializeField] bool combineRenderers;

    Transform head;
    Transform spine;

    void Awake()
    {
        if (combineRenderers) CombineRenderers();

        foreach (BodyPart _part in GetComponentsInChildren<BodyPart>())
        {
            switch (_part.Part)
            {
                case BodyPart.BodyPartType.Head:
                    head = _part.transform;
                    break;
                case BodyPart.BodyPartType.Spine:
                    spine = _part.transform;
                    break;
            }
        }
    }

    void CombineRenderers()
    { 
        SkinnedMeshRenderer _mainRend = GetComponentInChildren<SkinnedMeshRenderer>();

        foreach(SkinnedMeshRenderer _rend in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
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
}
