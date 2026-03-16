using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBeamManager : MonoBehaviour
{
    private Beam beam;
    [SerializeField] private Slider chargeBar;
    // Start is called before the first frame update
    void Start()
    {
        beam = FindObjectOfType<Beam>();
    }

    // Update is called once per frame
    void Update()
    {
        if (beam.IsCharging())
        {
            chargeBar.gameObject.SetActive(true);
            chargeBar.value = Mathf.Clamp(beam.GetHeldTime() / beam.chargeTimeRequired, 0, 1);
        }
        else
        {
            chargeBar.gameObject.SetActive(false);
        }
        
    }
}
