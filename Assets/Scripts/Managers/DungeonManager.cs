using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    [SerializeField]
    private DungeonRoomList roomList;
    [SerializeField]
    private DungeonRoom startRoom;

    [SerializeField]
    private Dictionary<string, RoomNode> roomNodes = new Dictionary<string, RoomNode>();
    [SerializeField]
    private List<RoomNode> spawnNodes = new List<RoomNode>();

    // Should only be called by GameManager. Initializes the singleton
    public void Init()
    {
        // If there is no start room, find one
        if(startRoom == null)
        {
            startRoom = GameObject.FindGameObjectWithTag("DungeonRoom").GetComponent<DungeonRoom>();
        }

        // Generate the dungeon
        Generate(startRoom);
    }

    // Generates the dungeon starting from the given room
    public void Generate(DungeonRoom room)
    {
        // Keep track of the latest node used
        GameObject latestRoom = null;
        
        // Keep a list of all nodes that have been spawned and a list of all nodes that should spawn
        roomNodes.Clear();
        spawnNodes.Clear();

        // Add the spawn nodes of the first room to the dictionary
        AddNodesToDict(roomNodes, spawnNodes, room.roomNodes);

        // Keep generating until all nodes have been exhausted
        while(spawnNodes.Count > 0)
        {
            // Pop the first spawnNode
            RoomNode tempNode = spawnNodes[0];
            //Print.Log($"Running loop on node: [{tempNode}]");

            // Find a random room that meets the spawn requirements
            GameObject tempRoomPrefab = roomList.GetRandomRoomByFlag(tempNode.reqFlag);
            if(tempRoomPrefab != null)
            {
                latestRoom = Instantiate(tempRoomPrefab, tempNode.transform.position, Quaternion.identity, PrefabManager.Instance.levelHolder);
                DungeonRoom tempDungeonRoom = latestRoom.GetComponent<DungeonRoom>();
                tempDungeonRoom.theme = room.theme;
                AddNodesToDict(roomNodes, spawnNodes, tempDungeonRoom.roomNodes);
            }
            // Mark the node as completed and remove it from the list
            tempNode.hasSpawned = true;
            spawnNodes.RemoveAt(0);

            // Remove all nodes that have been exhausted
            TrimSpawnedNodes(spawnNodes);
        }

        // When generation is done, spawn an exit in the last room spawned
        Instantiate(PrefabManager.Instance.exitPrefab, latestRoom.transform.position, Quaternion.identity, PrefabManager.Instance.levelHolder);
    }

    // Remove all nodes that have already spawned from the list
    public void TrimSpawnedNodes(List<RoomNode> spawnNodes)
    {
        // Find all nodes that have already spawned or don't need to spawn
        for(int i = spawnNodes.Count - 1; i >= 0; i--)
        {
            // If the node doesn't need to spawn or has already spawned, flag it for deletion
            if(!spawnNodes[i].shouldSpawn || spawnNodes[i].hasSpawned)
                spawnNodes.RemoveAt(i);
        }
    }

    // Adds all the nodes from the given list to the dictionary, combining duplicates
    public void AddNodesToDict(Dictionary<string, RoomNode> roomNodes, List<RoomNode> spawnNodes, List<RoomNode> roomList)
    {
        // Check each node against both dicts before adding it
        for(int i = 0; i < roomList.Count; i++)
        {
            // Get the key of the current node
            string tempKey = roomList[i].GetKey();
            RoomNode tempNode = roomList[i];

            // If the list already has a node with the given key, combine them
            if (roomNodes.ContainsKey(tempKey))
            {
                roomNodes[tempKey].CombineNodes(tempNode);
                tempNode = roomNodes[tempKey];
            }
            // If the list does not have a node with the given key, add it
            else
            {
                roomNodes.Add(tempKey, tempNode);
            }

            // If the room is valid for spawning, add it to the spawn list
            if(!tempNode.hasSpawned && tempNode.shouldSpawn && !spawnNodes.Contains(tempNode))
            {
                spawnNodes.Add(tempNode);
            }
        }
    }
}
