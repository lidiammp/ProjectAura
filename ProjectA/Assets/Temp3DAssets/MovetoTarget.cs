using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetoTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    bool move = false;
   
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            move = true;
        }
        if (target != null && move)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 2f);
        }
    }
}
