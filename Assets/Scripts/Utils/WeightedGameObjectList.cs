using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedGameObjectList
{
    [Header("Wave Spawn Information")]
    [SerializeField]
    private List<WeightedGameObject> list = new List<WeightedGameObject>();

    public WeightedGameObject Get(int index) { return list[index]; }
    public GameObject GetObject(int index) { return list[index].prefab; }
    public int GetWeight(int index) { return list[index].weight; }

    public int Count() { return list.Count; }
    public void Clear() { list.Clear(); }

    public void Add(WeightedGameObject obj) { list.Add(obj); }
    public void Add(GameObject obj, int weight) { list.Add(new WeightedGameObject(obj, weight));}
    public void RemoveAt(int index) { list.RemoveAt(index); }
    public bool Remove(WeightedGameObject obj) { return list.Remove(obj); }

    public bool Remove(GameObject obj)
    {
        foreach(WeightedGameObject tempObj in list)
        {
            if(tempObj.prefab == obj)
            {
                return list.Remove(tempObj);
            }
        }
        return false;
    }

    // Returns a reference to a random game object prefab by weight
    public GameObject GetRandomObject()
    {
        int index = 0;
        int rng = Random.Range(0, GetTotalWeights()+1);

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

    // Gets the total weight of all game objects in wave
    public int GetTotalWeights()
    {
        int totalWeight = 0;
        foreach (var obj in list)
            totalWeight += obj.weight;
        return totalWeight;
    }
}
