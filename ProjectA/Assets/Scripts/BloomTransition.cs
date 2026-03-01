using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BloomTransition : MonoBehaviour
{
    public Volume volume;                    // Assign your Global Volume here
    public float bloomIncreaseSpeed = 5f;    // How fast bloom rises
    public float targetBloomIntensity = 20f; // Max bloom before switching
    public string nextSceneName;

    private Bloom bloom;
    
    void Start()
    {
        // Try to get Bloom from Volume
        if (volume.profile.TryGet(out bloom))
        {
            bloom.intensity.value = 0f; // Optional: start from no bloom
        }
        else
        {
            Debug.LogError("No Bloom override found in Volume!");
        }
    }

    public void StartTransition()
    {
        StartCoroutine(BloomRiseThenChange());
    }

    IEnumerator BloomRiseThenChange()
    {
        if (bloom == null) yield break;

        while (bloom.intensity.value < targetBloomIntensity)
        {
            bloom.intensity.value += bloomIncreaseSpeed * Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
