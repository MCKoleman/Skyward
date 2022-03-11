using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedObjectList<T>
{
    [Header("Weighted Objects")]
    [SerializeField]
    private List<WeightedObject<T>> list = new List<WeightedObject<T>>();

    public WeightedObject<T> Get(int index)
    {
        if (index < 0 || index >= list.Count)
            return new WeightedObject<T>(default(T), -1);
        return list[index];
    }

    public T GetObject(int index)
    {
        if (index < 0 || index >= list.Count)
            return default(T);
        return list[index].obj;
    }

    public int GetWeight(int index)
    {
        if (index < 0 || index >= list.Count)
            return -1;
        return list[index].weight;
    }

    public int Count() { return list.Count; }
    public void Clear() { list.Clear(); }

    public void Add(WeightedObject<T> obj) { list.Add(obj); }
    public void Add(T obj, int weight) { list.Add(new WeightedObject<T>(obj, weight)); }
    public void RemoveAt(int index) { list.RemoveAt(index); }
    public bool Remove(WeightedObject<T> obj) { return list.Remove(obj); }

    public bool Remove(T obj)
    {
        foreach (WeightedObject<T> tempObj in list)
        {
            if (tempObj.obj.Equals(obj))
            {
                return list.Remove(tempObj);
            }
        }
        return false;
    }

    // Returns a reference to a random game object prefab by weight
    public T GetRandomObject()
    {
        int index = 0;
        int rng = Random.Range(0, GetTotalWeights() + 1);

        // Iterate through wave enemies until weight limit is reached
        for (; index < list.Count; index++)
        {
            // If random number generated is greater than the weight of the object at index, move forward
            int weight = GetWeight(index);
            if (rng > weight)
                rng -= weight;
            // If rng is too low to move on, break and return the index
            else
                break;
        }

        return GetObject(index);
    }

    // Returns a reference to a random game object prefab by inverted weight
    public T GetInverseRandomObject()
    {
        int index = 0;
        int rng = Random.Range(0, GetInverseTotalWeights() + 1);
        int totalWeights = GetTotalWeights();

        // Iterate through wave enemies until weight limit is reached
        for (; index < list.Count; index++)
        {
            // If random number generated is greater than the weight of the object at index, move forward
            int weight = totalWeights - GetWeight(index);
            if (rng > weight)
                rng -= weight;
            // If rng is too low to move on, break and return the index
            else
                break;
        }

        return GetObject(index);
    }

    // Gets the total weight of all game objects in the list
    public int GetTotalWeights()
    {
        int totalWeight = 0;
        foreach (var obj in list)
            totalWeight += obj.weight;
        return totalWeight;
    }

    // Gets the inverted total weight of all game objects in the list
    public int GetInverseTotalWeights()
    {
        int total = GetTotalWeights();
        int totalWeight = 0;
        foreach (var obj in list)
            totalWeight += total - obj.weight;
        return totalWeight;
    }
}
