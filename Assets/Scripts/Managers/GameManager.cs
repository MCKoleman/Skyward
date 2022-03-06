using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : Singleton<GameManager>
{
    public enum GameState { MENU, OVERWORLD, PAUSED, IN_DUNGEON, IN_BOSS_ROOM, IN_DIALOGUE, GENERATING_DUNGEON }
    public bool IsGameActive { get; private set; }
    [SerializeField]
    private GameState gameState;

    private SceneLoader sceneLoader;

    // Initialize all other singletons
    void Start()
    {
        sceneLoader = this.GetComponent<SceneLoader>();
        this.Init();
    }

    // Initializes the game manager
    public void Init()
    {
        PrefabManager.Instance.Init();
        AudioManager.Instance.Init();
        UIManager.Instance.Init();
        SaveManager.Instance.Init();
        CheckpointManager.Instance.Init();
        DialogueManager.Instance.Init();
        DungeonManager.Instance.Init();
        StartGame();
    }

    // Starts the game
    public void StartGame()
    {
        SetIsGameActive(true);
        UIManager.Instance.InitHUD();
        SetTimeScale(1.0f);
        Print.Log("Started game");
    }

    // Ends the game 
    public void EndGame()
    {
        Print.Log("Ended game");
        SetIsGameActive(false);
        SaveManager.Instance.EndGame();
        PrefabManager.Instance.ResetLevel();
    }

    // Restarts the game
    public void RestartGame()
    {
        EndGame();
        this.Init();
        StartGame();
    }

    // Swaps the level to the given level
    public void HandleLevelSwap(int newLevelIndex)
    {
        // TODO: Save any necessary information from previous level

        sceneLoader.LoadSceneWithId(newLevelIndex);
    }

    // Sets the time scale of the game to the given float
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    // Loads the next available level
    public void HandleNextLevel()
    {
        sceneLoader.LoadNextScene();
    }

    // Quits the game
    public void QuitGame()
    {
        sceneLoader.Quit();
    }

    // Getters and setters
    public void SetIsGameActive(bool state) { IsGameActive = state; }
    public GameState GetGameState() { return gameState; }

    // Sets the gameState to the given gameState. This function should be managed carefully
    public void SetGameState(GameState _gameState) { gameState = _gameState; }
}
