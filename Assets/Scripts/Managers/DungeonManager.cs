using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    [Header("Spawn information")]
    [SerializeField]
    private DungeonRoomList roomList;
    [SerializeField]
    private DungeonContentList contentList;
    [SerializeField]
    private DungeonRoom startRoom;
    [SerializeField]
    private GlobalVars.DungeonTheme startTheme;

    [Header("Runtime information")]
    [SerializeField]
    private List<MinimapNode> roomMap = new List<MinimapNode>();
    [SerializeField]
    private int numRooms = 0;
    [SerializeField]
    private bool isGenerated = false;
    [SerializeField]
    private Vector2 roomSize = Vector2.zero;
    [SerializeField]
    private Vector2Int blCoord = new Vector2Int(int.MaxValue, int.MaxValue);
    [SerializeField]
    private Vector2Int trCoord = new Vector2Int(int.MinValue, int.MinValue);
    [SerializeField]
    private Vector2 dungeonCenter = Vector2.zero;
    [SerializeField]
    private Vector2 dungeonSize = Vector2.zero;

    // Room spawn data
    private Dictionary<string, RoomNode> roomNodes = new Dictionary<string, RoomNode>();
    private List<RoomNode> spawnNodes = new List<RoomNode>();



    // TEMP: REMOVE ASAP. DO NOT KEEP THIS UPDATE FUNCTION AROUND
    // ----------------------------------------------------------
    public Vector2 plPos = new Vector2();
    private GameObject player;
    private void Update()
    {
        // Don't do anything if the player does not exist
        if (player == null)
            return;

        plPos = new Vector2(
            (player.transform.position.x + dungeonCenter.x) / (dungeonSize.x * roomSize.x), 
            (player.transform.position.z + dungeonCenter.y) / (dungeonSize.y * roomSize.y));
    }
    // TEMP: REMOVE ASAP. DO NOT KEEP THIS UPDATE FUNCTION AROUND
    // ----------------------------------------------------------



    // Should only be called by GameManager. Initializes the singleton
    public void Init()
    {
    }

    // Starts the dungeon generation process for the current level.
    public void StartDungeon()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // If there is no start room, make one
        if (startRoom == null)
        {
            GameObject tempObj = Instantiate(PrefabManager.Instance.baseRoomPrefab, Vector3.zero, Quaternion.identity, PrefabManager.Instance.levelHolder);
            startRoom = tempObj.GetComponent<DungeonRoom>();
        }

        // Set theme
        startRoom.theme = startTheme;

        // Generate the dungeon
        StartCoroutine(AsyncGenerateHandle(startRoom));
    }

    // Asynchronously generates the dungeon
    private IEnumerator AsyncGenerateHandle(DungeonRoom room)
    {
        // Communicate beginning of generation to GameStateManager
        GameManager.Instance.SetGameState(GameManager.GameState.GENERATING_DUNGEON);
        UIManager.Instance.EnableLoadingScreen(true);

        // Start dungeon generation
        isGenerated = false;
        Generate(room);

        // Suspends the coroutine until the dungeon is fully generated
        yield return new WaitUntil(() => isGenerated);

        // Wait an extra second to allow time to transition
        yield return new WaitForSecondsRealtime(1.0f);

        // Communicate ending of generation to GameStateManager
        GameManager.Instance.StartGame();
        GameManager.Instance.SetGameState(GameManager.GameState.IN_DUNGEON);
        UIManager.Instance.EnableLoadingScreen(false);
    }

    // Generates the dungeon starting from the given room
    private void Generate(DungeonRoom room)
    {
        // Update loading UI
        UIManager.Instance.SetLoadingProgressText("Setting up dungeon");

        // Reset dungeon info
        GameObject latestRoom = null;
        numRooms = 1;
        blCoord = new Vector2Int(int.MaxValue, int.MaxValue);
        trCoord = new Vector2Int(int.MinValue, int.MinValue);

        // Clear previous dungeon
        roomMap.Clear();
        roomNodes.Clear();
        spawnNodes.Clear();

        // Get spawn parameters
        int minRooms = roomList.GetMinRoomCount();
        int maxRooms = roomList.GetMaxRoomCount();
        int prefRooms = roomList.GetPrefRoomCount();
        roomSize = room.CalcSize();

        // Keep a list of all nodes that have been spawned and a list of all nodes that should spawn
        // Add the spawn nodes of the first room to the dictionary
        AddNodesToDict(roomNodes, spawnNodes, room.roomNodes);

        // Update loading UI
        UIManager.Instance.SetLoadingProgressText("Generating dungeon");

        // Keep generating until all nodes have been exhausted
        while (spawnNodes.Count > 0)
        {
            // Pop the first spawnNode
            RoomNode tempNode = spawnNodes[0];
            //Print.Log($"Running loop on node: [{tempNode}]");

            // Find a random room that meets the spawn requirements
            GameObject tempRoomPrefab;
            
            // If the minimum number of rooms hasn't been met yet and there are less spawn nodes than needed rooms, spawn max rooms
            if(minRooms > numRooms + spawnNodes.Count)
                tempRoomPrefab = roomList.GetRandomRoomByFlag(GlobalVars.LockInverseFlagReqs(tempNode.reqFlag));

            // If the preferred number of rooms hasn't been met yet, spawn with inverted spawn rates
            else if (prefRooms > numRooms + spawnNodes.Count)
                tempRoomPrefab = roomList.GetInverseRandomRoomByFlag(tempNode.reqFlag);

            // If the preferred number of rooms has been met, spawn normally until approaching max rooms
            else if (maxRooms > numRooms + spawnNodes.Count)
                tempRoomPrefab = roomList.GetRandomRoomByFlag(tempNode.reqFlag);

            // If approaching max rooms, force stop
            else
                tempRoomPrefab = roomList.GetRandomRoomByFlag(GlobalVars.LockFlagReqs(tempNode.reqFlag));

            //Print.Log($"Picked prefab: [{tempRoomPrefab}]. Needs to satisfy [{tempNode.reqFlag}].
            //Locked: [{GlobalVars.LockFlagReqs(tempNode.reqFlag)}], InvLocked: [{GlobalVars.LockInverseFlagReqs(tempNode.reqFlag)}]");

            // Spawn the room chosen
            if(tempRoomPrefab != null)
            {
                latestRoom = Instantiate(tempRoomPrefab, tempNode.transform.position, Quaternion.identity, PrefabManager.Instance.levelHolder);
                
                // Update dungeon room information
                DungeonRoom tempDungeonRoom = latestRoom.GetComponent<DungeonRoom>();
                tempDungeonRoom.theme = room.theme;
                tempDungeonRoom.roomNum = numRooms;
                tempDungeonRoom.roomPos = new Vector2Int(
                    Mathf.FloorToInt(latestRoom.transform.position.x / roomSize.x), 
                    Mathf.FloorToInt(latestRoom.transform.position.z / roomSize.y));

                AddNodesToDict(roomNodes, spawnNodes, tempDungeonRoom.roomNodes);

                // Add room to minimap
                roomMap.Add(new MinimapNode(tempDungeonRoom));

                // Find bottom and top bounds of the map
                blCoord = new Vector2Int(Mathf.Min(blCoord.x, tempDungeonRoom.roomPos.x), Mathf.Min(blCoord.y, tempDungeonRoom.roomPos.y));
                trCoord = new Vector2Int(Mathf.Max(trCoord.x, tempDungeonRoom.roomPos.x), Mathf.Max(trCoord.y, tempDungeonRoom.roomPos.y));

                // Increment the current room number
                numRooms++;
            }
            // Mark the node as completed and remove it from the list
            tempNode.hasSpawned = true;
            spawnNodes.RemoveAt(0);

            // Remove all nodes that have been exhausted
            TrimSpawnedNodes(spawnNodes);
        }

        // Update loading UI
        UIManager.Instance.SetLoadingProgressText("Reticulating splines");

        // Calculate dungeon size information. 
        dungeonSize = trCoord - blCoord;
        dungeonCenter = dungeonSize / 2;

        // For finding the actual size of the dungeon, add 0.5 * 2 because coordinates are in the center of rooms.
        dungeonSize += Vector2.one;

        // When generation is done, spawn an exit in the last room spawned
        Instantiate(PrefabManager.Instance.exitPrefab, latestRoom.transform.position, Quaternion.identity, PrefabManager.Instance.levelHolder);
        isGenerated = true;
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

    // Spawns content for the given room
    public void SpawnContent(ContentNode node)
    {
        // Find random content for the room
        GameObject tempContent = contentList.GetRandomContent(node.GetParentRoom().roomNum / (float)numRooms, node.nodePlace);
        if (tempContent != null)
        {
            Instantiate(tempContent, node.transform.position, node.transform.rotation, node.GetParentRoom().transform);
        }
    }

    // Returns the number of rooms in this dungeon
    public int GetNumRooms() { return numRooms; }
}
