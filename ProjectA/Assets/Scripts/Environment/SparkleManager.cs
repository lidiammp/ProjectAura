using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleManager : MonoBehaviour
{    
    private ParticleSystem sparkleSystem;
    void Start()
    {
        sparkleSystem = GetComponent<ParticleSystem>();
    }
    public void PlaySparkles()
    {
        if (!sparkleSystem.isPlaying)
            sparkleSystem.Play();
    }

    public void StopSparkles()
    {
        if (sparkleSystem.isPlaying)
            sparkleSystem.Stop();
    }

    public void EmitBurst(int amount)
    {
        sparkleSystem.Emit(amount);
    }

}
