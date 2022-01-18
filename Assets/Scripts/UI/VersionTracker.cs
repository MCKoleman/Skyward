using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersionTracker : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().text = "v" + Application.version;
    }
}
