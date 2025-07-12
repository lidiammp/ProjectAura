using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinStar : MonoBehaviour
{
    public WinScreen winScreen;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            winScreen.SetUp();
        }
        
    }
}
