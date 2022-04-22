using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    // Exits the dungeon, completing it
    private void ExitDungeon()
    {
        if (DungeonManager.Instance.GetNextSceneType() != GlobalVars.SceneType.MENU || GameManager.Instance.GetIsInfinite())
            GameManager.Instance.HandleLevelSwap((byte)DungeonManager.Instance.GetNextSceneType());
        else
            UIManager.Instance.ShowEndScreen();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            ExitDungeon();
        }
    }
}
