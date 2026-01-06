using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave{
        public GameObject[] enemyTypes;//types
        public int enemyCount;//number of enemies to spawn
    }
    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;
    private float timeBetweenSpawn = 1f;
    public Transform roomTransform;
    public Room room;
    public GameObject tpStar;

    public GameObject spawnCloudPrefab;
    private bool playerInRoom = false;
    public int GetWaveIndex()
    {
        return currentWaveIndex;
    }   
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartWave());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRoom = true;
        }
}
    // Update is called once per frame
    IEnumerator StartWave()
    {
        // Wait until player enters the room
        yield return new WaitUntil(() => playerInRoom);

        while (currentWaveIndex < waves.Length)
        {
            Wave wave = waves[currentWaveIndex];

            for (int i = 0; i < wave.enemyCount; i++)
            {
                SpawnEnemy(wave.enemyTypes);
                room.RefreshEnemies();
                enemiesAlive++;
                yield return new WaitForSeconds(timeBetweenSpawn);
            }

            // wait until all enemies defeated
            while (enemiesAlive > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(timeBetweenWaves);

            if (currentWaveIndex == waves.Length)
            {
                GetComponentInParent<LightChanger>().OnFinalWaveClearedSmooth();
            }
            else
            {
                room.TriggerClearEffects();
            }

            currentWaveIndex++;
        }

        Debug.Log("All waves completed");
        
        if (tpStar != null)
        {
            tpStar.SetActive(true);
        }
    }


    void SpawnEnemy(GameObject[] enemyTypes)
    {
        if (enemyTypes.Length == 0) return;

        GameObject enemyPrefab = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];
        Transform spawnPoint;
        
        //if this is the first wave and the first enemy, use a fixed spawn point
        if (currentWaveIndex == 0 && enemiesAlive == 0)
        {
            //will use first spawn point
            spawnPoint = spawnPoints[0];
        }
        else
        {
            spawnPoint = spawnPoints[UnityEngine.Random.Range(1, spawnPoints.Length)];
        }
        GameObject cloud = Instantiate(spawnCloudPrefab, spawnPoint.position, Quaternion.identity);
        Destroy(cloud, 2f); // lasts 1 second

        GameObject enemy = WorldSpawner.Spawn(enemyPrefab, spawnPoint.position, roomTransform);
        enemy.GetComponent<Enemy>().OnDeath += EnemyDefeated;
    }


    void EnemyDefeated(){
        enemiesAlive--;
    }
}
