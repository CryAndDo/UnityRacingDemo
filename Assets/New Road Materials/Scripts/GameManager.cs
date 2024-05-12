using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameStates { countDown, running, raceOver};
public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;


    GameStates gameState = GameStates.countDown;

    float raceStartedTime = 0;
    float raceCompletedTime = 0;

    public event Action<GameManager> OnGameStateChanged;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    void LevelStart()
    {
        gameState = GameStates.countDown;
        Debug.Log("Level started");
    }
    public GameStates GetGameState()
    {
        return gameState;
    }
    void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;
            OnGameStateChanged?.Invoke(this);
        }
    }
    public float GetRaceTime()
    {
        if (gameState == GameStates.countDown)
            return 0;
        else if (gameState == GameStates.raceOver)
            return raceCompletedTime - raceStartedTime;
        else return Time.time - raceStartedTime;
    }
    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");

         raceStartedTime = Time.time;
        ChangeGameState(GameStates.running);
    }
    public void OnRaceCompleted()
    {
        Debug.Log("OnRaceCompleted");
        raceCompletedTime = Time.time;
        ChangeGameState(GameStates.raceOver);
    }
  
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }
}
