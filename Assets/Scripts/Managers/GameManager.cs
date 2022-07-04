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

    public Camera cam;
    private MainCharacter player;

    private void OnEnable() {
        instance = this;
    }
    private void Start() {
        CurrentGameStates = GameStates.MainMenu;
        cam = Camera.main;
        
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
            GameStates oldState = currentGameState;
            currentGameState = value;
            UIManager.Instance.UpdateUI((int)oldState, (int)currentGameState);
            switch (currentGameState) {
                case GameStates.MainMenu:
                    foreach (var item in GameObject.FindGameObjectsWithTag("Enemy")) {
                        Destroy(item);
                    }
                    StopAllCoroutines();
                    Time.timeScale = 1f;
                    Init();
                    break;

                case GameStates.InGame:
                    Time.timeScale = 1.0f;
                    if (oldState == GameStates.Pause) return;
                    if (oldState == GameStates.Defense) return;
                    StartRoundTimeline(0);
                    break;

                case GameStates.Pause:
                    Time.timeScale = 0.0f;
                    break;

                case GameStates.GameOver:
                    Time.timeScale = 0f;
                    break;

                case GameStates.Defense:
                    Time.timeScale = 0.0f;
                    break;
            }
        }
    }
    public static int score;
    public int round;
    [SerializeField] private int lives = 4;

    [SerializeField] float timeStartRound = 5f;
    [SerializeField] float timeEndRound = 5f;

    [Header("SpawnManager")]
    [SerializeField] List<RoundTimeline> roundTimelines;
    [SerializeField] float updateEndRoundtime;

    [Header("Defense Phase")]
    public float updateTime = 0.01f;
    public float delayTimeAfterFailed = 0.3f;
    private void Init() {
        score = 0;
        round = 0;
        lives = 4;
        if (player == null) player = FindObjectOfType<MainCharacter>();
        player.InitAnim();
        player.animDone = false;
    }
    public int AddScore(int scoreToAdd) {
        score += scoreToAdd;
        UIManager.Instance.UpdateScore();
        return score;
    }

    public int GetLives() {
        return lives;
    }

    public void SetPlayer(MainCharacter player)
    {
        this.player = player;
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
            player.GetComponent<Animator>().SetInteger("Lives", GetLives());
            player.GetComponent<CamShake>().Shake(0.1f, 0.25f);
            player.GetComponent<Animator>().SetTrigger("Hurt");
        }

        CurrentGameStates = GameStates.InGame;

    }



    private void Update() {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            StartRoundTimeline(0);
        }
#endif
    }
    public void StartRoundTimeline(int index = -1) {
        round = index <= -1 ? round : index;
        StartCoroutine(StartRoundAnimation());
    }
    IEnumerator StartRoundAnimation() {
        UIManager.Instance.SetStartRound(true);
        yield return new WaitForSeconds(timeStartRound);
        UIManager.Instance.SetStartRound(false);
        StartCoroutine(StartTimeline(roundTimelines[round]));
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
        round++;
        StartCoroutine(EndRoundAnimation());
    }
    IEnumerator EndRoundAnimation() {
        UIManager.Instance.SetEndRound(true);
        yield return new WaitForSeconds(timeEndRound);
        UIManager.Instance.SetEndRound(false);
        if (round >= roundTimelines.Count) {
            CurrentGameStates = GameStates.MainMenu;
            yield break;
        }
        StartRoundTimeline();
    }
    public void ChangeState(int newState) {
        CurrentGameStates = (GameStates)newState;
    }
    public void Quit() {
        Application.Quit();
    }
}
