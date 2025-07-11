using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmbraceManager : MonoBehaviour
{
    private PlayerEmbrace playerEmbrace;
    [SerializeField] private GameObject embraceWheel;
    [SerializeField] private Image fill;
    // Start is called before the first frame update
    void Start()
    {
        playerEmbrace = FindObjectOfType<PlayerEmbrace>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEmbrace.IsCharging())
        {
            embraceWheel.gameObject.SetActive(true);
            fill.fillAmount = Mathf.Clamp(playerEmbrace.GetHeldTime() / playerEmbrace.chargeTime, 0, 1.10f);
        }
        else
        {
            embraceWheel.gameObject.SetActive(false);
        }
        
    }
}
