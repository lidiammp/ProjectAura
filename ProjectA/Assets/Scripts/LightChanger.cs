using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : MonoBehaviour
{

    [SerializeField] private float test;
    [SerializeField] private Color startLight;
    [SerializeField] private Color clearedLight;
    private Light[] roomLights;
    private Room room;

    //ratio of enemies
    void Start()
    {
        room = GetComponent<Room>();
        roomLights = GetComponentsInChildren<Light>();
    }
    void Update()
    {
        //get room stuff
        int alive = room.GetAliveEnemies();
        int total = room.GetTotalEnemies();

        if (total <= 0) return;
        //get ratio on enemies
        float ratio = (float)alive / total;
        float t = Mathf.Clamp01(1f - ratio);
        //set the room lights
        Color targetColor = Color.Lerp(startLight, clearedLight, t);
        SmoothSetAllLights(targetColor);
    }

    private void SmoothSetAllLights(Color targetColor)
    {
        foreach (var l in roomLights)
        {
            if (l != null)
            {
                l.color = Color.Lerp(l.color, targetColor, Time.deltaTime * 2f);
                // Multiply by 2f to adjust speed
            }
        }
    }
    public void SetClearedLight()
    {
        SmoothSetAllLights(clearedLight);
    }
}
