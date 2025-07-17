using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Enemy[] totalEnemies;
    public string roomName;
    [SerializeField] private int aliveEnemies = 0;
    // Start is called before the first frame update
    void Start()
    {
        totalEnemies = GetComponentsInChildren<Enemy>();

        //for each enemy in enemy if they die add to enemy died
        foreach (Enemy e in totalEnemies)
        {
            //if enemy dies, --alive enemies
            e.OnDeath += OnEnemyDied;
        }

        //alive enemies will be total
        aliveEnemies = totalEnemies.Length;
    }

    void OnEnemyDied()
    {
        aliveEnemies--;
        Debug.Log($"Enemy died in {roomName}. Enemies left: {aliveEnemies}");

        if (aliveEnemies <= 0)
        {
            Debug.Log($"{roomName} is cleared!");
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
}
