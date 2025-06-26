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

    public int GetWaveIndex(){
        return currentWaveIndex;
    }   
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartWave());
    }

    // Update is called once per frame
    IEnumerator StartWave(){
        while( currentWaveIndex < waves.Length){
            Wave wave = waves[currentWaveIndex];
            for(int i = 0; i < wave.enemyCount;i++){
                SpawnEnemy(wave.enemyTypes);
                enemiesAlive++;
                yield return new WaitForSeconds(0.5f);
            }
            while(enemiesAlive > 0){
                yield return null;
            }
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWaveIndex++;
            
        }
        Debug.Log("All waves completed");
    }

    void SpawnEnemy(GameObject[] enemyTypes){
        if(enemyTypes.Length == 0) return;

        GameObject enemyPrefab = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        GameObject enemy = WorldSpawner.Spawn(enemyPrefab, spawnPoint.position);
        enemy.GetComponent<Enemy>().OnDeath += EnemyDefeated;
    }

    void EnemyDefeated(){
        enemiesAlive--;
    }
}
