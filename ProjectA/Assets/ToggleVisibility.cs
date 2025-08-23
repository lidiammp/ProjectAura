using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    [Header("References")]
    public string hiddenLayerName = "HiddenFromCamera"; // Layer to hide the room
    public string defaultLayerName = "Default";         // Original layer
    public GameObject island;

    private void SetLayerRecursively(Transform parent, int layer)
    {
        // skip enemy
        if (parent.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
            parent.gameObject.layer == LayerMask.NameToLayer("Attack"))
        {
            return;
        }

        parent.gameObject.layer = layer;

        foreach (Transform child in parent)
        {
            SetLayerRecursively(child, layer);
        }
    }
    
    void SetEnemyVisible(GameObject enemy, bool visible)
    {
        foreach (var sr in enemy.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = visible;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show room
            SetLayerRecursively(transform, LayerMask.NameToLayer(defaultLayerName));
            island.SetActive(false);

            // Show enemies
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                SetEnemyVisible(enemy, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide room
            SetLayerRecursively(transform, LayerMask.NameToLayer(hiddenLayerName));
            island.SetActive(true);

            // Hide enemies
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                SetEnemyVisible(enemy, false);
            }
        }
    }

}
