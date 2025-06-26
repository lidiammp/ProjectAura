using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveNumber : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject SpawnManager;
    private EnemySpawner enemySpawner;
    public TextMeshProUGUI Text;

    int waveNumber;
    void Start()
    {
        SpawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
        enemySpawner = SpawnManager.GetComponent<EnemySpawner>();
        Text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        waveNumber = enemySpawner.GetWaveIndex()+1;
        Text.text = "WAVE# " + waveNumber;
    }
}
