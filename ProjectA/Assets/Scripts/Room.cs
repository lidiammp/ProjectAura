using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Enemy[] totalEnemies;
    public string roomName;
    [SerializeField] private int aliveEnemies = 0;

    private bool isCleared = false;
    [SerializeField] private SparkleManager sparkleManager;
    [SerializeField] private LightChanger lightManager;
    [SerializeField] private DoorController[] doorControllers;
    // Start is called before the first frame update

    void Start()
    {
        // sparkleManager = GetComponentinChildren<SparkleManager>();
    }
    
    public void StartRoom()
    {
        // Close all doors
        foreach (DoorController door in doorControllers)
        {
            door.SetOpen(false);
        }

        // Setup enemies
        totalEnemies = GetComponentsInChildren<Enemy>();
        aliveEnemies = totalEnemies.Length;

        foreach (Enemy e in totalEnemies)
        {
            e.OnDeath -= OnEnemyDied; // Avoid double-subscription
            e.OnDeath += OnEnemyDied;
        }
    }
    void OnEnemyDied()
    {
        aliveEnemies--;
        CheckRoomClear();
        // Debug.Log($"Enemy died in {roomName}. Enemies left: {aliveEnemies}");

        // if (aliveEnemies <= 0)
        // {
        // Debug.Log($"{roomName} is cleared!");
        // }
    }
    private void CheckRoomClear()
    {
        if (aliveEnemies <= 0 && !isCleared)
        {
            isCleared = true;
            sparkleManager.PlaySparkles();
            lightManager.SetClearedLight();
            foreach (DoorController door in doorControllers) {
                door.SetOpen(isCleared);
            }
        }
    }
    public int GetAliveEnemies()
    {
        return aliveEnemies;
    }

    public int GetTotalEnemies()
    {
        return totalEnemies.Length;
    }
    public void RefreshEnemies()
    {
        totalEnemies = GetComponentsInChildren<Enemy>();

        aliveEnemies = 0;

        foreach (Enemy e in totalEnemies)
        {
            // Prevent duplicate event subscriptions
            e.OnDeath -= OnEnemyDied;
            e.OnDeath += OnEnemyDied;
            aliveEnemies++;
        }


        // Debug.Log($"[{roomName}] Refreshed enemy list. Alive: {aliveEnemies}, Total: {totalEnemies.Length}");
    }

}
