using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : Singleton<GameManager>
{
    public bool IsGameActive { get; private set; }

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
        StartGame();
    }

    // Starts the game
    public void StartGame()
    {
        SetIsGameActive(true);
        UIManager.Instance.InitHUD();
        Time.timeScale = 1.0f;
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

    // Loads the next available level
    public void HandleNextLevel()
    {
        sceneLoader.LoadNextScene();
    }

    public void QuitGame()
    {
        sceneLoader.Quit();
    }

    // Getters and setters
    public void SetIsGameActive(bool state) { IsGameActive = state; }
}
