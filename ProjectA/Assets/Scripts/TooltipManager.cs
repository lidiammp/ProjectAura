using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private Enemy enemy;
    [SerializeField] private GameObject embraceTooltip;
    [SerializeField] private GameObject shootTooltip;
    [SerializeField] private GameObject dashTooltip;
    void Start()
    {

    }

    public void DisableEmbraceTooltip()
    {
        embraceTooltip.SetActive(false);
    }

    public void EnableEmbraceTooltip()
    {
        embraceTooltip.SetActive(true);
    }
    public void EnableDashTooltip()
    {
        dashTooltip.SetActive(true);
    }
    public void DisableDashTooltip()
    {
        dashTooltip.SetActive(false);
    }

    public void DisableShootTooltip()
    {
        shootTooltip.SetActive(false);
    }

    public void EnableShootTooltip()
    {
        shootTooltip.SetActive(true);
    }
}
