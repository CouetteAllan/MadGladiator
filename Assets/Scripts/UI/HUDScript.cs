using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDScript : MonoBehaviour {

    [Header("Groups References")]
    [SerializeField] List<GameObject> groups;
    [Header("In Game")]
    [SerializeField] TextMeshProUGUI scoreText;
    [Header("Defense Phase")]
    [SerializeField] TextMeshProUGUI encounterText;
    [SerializeField] TimerBar timerBar;
    [SerializeField] Transform[] inputsKeyRenderers;
    [Header("GameOver")]
    [SerializeField] TextMeshProUGUI gameOverScoreText;


    private int actualIndex = -1;

    private float timerAnimMax = 1.1f;
    private float timerAnim;

    void Start() {
        EndDisplay();
        UpdateScoreText();
    }

    public void UpdateScoreText() {
        scoreText.text = GameManager.score.ToString();
    }

    public void DisplayInputs(EnemyBehavior enemy) {
        DisplayInputPatternUI(enemy);
        encounterText.text = enemy.name + " vous attaque ! Défendez vous !";
        timerBar.SetTimer(enemy.timeDuringDefense);
        actualIndex = 0;
        timerAnim = timerAnimMax;
    }

    public void UpdateUI(int previousState, int newState) {

        if((GameManager.GameStates)newState == GameManager.Instance.CurrentGameStates) {
            gameOverScoreText.text = scoreText.text;
        }

        groups[previousState].SetActive(false);
        groups[newState].SetActive(true);
    }

    public void EndDisplay() {
        StopAllCoroutines();
        ResetUIPattern();
        actualIndex = -1;
    }

    private void ResetUIPattern() {
        foreach (var item in inputsKeyRenderers) {
            item.GetComponent<TextMeshProUGUI>().SetText("?");
        }
    }

    private void DisplayInputPatternUI(EnemyBehavior enemy) {
        char[] chars = enemy.CharsOfChoosenPattern;
        for (int i = 0; i < inputsKeyRenderers.Length; i++) {

            inputsKeyRenderers[i].gameObject.SetActive(chars.Length > i);
            if (chars.Length <= i) {
                continue;
            }
        }
        StartCoroutine(CheckInputsInDefenseUI(enemy));

    }


    IEnumerator CheckInputsInDefenseUI(EnemyBehavior enemy) {
        char[] chars = enemy.CharsOfChoosenPattern;
        ScriptableInputsPattern pattern = enemy.ChoosenPattern;
        int index = 0;
        while (index != pattern.inputs.Count) {
            if (Input.GetKey(pattern.inputs[index])) {
                inputsKeyRenderers[index].GetComponent<TextMeshProUGUI>().SetText(chars[index].ToString());
                inputsKeyRenderers[index].GetComponent<TextMeshProUGUI>().color = new Color32(86, 95, 243, 250);

                inputsKeyRenderers[index].GetComponent<Animator>().SetTrigger("Bounce");
                index++;
                yield return new WaitForSecondsRealtime(GameManager.Instance.updateTime);
            } else if (Input.anyKey) {
                yield return new WaitForSecondsRealtime(GameManager.Instance.delayTimeAfterFailed);
            } else {
                yield return new WaitForSecondsRealtime(GameManager.Instance.updateTime);
            }
        }
    }

}