using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private Enemy enemy;
    [SerializeField] private GameObject embraceTooltip;
    [SerializeField] private GameObject shootTooltip;
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

    public void DisableShootTooltip()
    {
        shootTooltip.SetActive(false);
    }

    public void EnableShootTooltip()
    {
        shootTooltip.SetActive(true);
    }
}
