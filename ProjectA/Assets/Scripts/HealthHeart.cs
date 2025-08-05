using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart, halfHeart, emptyHeart;
    Image heartImage;
    
    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }
    //set heart
    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;
        }
    }
    //clear hearts

}

public enum HeartStatus
{
    Empty = 0,
    Full = 2,
    Half = 1
}
