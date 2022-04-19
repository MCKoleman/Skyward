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
    private LevelProgressList levelProgressList;
    [SerializeField]
    private DungeonRoom startRoom;
    [SerializeField]
    private Vector3 bossSpawnOffset;

    [Header("Runtime information")]
    [SerializeField]
    private List<MinimapNode> roomMap = new List<MinimapNode>();
    [SerializeField]
    private DungeonRoom currentRoom;
    [SerializeField]
    private int numRooms = 0;
    [SerializeField]
    private int curLevel = 0;
    [SerializeField]
    private LevelProgressList.LevelProgress curLevelInfo;
    [SerializeField]
    private bool isGenerated = false;

    [Header("Dungeon Info")]
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

    private Vector2 revealedBL = Vector2.zero;
    private Vector2 revealedTR = Vector2.zero;
    private CameraController cameraController;
    private const float MINIMAP_SIZE_MOD = 0.55f;

    // Room spawn data
    private Dictionary<string, RoomNode> roomNodes = new Dictionary<string, RoomNode>();
    private List<RoomNode> spawnNodes = new List<RoomNode>();



    // Should only be called by GameManager. Initializes the singleton
    public void Init()
    {
        curLevel = 1;
        UIManager.Instance.SetLevelNum(curLevel);
    }

    // Starts the dungeon generation process for the current level.
    public void StartDungeon()
    {
        cameraController = Camera.main?.GetComponent<CameraController>();

        // Set theme
        curLevelInfo = levelProgressList.GetCurrentLevel(curLevel);
        GameObject.FindGameObjectWithTag("PostProcess").GetComponent<PostProcessHandler>().SetTheme(curLevelInfo.theme);
        
        // Get start room
        if (curLevelInfo.sceneType == GlobalVars.SceneType.DUNGEON)
        {
            GameObject tempObj = Instantiate(PrefabManager.Instance.baseRoomPrefab, Vector3.zero, Quaternion.identity, PrefabManager.Instance.levelHolder);
            startRoom = tempObj.GetComponent<DungeonRoom>();
        }
        // Get boss room
        else if(curLevelInfo.sceneType == GlobalVars.SceneType.BOSS || curLevelInfo.sceneType == GlobalVars.SceneType.MINIBOSS)
        {
            GameObject tempObj = Instantiate(PrefabManager.Instance.bossRoomPrefab, Vector3.zero, Quaternion.identity, PrefabManager.Instance.levelHolder);
            startRoom = tempObj.GetComponent<DungeonRoom>();
        }
        startRoom.theme = curLevelInfo.theme;

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
        GameObject latestRoom = startRoom.gameObject;
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
        roomSize = room.GetSize();
        Debug.Log($"Dungeon room size is [{roomSize}]");

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

            // Find a random room that meets the spawn requirements
            GameObject tempRoomPrefab = SelectRoomPrefab(minRooms, maxRooms, prefRooms, tempNode);
            //Debug.Log($"Selected room [{tempRoomPrefab}] for flag [{tempNode.ReqFlag}]");
            
            // Spawn the room chosen
            if(tempRoomPrefab != null)
            {
                latestRoom = Instantiate(tempRoomPrefab, tempNode.transform.position, Quaternion.identity, PrefabManager.Instance.levelHolder);
                
                // Update dungeon room information
                DungeonRoom tempDungeonRoom = latestRoom.GetComponent<DungeonRoom>();
                roomSize = tempDungeonRoom.GetSize();
                tempDungeonRoom.theme = room.theme;
                tempDungeonRoom.roomNum = numRooms;
                tempDungeonRoom.roomPos = new Vector2Int(
                    Mathf.FloorToInt(latestRoom.transform.position.x / roomSize.x), 
                    Mathf.FloorToInt(latestRoom.transform.position.z / roomSize.y));

                AddNodesToDict(roomNodes, spawnNodes, tempDungeonRoom.roomNodes);

                // Combine rooms that want to be combined
                tempDungeonRoom.CheckCombination();

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

        // When generation is done, spawn an exit
        SpawnExit(latestRoom);
        isGenerated = true;
    }

    // Selects a room prefab to spawn that meets the current criteria
    private GameObject SelectRoomPrefab(int minRooms, int maxRooms, int prefRooms, RoomNode node)
    {
        GameObject tempPrefab = null;

        // First check if a special room should be attempted (either it is required, or there is space and a random chance succeeds)
        if (GlobalVars.DoesRequireSpecialReq(node.ReqFlag) || (prefRooms > numRooms + spawnNodes.Count && roomList.ShouldAttemptSpecialRoom()))
            tempPrefab = roomList.GetRandomSpecialRoomByFlag(node.ReqFlag);

        // If a suitable special room was not found or searched for, keep going
        if (tempPrefab != null)
            return tempPrefab;

        // If the minimum number of rooms hasn't been met yet and there are less spawn nodes than needed rooms, spawn max rooms
        if (minRooms > numRooms + spawnNodes.Count)
            return roomList.GetRandomRoomByFlag(GlobalVars.LockInverseFlagReqs(node.ReqFlag));

        // If the preferred number of rooms hasn't been met yet, spawn with inverted spawn rates
        else if (prefRooms > numRooms + spawnNodes.Count)
            return roomList.GetInverseRandomRoomByFlag(node.ReqFlag);

        // If the preferred number of rooms has been met, spawn normally until approaching max rooms
        else if (maxRooms > numRooms + spawnNodes.Count)
            return roomList.GetRandomRoomByFlag(node.ReqFlag);

        // If approaching max rooms, force stop
        else
            return roomList.GetRandomRoomByFlag(GlobalVars.LockFlagReqs(node.ReqFlag));
    }

    // Remove all nodes that have already spawned from the list
    private void TrimSpawnedNodes(List<RoomNode> spawnNodes)
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
    private void AddNodesToDict(Dictionary<string, RoomNode> roomNodes, List<RoomNode> spawnNodes, List<RoomNode> roomList)
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

    // Sets the current room to the given room
    public void SetCurrentRoom(DungeonRoom newRoom)
    {
        currentRoom = newRoom;

        // Set camera to follow new room
        if (newRoom != null)
            cameraController.SetRoom(newRoom);

        // Reveal the room if it hasn't been revealed yet
        if (!currentRoom.isRevealed)
        {
            currentRoom.RevealFogOfWar();
            UpdateMinimapCamera();
        }
    }

    // Returns the room node with the given key
    public DungeonRoom GetRoomWithKey(string key)
    {
        // Don't search an empty key
        if (key == "")
            return new DungeonRoom();

        // Search roomMap for a matching key
        for(int i = 0; i < roomMap.Count; i++)
        {
            // If a matching room is found, return it
            if (roomMap[i].room.GetKey() == key)
                return roomMap[i].room;
        }

        return new DungeonRoom();
    }
    
    // Updates the minimap camera each time a new room is set
    private void UpdateMinimapCamera()
    {
        if (currentRoom == null)
            return;

        // Get the size of the dungeon
        revealedBL = Vector2.Min(revealedBL, currentRoom.blBound);
        revealedTR = Vector2.Max(revealedTR, currentRoom.trBound);

        Vector2 tempSize = revealedTR - revealedBL;
        UIManager.Instance.SetMinimapDungeonCenter(new Vector3((revealedBL.x + revealedTR.x) / 2.0f, 0.0f, (revealedBL.y + revealedTR.y) / 2.0f));
        UIManager.Instance.SetMinimapCameraWidth(Mathf.Max(tempSize.x, tempSize.y) * MINIMAP_SIZE_MOD);
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

    // Spawns a way to exit the dungeon
    private void SpawnExit(GameObject latestRoom)
    {
        switch(curLevelInfo.sceneType)
        {
            case GlobalVars.SceneType.DUNGEON:
                Instantiate(PrefabManager.Instance.exitPrefab, latestRoom.transform.position, Quaternion.identity, PrefabManager.Instance.levelHolder);
                break;
            case GlobalVars.SceneType.MINIBOSS:
                Instantiate(PrefabManager.Instance.GetMiniboss(curLevelInfo.theme), bossSpawnOffset, Quaternion.identity, PrefabManager.Instance.bossHolder);
                break;
            case GlobalVars.SceneType.BOSS:
                Instantiate(PrefabManager.Instance.mainBossPrefab, bossSpawnOffset, Quaternion.identity, PrefabManager.Instance.bossHolder);
                break;
            case GlobalVars.SceneType.MENU:
            default:
                break;
        }
    }

    // Continues level progression
    public void ProgressToNextLevel()
    {
        curLevel++;
        UIManager.Instance.SetLevelNum(levelProgressList.GetLevelMinusMiniBosses(curLevel));
    }

    // Resets the current level number
    public void ResetCurLevel() { curLevel = 0; }

    // Returns the number of rooms in this dungeon
    public int GetNumRooms() { return numRooms; }
    // Returns the current level number
    public int GetLevelNum() { return curLevel; }
    // Returns the next scene type
    public GlobalVars.SceneType GetNextSceneType() { return levelProgressList.GetNextSceneType(curLevel); }
}
