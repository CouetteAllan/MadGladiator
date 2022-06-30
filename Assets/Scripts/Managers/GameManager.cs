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
        Defense,
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
                    Time.timeScale = 0.0f;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    break;

                case GameStates.Defense:
                    Time.timeScale = 0.0f;
                    break;


            }


        }
    }

    private int score;
    [SerializeField]private int lives = 4;

    public int GetScore() //Obtenir le score depuis n'importe quel autre script en faisant "GameManager.instance.GetScore()"
    {
        return score;
    }

    public int AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        return score;
    }

    public int GetLives()
    {
        return lives;
    }

    public void Damaged(int damages)
    {
        lives -= damages;
    }

    public void EnemyTriggered(EnemyBehavior enemy)
    {
        UIManager.Instance.EnemyTriggerUI(enemy);
    }

    public void EndDefense(bool won,EnemyBehavior enemy) //sur une fin de défense, on regarde si le joueur a gagné sa confrontation en lui donnant du score en fonction de l'ennemi
    {
        UIManager.Instance.EndDefenseUI();
        if (won)
        {
            AddScore(enemy.scoreEarned);
        }
        else
        {
            Damaged(enemy.damages);
        }

        CurrentGameStates = GameStates.InGame;

    }

    public int IndexRightInput(int index)
    {
        return index;
    }
}
