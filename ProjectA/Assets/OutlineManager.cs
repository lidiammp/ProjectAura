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
        spriteRenderer.color = outlineColor;
        DisableOutline();
    }

    void Update()
    {

    }

    public void EnableOutline()
    {
        outlineColor.a = 1f;
        spriteRenderer.color = outlineColor;
    }

    public void DisableOutline()
    {
        outlineColor.a = 0f;
        spriteRenderer.color = outlineColor;
    }

    public void DisableSprite()
    {
        spriteRenderer.enabled = false;
    }

    public void EnableSprite()
    {
        spriteRenderer.enabled = true;
    }
}
