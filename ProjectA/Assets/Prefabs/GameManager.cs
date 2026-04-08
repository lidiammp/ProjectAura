using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Paused,
    Playing,
    GameOver
}
public class GameManager : Singleton<GameManager>
{
    private GameState currentState;
    public GameState CurrentState { get { return currentState; } }

    private void Start()
    {
        SetState(GameState.Playing);
    }


    public void SetState(GameState state)
    {
        currentState = state;
        switch (currentState)
        {
            case GameState.Paused:
                EventDispatcher.instance.SendEvent<PauseEvent>(new PauseEvent
                {
                    duration = -1f,
                    timeScale = 0f
                });
                break;
            case GameState.Playing:
                EventDispatcher.instance.SendEvent<ResumeEvent>( new ResumeEvent());
                break;
            case GameState.GameOver:
                Debug.Log("Game Over!");
                break;
        };
        Debug.Log(currentState.ToString());
    }



    
}
