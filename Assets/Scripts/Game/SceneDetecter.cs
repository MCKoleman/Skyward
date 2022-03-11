using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDetecter : MonoBehaviour
{
    [SerializeField]
    private GlobalVars.SceneType sceneType;

    void Start()
    {
        GameManager.Instance.HandleSceneChange(sceneType);
    }
}
