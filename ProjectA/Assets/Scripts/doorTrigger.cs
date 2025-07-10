using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    public GameObject Door;
    private Renderer renderer;

     void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
    }
    // void OnMouseDown()
    void Update()
    {
        // if (Door != null)
        if (Input.GetKeyDown(KeyCode.E) && Door != null)
        {
            MeshRenderer mesh = Door.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                mesh.enabled = false;
                renderer.material.color = Color.green;
            }
        }
    }

}
