using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public Healthbar playerHealth;
    List<HealthHeart> hearts = new List<HealthHeart>();
    private void OnEnable()
    {
        Healthbar.OnPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        Healthbar.OnPlayerDamaged -= DrawHearts;
    }
    void Start()
    {
        DrawHearts();
    }
    public void DrawHearts()
    {
        ClearHearts();
        //determine amount of hearts
        //based of max health
        float maxHealthRemainder = playerHealth.GetMaxHealth() % 2;
        int heartsToMake = (int)((playerHealth.GetMaxHealth() / 2) + maxHealthRemainder);
        // make hearts
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }
        //fill em up using math i dont understand
        for (int i = 0; i < hearts.Count; i++)
        {
            int HeartStatusRemainder = (int)Mathf.Clamp(playerHealth.GetCurrentHealth() - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)HeartStatusRemainder);
        }
    }
    //clear the hearts
    public void ClearHearts()
    {
        //for each child gameobject
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

    //create hearts
    public void CreateEmptyHeart()
    {
        //create the heart
        GameObject newHeart = Instantiate(heartPrefab);
        //set it to be a child of this game object
        newHeart.transform.SetParent(transform, false);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }
}
