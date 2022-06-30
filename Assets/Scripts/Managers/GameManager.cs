using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Instances;

    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null)
                Debug.LogError("GameManager Instance not found.");

            return instance;
        }
    }


    #endregion

    private void OnEnable() {
        instance = this;
    }
    private void Start() {
        CurrentGameStates = GameStates.MainMenu;
    }

    private GameStates gameState;
    public enum GameStates {
        MainMenu = 0,
        InGame = 1,
        Pause = 2,
        GameOver = 3,
        Defense = 4,
    }

    private GameStates currentGameState;
    public GameStates CurrentGameStates {
        get => currentGameState;
        set {
            UIManager.Instance.UpdateUI((int)currentGameState, (int)value);
            currentGameState = value;
            switch (currentGameState) {
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
    public static int score;
    [SerializeField] private int lives = 4;

    [Header("SpawnManager")]
    [SerializeField] List<RoundTimeline> roundTimelines;
    [SerializeField] float updateEndRoundtime;

    [Header("Defense Phase")]
    public float updateTime = 0.01f;
    public float delayTimeAfterFailed = 0.3f;

    public int AddScore(int scoreToAdd) {
        score += scoreToAdd;
        UIManager.Instance.UpdateScore();
        return score;
    }

    public int GetLives() {
        return lives;
    }

    public void Damaged(int damages) {
        lives -= damages;
    }

    public void EnemyTriggered(EnemyBehavior enemy) {
        UIManager.Instance.EnemyTriggerUI(enemy);
    }

    public void EndDefense(bool won, EnemyBehavior enemy) //sur une fin de défense, on regarde si le joueur a gagné sa confrontation en lui donnant du score en fonction de l'ennemi
    {
        UIManager.Instance.EndDefenseUI();
        if (won) {
            AddScore(enemy.scoreEarned);
        } else {
            Damaged(enemy.damages);
        }

        CurrentGameStates = GameStates.InGame;

    }

    private void Update() {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            StartTimeline(0);
        }
#endif
    }
    public void StartTimeline(int index) {
        StartCoroutine(StartTimeline(roundTimelines[index]));
    }

    IEnumerator StartTimeline(RoundTimeline roundTimeline) {
        for (int i = 0; i < roundTimeline.events.Count; i++) {
            RoundTimeline.Event currentEvent = roundTimeline.events[i];
            yield return new WaitForSeconds(currentEvent.spawnTime);
            SpawnManager.Instance.Spawn(
                currentEvent.position,
                currentEvent.enemy,
                currentEvent.patternIndex
            );
        }
        while (!SpawnManager.Instance.RoundDone) {
            yield return new WaitForSeconds(updateEndRoundtime);
        }
        Debug.Log("Fin round");
    }

    public void ChangeState(int newState) {
        CurrentGameStates = (GameStates)newState;
    }
    public void Quit() {
        Application.Quit();
    }
}
