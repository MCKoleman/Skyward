using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedGameObject
{
    public WeightedGameObject(GameObject _obj, int _weight)
    {
        weight = _weight;
        prefab = _obj;
    }

    public int weight;
    public GameObject prefab;
}
