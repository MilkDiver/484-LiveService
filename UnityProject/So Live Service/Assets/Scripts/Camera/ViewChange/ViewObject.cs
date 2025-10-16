using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ViewObject", menuName = "Scriptable Objects/ViewObject")]
public class ViewObject : ScriptableObject
{
    public List<SectionInformation> sectionList;

    public float mapID;
    public Vector3 pointsOffset;
}

[Serializable]
public class SectionInformation
{
    [field: SerializeField] public SightMode sight { get; private set; }

    [field: SerializeField] public Color sightColor { get; private set; }

    //
    [field: SerializeField] public string visibleLayer { get; private set; }

    //
    [field: SerializeField] public string invisibleLayer { get; private set; }

    [field: SerializeField] public string effectLayer { get; private set; }
}
