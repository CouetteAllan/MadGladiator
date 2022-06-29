using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("GameManager null");
            }
            return instance;
        }
    }


    private GameState gameState;
    public enum GameState
    {
        InGame,
        Pause,
        MainMenu,
        GameOver,
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
