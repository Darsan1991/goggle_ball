using System.Collections.Generic;
using UnityEngine;

public class RandomLevelCreatorPrefabSelector : LevelCreatorPrefabSelector
{
    [SerializeField] private List<RectEnvironment> _prefabs = new List<RectEnvironment>();

    public override ICycleTile GetSelectedPrefab()
    {
        return _prefabs.Count == 0? null : _prefabs[Random.Range(0, _prefabs.Count)];
    }
}