using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print : MonoBehaviour
{
    /// <summary>
    /// Logs a message to the Unity console, but hiding the printline.
    /// </summary>
    /// <param name="message"></param>
    public static void Log(object message)
    {
        Debug.Log(message + "\n\n");
    }

    /// <summary>
    /// A variant of Debug.Log that logs an error message to the Unity console, but hiding the printline.
    /// </summary>
    /// <param name="message"></param>
    public static void LogError(object message)
    {
        Debug.LogError(message + "\n\n");
    }

    /// <summary>
    /// A variant of Debug.Log that logs a warning message to the Unity console, but hiding the printline.
    /// </summary>
    /// <param name="message"></param>
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message + "\n\n");
    }
}
