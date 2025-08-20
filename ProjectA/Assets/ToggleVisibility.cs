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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int targetLayer = LayerMask.NameToLayer(defaultLayerName);
            if (targetLayer == -1)
            {
                Debug.LogError("Layer not found: " + hiddenLayerName);
                return;
            }
            // insideCastlePart.SetActive(true);  // Show when player enters
            SetLayerRecursively(transform, LayerMask.NameToLayer(defaultLayerName));
            island.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int targetLayer = LayerMask.NameToLayer(hiddenLayerName);
            if (targetLayer == -1)
            {
                Debug.LogError("Layer not found: " + hiddenLayerName);
                return;
            }
            // insideCastlePart.SetActive(false); // Hide when player leaves
            SetLayerRecursively(transform, LayerMask.NameToLayer(hiddenLayerName));
            island.SetActive(true);
        }
    }
}
