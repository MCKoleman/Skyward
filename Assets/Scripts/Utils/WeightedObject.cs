using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedObject<T>
{
    public WeightedObject(T _obj, int _weight)
    {
        weight = _weight;
        obj = _obj;
    }

    public int weight;
    public T obj;
}
