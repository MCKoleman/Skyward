using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Animator transition;
    [SerializeField]
    private float transitionTime = 1.0f;
    [SerializeField]
    private bool skipStartAnim = false;

    private void Start()
    {
        // Skip the start animation by disabling the gameObject that it is on
        if (skipStartAnim)
            transition.gameObject.SetActive(false);
    }

    // Loads the next scene if it exists
    public void LoadNextScene()
    {
        // Loads the next available scene or the main menu if no scene exists
        Print.Log(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1) != null);
        int index = SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1) != null ? SceneManager.GetActiveScene().buildIndex + 1 : 0;
        LoadSceneWithId(index);
    }

    // Loads the scene with the given ID
    public void LoadSceneWithId(int level)
    {   
        // Reset time to unpause
        Time.timeScale = 1.0f;
        GameManager.Instance.SetIsGameActive(false);
        SaveManager.Instance.EndGame();

        // Only enable the cursor in the main menu
        //Cursor.visible = (level == 0);

        // Only play the transition if it exists
        if (transition != null)
            StartCoroutine(HandleLoadLevel(level));
        // If the transition does not exist, swap immediately
        else
            LoadLevel(level);
    }
    
    // Loads the given scene while playing the correct animation
    private IEnumerator HandleLoadLevel(int level)
    {
        transition.gameObject.SetActive(true);
        // Play animation
        transition.SetTrigger("Start");

        // Wait for animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Change scene
        LoadLevel(level);
    }

    // Loads the given level
    private void LoadLevel(int level)
    {
        GameManager.Instance.SetIsGameActive(true);
        SceneManager.LoadScene(level);
    }

    // Quits the game
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
