using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Paused,
    Playing
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentState;
    void Awake()
    {
        // persistance
        if (instance != null && instance != this)
        {
             //if there is destroy it
            Destroy(gameObject);
            return;
        }
        //if theres no instance of this orig, create a new one
        instance = this;
    }
    private PlayerMovement playerMovement;
    private Healthbar playerHealth;
    public GameOverScreen gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }


    public void SetState(GameState state)
    {
        currentState = state;
        switch (state)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;
        }
        Debug.Log(currentState);
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
