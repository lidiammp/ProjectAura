using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<enemy> enemiesInTrigger = new List<enemy>();

    public void AddEnemy (enemy enemy)
    {
        enemiesInTrigger.Add(enemy);
    }

     public void RemoveEnemy (enemy enemy)
    {
        enemiesInTrigger.Remove(enemy);
    }
}
