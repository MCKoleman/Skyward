using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : Singleton<GameManager>
{
    public enum GameState { INVALID = 0, MENU = 1, OVERWORLD = 2, PAUSED = 3, IN_DUNGEON = 4, IN_BOSS_ROOM = 5, IN_DIALOGUE = 6, GENERATING_DUNGEON = 7 }

    [SerializeField]
    private GameState gameState;
    [SerializeField]
    private GameState prevGameState;
    public GameState State { get { return gameState; } }
    public bool IsGameActive { get; private set; }

    private SceneLoader sceneLoader;

    // Initialize all other singletons
    void Start()
    {
        sceneLoader = this.GetComponent<SceneLoader>();
        gameState = GameState.INVALID;
        prevGameState = GameState.INVALID;
        this.Init();
    }

    // Initializes the game manager and all singletons
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

    // Sets the gameState to the given gameState. This function should be managed carefully
    public void SetGameState(GameState _gameState, bool savePrev = false)
    {
        // If the previous game state should be preserved, update it
        if (savePrev)
            prevGameState = this.gameState;
        // If the previous game state should not be preserved, update it to the current one
        else
            prevGameState = _gameState;

        gameState = _gameState;
        EvaluateGameStateEffects();
    }

    // Revers the gameState to the previous gameState. If there is no previous game state or the previous gameState is invalid, don't do anything
    public void RevertToPreviousGameState()
    {
        if (prevGameState != GameState.INVALID)
            gameState = prevGameState;

        EvaluateGameStateEffects();
    }

    // Acts on the effects of changing the gamestate
    private void EvaluateGameStateEffects()
    {
        // If the game is not paused, timeScale should be 1. If the game is paused, the timeScale should be 0
        SetTimeScale(gameState != GameState.PAUSED ? 1.0f : 0.0f);
    }

    // Returns whether pausing is allowed currently based on the current gameState
    public bool CanPause()
    {
        switch(gameState)
        {
            // Only allow pausing in situations where the game is "active"
            case GameState.IN_DUNGEON:
            case GameState.IN_BOSS_ROOM:
            case GameState.OVERWORLD:
            case GameState.PAUSED:
                return true;
            // For every other situation, disable pausing
            default:
                return false;
        }
    }

    // Getters and setters
    public void SetIsGameActive(bool state) { IsGameActive = state; }
}
