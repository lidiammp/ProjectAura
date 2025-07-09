using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TakeDamage : MonoBehaviour
{
    public float intensity = 0f;
    UnityEngine.Rendering.Universal.Vignette vignette;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        
        if(!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        //vignette.intensity.Override(0.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void VignetteEffect()
    {
        StartCoroutine(TakeDamageEffect());
    }
    private IEnumerator TakeDamageEffect()
    {
        intensity = 0.4f;
        vignette.intensity.Override(0.4f);
        yield return new WaitForSeconds(0.4f);
        while (intensity > 0)
        {
            intensity -= 0.01f;
            if (intensity < 0)
            {
                intensity = 0;
            }
            vignette.intensity.Override(intensity);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
}
