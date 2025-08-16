using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    [Header("References")]
    public GameObject insideCastlePart; // Drag your object here
    public GameObject island;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            insideCastlePart.SetActive(true);  // Show when player enters
            island.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            insideCastlePart.SetActive(false); // Hide when player leaves
            island.SetActive(true);
        }
    }
}
