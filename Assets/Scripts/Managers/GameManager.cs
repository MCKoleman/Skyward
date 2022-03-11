using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : Singleton<GameManager>
{
    public enum GameState { INVALID = 0, MENU = 1, OVERWORLD = 2, PAUSED = 3, IN_DUNGEON = 4, IN_BOSS_ROOM = 5, IN_DIALOGUE = 6, GENERATING_DUNGEON = 7, LOADING_LEVEL = 8 }

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

    // Communicates scene changing to necessary components, such as the dungeon generator
    public void HandleSceneChange(GlobalVars.SceneType sceneType)
    {
        switch(sceneType)
        {
            case GlobalVars.SceneType.MENU:
                SetGameState(GameState.MENU);
                break;
            case GlobalVars.SceneType.DUNGEON:
            case GlobalVars.SceneType.BOSS:
                DungeonManager.Instance.StartDungeon();
                break;
            default:
                break;
        }
    }

    // Swaps the level to the given level
    public void HandleLevelSwap(int newLevelIndex)
    {
        // TODO: Save any necessary information from previous level
        EndGame();

        sceneLoader.LoadSceneWithId(newLevelIndex);
    }

    // Sets the time scale of the game to the given float
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    // Quits the game
    public void QuitGame()
    {
        sceneLoader.Quit();
    }

    // Sets the gameState to the given gameState. This function should be managed carefully
    public void SetGameState(GameState _gameState, bool savePrev = false)
    {
#if UNITY_EDITOR
        Debug.Log($"Changing gamestate. Previous: [{prevGameState.ToString()}], New: [{_gameState.ToString()}]");
#endif

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
        switch (gameState)
        {
            // Pause the game for states where the game should be paused
            case GameState.LOADING_LEVEL:
            case GameState.GENERATING_DUNGEON:
            case GameState.PAUSED:
                SetTimeScale(0.0f);
                break;
            case GameState.MENU:
                SetTimeScale(1.0f);
                break;
            // For every other situation, do nothing but unpause
            default:
                SetTimeScale(1.0f);
                break;
        }
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
