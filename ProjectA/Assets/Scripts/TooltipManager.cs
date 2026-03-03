using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private Enemy enemy;
    [SerializeField] private UI embraceTooltip;
    [SerializeField] private UI shootTooltip;
    [SerializeField] private UI dashTooltip;

    public void DisableEmbraceTooltip()
    {
        embraceTooltip.CloseWindow(embraceTooltip.gameObject);
    }

    public void EnableEmbraceTooltip()
    {
        embraceTooltip.OpenWindow(embraceTooltip.gameObject);
    }
    public void EnableDashTooltip()
    {
        dashTooltip.OpenWindow(dashTooltip.gameObject);
    }
    public void DisableDashTooltip()
    {
        dashTooltip.CloseWindow(dashTooltip.gameObject);
    }

    public void DisableShootTooltip()
    {
        shootTooltip.CloseWindow(shootTooltip.gameObject);
    }

    public void EnableShootTooltip()
    {
        shootTooltip.OpenWindow(shootTooltip.gameObject);
    }
}
