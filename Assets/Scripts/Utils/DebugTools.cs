using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugTools : MonoBehaviour
{
#if UNITY_EDITOR
    /* ================================================== Constants ============================================================= */
    private const float POWERUP_SPAWN_Y = 5.0f;
    private const float ENEMY_SPAWN_Y = 17.0f;

    /* ================================================== Powerups ============================================================= */
    /*
    [MenuItem("DebugTools/Powerups/2x Score #1")]
    private static void CreateScorePowerup2x()
    {
        CreatePowerup("Score2xPowerup");
        Debug.Log("Created a 2x score multiplier powerup");
    }

    [MenuItem("DebugTools/Powerups/4x Score %#1")]
    private static void CreateScorePowerup4x()
    {
        CreatePowerup("Score4xPowerup");
        Debug.Log("Created a 4x score multiplier powerup");
    }

    [MenuItem("DebugTools/Powerups/8x Score #2")]
    private static void CreateScorePowerup8x()
    {
        CreatePowerup("Score8xPowerup");
        Debug.Log("Created a 8x score multiplier powerup");
    }

    [MenuItem("DebugTools/Powerups/Heart #3")]
    private static void CreateHeartPowerup()
    {
        CreatePowerup("HeartPowerup");
        Debug.Log("Created a heart powerup");
    }

    [MenuItem("DebugTools/Powerups/Super Heart %#3")]
    private static void CreateSuperHeartPowerup()
    {
        CreatePowerup("SuperHeartPowerup");
        Debug.Log("Created a super heart powerup");
    }

    [MenuItem("DebugTools/Powerups/Shield #4")]
    private static void CreateShieldPowerup()
    {
        CreatePowerup("ShieldPowerup");
        Debug.Log("Created a shield powerup");
    }

    [MenuItem("DebugTools/Powerups/Super Shield %#4")]
    private static void CreateSuperShieldPowerup()
    {
        CreatePowerup("SuperShieldPowerup");
        Debug.Log("Created a super shield powerup");
    }

    [MenuItem("DebugTools/Powerups/Drain Shield #5")]
    private static void CreateDrainShieldPowerup()
    {
        CreatePowerup("DrainShieldPowerup");
        Debug.Log("Created a drain shield powerup");
    }

    [MenuItem("DebugTools/Powerups/Super Drain Shield %#5")]
    private static void CreateSuperDrainShieldPowerup()
    {
        CreatePowerup("SuperDrainShieldPowerup");
        Debug.Log("Created a super drain shield powerup");
    }

    [MenuItem("DebugTools/Powerups/Shotgun #q")]
    private static void CreateShotgunPowerup()
    {
        CreatePowerup("ShotgunPowerup");
        Debug.Log("Created a shotgun powerup");
    }

    [MenuItem("DebugTools/Powerups/Super Shotgun %#q")]
    private static void CreateSuperShotgunPowerup()
    {
        CreatePowerup("SuperShotgunPowerup");
        Debug.Log("Created a super shotgun powerup");
    }

    [MenuItem("DebugTools/Powerups/Machinegun #w")]
    private static void CreateMachinegunPowerup()
    {
        CreatePowerup("MachineGunPowerup");
        Debug.Log("Created a machinegun powerup");
    }

    [MenuItem("DebugTools/Powerups/Super Machinegun %#w")]
    private static void CreateSuperMachinegunPowerup()
    {
        CreatePowerup("SuperMachineGunPowerup");
        Debug.Log("Created a super machinegun powerup");
    }

    [MenuItem("DebugTools/Powerups/Chain Lightning #e")]
    private static void CreateChainLightningPowerup()
    {
        CreatePowerup("ChainLightningPowerup");
        Debug.Log("Created a chain lightning powerup");
    }

    [MenuItem("DebugTools/Powerups/Super Chain Lightning %#e")]
    private static void CreateSuperChainLightningPowerup()
    {
        CreatePowerup("SuperChainLightningPowerup");
        Debug.Log("Created a super chain lightning powerup");
    }

    [MenuItem("DebugTools/Powerups/Nuke #r")]
    private static void CreateNukePowerup()
    {
        CreatePowerup("NukePowerup");
        Debug.Log("Created a nuke powerup");
    }
    */

    /* ================================================== Enemies ============================================================= */
    /*
    [MenuItem("DebugTools/Enemies/Swarm #&1")]
    private static void CreateSwarm()
    {
        CreateEnemy("SwarmEnemy");
        Debug.Log("Created a swarm enemy");
    }

    [MenuItem("DebugTools/Enemies/Cannon #&2")]
    private static void CreateCannon()
    {
        CreateEnemy("CannonEnemy");
        Debug.Log("Created a cannon enemy");
    }

    [MenuItem("DebugTools/Enemies/Charger #&3")]
    private static void CreateCharger()
    {
        CreateEnemy("ChargerEnemy");
        Debug.Log("Created a charger enemy");
    }

    [MenuItem("DebugTools/Enemies/Super Swarm #&4")]
    private static void CreateSuperSwarm()
    {
        CreateEnemy("StrongSwarmEnemy");
        Debug.Log("Created a super swarm enemy");
    }

    [MenuItem("DebugTools/Enemies/Super Cannon #&5")]
    private static void CreateSuperCannon()
    {
        CreateEnemy("StrongCannonEnemy");
        Debug.Log("Created a super cannon enemy");
    }

    [MenuItem("DebugTools/Enemies/Super Charger #&6")]
    private static void CreateSuperCharger()
    {
        CreateEnemy("StrongChargerEnemy");
        Debug.Log("Created a super charger enemy");
    }
    */

    /* ================================================== Bosses ============================================================= */
    /*
    [MenuItem("DebugTools/Enemies/Laser Boss %#&1")]
    private static void CreateLaserBoss()
    {
        CreateEnemy("LaserBossEnemy");
        Debug.Log("Created a laser boss enemy");
    }
    */

    /* ================================================== Helper Functions ============================================================= */
    /*
    [MenuItem("DebugTools/General/Increment Wave %&1")]
    private static void MoveToNextWave()
    {
        //SpawnManager.Instance.IncrementWave();
    }

    // Instantiates a prefab with the given name at the position DEBUG_PREFAB_POS
    private static void CreatePowerup(string name)
    {
        var loadedObj = Resources.Load("Prefabs/Powerups/" + name);
        Instantiate(loadedObj, new Vector3(0.0f, POWERUP_SPAWN_Y, 0.0f), Quaternion.identity);
    }

    private static void CreateEnemy(string name)
    {
        var loadedObj = Resources.Load("Prefabs/Enemies/" + name);
        Instantiate(loadedObj, new Vector3(0.0f, ENEMY_SPAWN_Y, 0.0f), Quaternion.identity);
    }
    */

    [MenuItem("DebugTools/SpawnExit #1")]
    private static void SpawnExit()
    {
        var loadedObj = Resources.Load("Prefabs/Rooms/RoomComponents/Exit");
        Instantiate(loadedObj, new Vector3(2.0f, 0.0f, 0.0f), Quaternion.identity);
    }
#endif
}
