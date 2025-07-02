using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetoTarget : MonoBehaviour
{
    public Transform target;
    private Vector3 originalPosition;
    private bool isMoving = false;
    private bool hasMoved = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isMoving && !hasMoved)
        {
            StartCoroutine(MoveToPosition(target.position, 1f)); 
        }
    }

    IEnumerator MoveToPosition(Vector3 destination, float duration)
    {
        isMoving = true;
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, destination, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
        isMoving = false;

        if (!hasMoved)
        {
            hasMoved = true;
            yield return new WaitForSeconds(8f);
            StartCoroutine(MoveToPosition(originalPosition, 1f)); 
        }
        else
        {
            hasMoved = false;
        }
    }
}


