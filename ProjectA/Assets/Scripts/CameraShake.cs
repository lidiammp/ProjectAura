using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void ShakeRotation(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }
    public IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        //keep orig pos
        Quaternion originalRotation = transform.localRotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //random rotational angles
            float xAngle = Random.Range(-1f, 1f) * magnitude;
            float yAngle = Random.Range(-1f, 1f) * magnitude;
            float zAngle = Random.Range(-1f, 1f) * magnitude;

            //rotate it
            transform.localRotation = originalRotation * Quaternion.Euler(xAngle, yAngle, zAngle);

            elapsed += Time.deltaTime;
            yield return null;
        }
        //set rotation back to normal
        transform.localRotation = originalRotation;

    }

    //layers
    //player 
        //camera shake<----
            //camera other stuff
}
