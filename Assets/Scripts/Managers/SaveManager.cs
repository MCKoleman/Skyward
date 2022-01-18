using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField]
    private LevelDataList levelDataList;
    [SerializeField]
    private LevelUserDataMap levelUserDataMap;

    [Header("Current level info")]
    // New map info
    [SerializeField]
    private LevelUserDataStruct currentLevelUserData;
    [SerializeField]
    private LevelInfoStruct currentLevel;

    [SerializeField]
    private int currentLevelIndex;
    [SerializeField]
    private float currentTime;
    [SerializeField]
    private int currentCollectibles;
    [SerializeField]
    private int currentDeaths;
    [SerializeField]
    private bool isLevelActive;

    private const float TIMER_INCREMENT = 0.01f;

    [System.Serializable]
    public class GameSave
    {
        public GameSave(LevelUserDataMap _levelUserMap)
        {
            levelUserMap = _levelUserMap.DeepCopy();
        }

        public LevelUserDataMap levelUserMap;
    }

    // Initializes the score manager
    public void Init()
    {
        // Init scores
        GameSave tempSave = LoadGameData();

        // Reset scores and state
        currentDeaths = 0;
        currentCollectibles = 0;
        currentTime = 0.0f;

        // Update HUD

        // Get the build index of the current scene
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        Print.Log($"Loaded scene with build index [{currentLevelIndex}]");

        // Load important info from save
        if (tempSave != null)
        {
            // Make sure level map is valid
            if (levelUserDataMap == null)
                levelUserDataMap = new LevelUserDataMap();

            // If the saved data is not null, copy it
            if (tempSave.levelUserMap != null)
                levelUserDataMap = tempSave.levelUserMap.DeepCopy();

            // Don't do anything for main menu
            if(currentLevelIndex > 0)
            {
                // If the levels list has the current level, load it
                if (currentLevelIndex - 1 < levelDataList.Count)
                    currentLevel = levelDataList[currentLevelIndex];
                // If the levels list does not have the current level, tell the developer
                else
                    Print.LogWarning($"Attempted to access level [{currentLevelIndex}] but that level does not have levelData. Was a new level created without modifying the LevelData scriptable object?");

                // If the levelMap has the current level, load it
                if (levelUserDataMap.ContainsLevel(currentLevelIndex))
                    currentLevelUserData = levelUserDataMap[currentLevelIndex];
                // If the levelMap does not have the current level, create it
                else
                    currentLevelUserData = levelUserDataMap.CreateNewLevel(currentLevelIndex);
            }
        }

        // Start level timer as long as not in main menu
        if (currentLevelIndex != 0)
            StartCoroutine(LevelTimer());
    }

    // Start timer and handle loop
    private IEnumerator LevelTimer()
    {
        // Reset info
        currentTime = 0.0f;
        isLevelActive = true;

        // Run timer until level is over
        while(isLevelActive)
        {
            yield return new WaitForSeconds(TIMER_INCREMENT);
            currentTime += TIMER_INCREMENT;
            //UIManager.Instance.UpdateTimeDisplay(currentTime);
        }
    }

    // Ends the game and saves the current score
    public void EndGame()
    {
        // End timer
        isLevelActive = false;
        StopCoroutine(LevelTimer());
    }

    // Wins the game and saves the scores
    public void WinLevel()
    {
        // End timer
        isLevelActive = false;
        StopCoroutine(LevelTimer());
        
        // Save highscores
        SaveGame();
    }

    // Saves the current game to file
    public void SaveGame()
    {
        // Don't save game data from main menu
        if(currentLevelIndex > 0)
        {
            // Save the current user data into the current level
            bool newBestTime = currentLevelUserData.SetBestTime(currentTime);
            bool newBestCollectibles = currentLevelUserData.SetBestCollectibles(currentCollectibles);
            bool newBestDeaths = currentLevelUserData.SetBestDeaths(currentDeaths);

            // Mark the level as completed
            currentLevelUserData.SetCleared(true);
            currentLevelUserData.SetRating(CalculateRating());

            // Save the current level into the list
            Print.Log($"Saving current level [{currentLevel.levelID}]. Deaths: [{currentLevelUserData.bestDeaths}], Collectibles: [{currentLevelUserData.collectiblesFound}], Time: [{currentLevelUserData.bestTime}]");
            Print.Log($"Best performances? Death: [{newBestDeaths}], Collectible: [{newBestCollectibles}], Time: [{newBestTime}]");

            levelUserDataMap[currentLevelIndex] = currentLevelUserData;

            // Save data
            SaveGameData(new GameSave(levelUserDataMap));

            // Update Victory Screen
            //UIManager.Instance.SetVictoryScreenInfo(currentDeaths, currentCollectibles, currentLevel.numCollectibles, currentTime, CalculateRating(), newBestDeaths, newBestTime, newBestCollectibles);
        }
    }

    // Saves the game
    private void SaveGameData(GameSave gameSave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save_game.dat");
        bf.Serialize(file, gameSave);
        file.Close();
    }

    // Loads the game
    private GameSave LoadGameData()
    {
        if (File.Exists(Application.persistentDataPath + "/save_game.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save_game.dat", FileMode.Open);
            GameSave gameSave = (GameSave)bf.Deserialize(file);
            file.Close();

            Print.Log(string.Format("File loaded from path: {0}{1}", Application.persistentDataPath, "/save_game.dat"));

            return gameSave;
        }
        else
        {
            Print.Log(string.Format("File doesn't exist at path: {0}{1}", Application.persistentDataPath, "/save_game.dat"));
            return null;
        }
    }

    // Increases the amount of saved deaths
    public void IncDeaths()
    {
        currentDeaths++;
        //UIManager.Instance.UpdateDeathDisplay(currentDeaths);
    }

    // Increases the amount of saved collectibles
    public void IncCollectibles()
    {
        currentCollectibles++;
        //UIManager.Instance.UpdateCollectibleDisplay(currentCollectibles);
    }

    // Calculates the rating on the level
    public char CalculateRating()
    {
        // If performance was perfect, return an S
        if (currentTime < currentLevel.parTime && currentDeaths == 0 && (currentCollectibles == currentLevel.numCollectibles || currentLevel.numCollectibles <= 0))
            return 'S';
        // If the player had otherwise perfect performance, but didn't beat the level in time return A
        else if (currentDeaths == 0 && currentCollectibles == currentLevel.numCollectibles)
            return 'A';
        else if ((currentDeaths == 0 && currentTime < currentLevel.parTime * 1.5f)
            || (currentDeaths <= 2 && currentTime < currentLevel.parTime)
            || (currentTime < currentLevel.parTime * 1.5f && currentCollectibles == currentLevel.numCollectibles))
            return 'B';
        else if ((currentDeaths == 0 && currentTime < currentLevel.parTime * 2.0f)
            || (currentDeaths <= 4 && currentTime < currentLevel.parTime)
            || (currentTime < currentLevel.parTime * 2.0f && currentCollectibles == currentLevel.numCollectibles))
            return 'C';
        else if ((currentDeaths == 0 && currentTime < currentLevel.parTime * 3.0f)
            || (currentTime < currentLevel.parTime)
            || (currentTime < currentLevel.parTime * 3.0f && currentCollectibles == currentLevel.numCollectibles))
            return 'D';
        else
            return 'E';
    }

    // Returns the number of levels in the game
    public int GetLevelCount() { return levelDataList.Count; }

    // Returns whether the current time is above par
    public bool IsAboveParTime() { return currentTime < currentLevel.parTime; }

    // Returns whether the current time is the best time
    public bool IsBestTime() { return currentTime < currentLevelUserData.bestTime || currentLevelUserData.bestTime <= 0.0f; }

    // Returns whether the current deaths is the best amount
    public bool IsBestDeaths() { return currentDeaths < currentLevelUserData.bestDeaths || currentDeaths == 0; }

    // Returns whether the current collectibles is all from the level and non-zero
    public bool IsAllCollectibles() { return currentCollectibles >= currentLevelUserData.collectiblesFound && currentCollectibles != 0; }

    // Returns the level data list
    public LevelDataList GetLevelDataList() { return levelDataList; }

    // Returns the level map
    public LevelUserDataMap GetLevelUserData() { return levelUserDataMap; }

    // Updates the UI display of the player score
    public void RefreshHUD() { UIManager.Instance.RefreshHUD(); }
}
