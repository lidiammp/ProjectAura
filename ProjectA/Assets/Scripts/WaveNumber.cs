using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WaveNumber : MonoBehaviour
{
    // Start is called before the first frame update
    private EnemySpawner enemySpawner;
    public TextMeshProUGUI Text;

    int waveNumber;
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        waveNumber = enemySpawner.GetWaveIndex()+1;
        Text.text = "WAVE# " + waveNumber;
    }
}
