using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject cavePostProcess;
    [SerializeField]
    private GameObject castlePostProcess;
    [SerializeField]
    private GameObject skyPostProcess;
    [SerializeField]
    private GlobalVars.DungeonTheme theme;

    private void Start()
    {
        SwitchPostProcess();
    }
    
    // Sets the theme of the post processing handler
    public void SetTheme(GlobalVars.DungeonTheme _theme)
    {
        this.theme = _theme;
        SwitchPostProcess();
    }

    // Switches the active post processing to be the theme that is being used
    public void SwitchPostProcess()
    {
        cavePostProcess.SetActive(theme == GlobalVars.DungeonTheme.CAVE);
        castlePostProcess.SetActive(theme == GlobalVars.DungeonTheme.CASTLE);
        skyPostProcess.SetActive(theme == GlobalVars.DungeonTheme.SKY);
    }
}
