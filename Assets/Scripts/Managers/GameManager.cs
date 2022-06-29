using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Instances;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("GameManager Instance not found.");

            return instance;
        }

    }


    #endregion

    private void OnEnable()
    {
        instance = this;
    }


    private GameStates gameState;
    public enum GameStates
    {
        InGame,
        Pause,
        MainMenu,
        GameOver,
    }

    private GameStates currentGameState;
    public GameStates CurrentGameStates
    {
        get => currentGameState;
        set
        {
            currentGameState = value;
            switch (currentGameState)
            {
                case GameStates.MainMenu:
                    break;

                case GameStates.InGame:
                    Time.timeScale = 1.0f;
                    break;

                case GameStates.Pause:
                    Time.timeScale = 0.0f;
                    break;

                case GameStates.GameOver:
                    Time.timeScale = 0.4f;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    break;


            }


        }
    }

    private int score;


    public int GetScore()
    {
        return score;
    }

    public int AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        return score;
    }


}
