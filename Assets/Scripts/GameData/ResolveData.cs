using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

[CreateAssetMenu(fileName = "ResolveVariant", menuName = "GameData/Resolve", order = 1)]
public class ResolveData : ScriptableObject
{
    public string LocalKey;
    public ResolveType Type;
    public bool IsPositive;
}
