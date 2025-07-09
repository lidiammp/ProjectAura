using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Healthbar playerHealth;
    public GameOverScreen gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        playerHealth = playerMovement.GetComponent<Healthbar>();
        if (playerHealth.Dead())
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        gameOverScreen.RestartButton();
    }
    // Update is called once per frame
    
}
