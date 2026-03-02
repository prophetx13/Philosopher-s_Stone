using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MajorArcanaData", menuName = "Scriptable Objects/MajorArcanaData")]
public class MajorArcanaData : ScriptableObject
{
    public string arcanaName;
    public int arcanaNumber;

    public Material arenaMaterialSet;
    public AudioClip musicTrack;

    public float tractionModifier;
    public float boostModifier;
    public float driftModifier;

    public GameObject levelPrefab;

    public List<MinorArcanaModifier> suitModifiers;
}
