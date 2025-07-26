using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    [SerializeField] private Color outlineColor;
    private SpriteRenderer spriteRenderer;
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void LateUpdate()
    {
        spriteRenderer.color = outlineColor;
    }
    public void SetOutlineColor(Color color)
    {
        outlineColor = color;
    }
    public void DisableOutline()
    {
        outlineColor.a = 0f;
    }

    public void EnableOutline()
    {
        outlineColor.a = 1;
    }

}
