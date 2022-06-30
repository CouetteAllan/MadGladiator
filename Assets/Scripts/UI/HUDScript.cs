using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HUDScript : MonoBehaviour
{
    private GameObject inputPanel;
    private GameObject blackPanel;
    private TextMeshProUGUI scoreText;
    [SerializeField] Transform[] inputsKeyRenderers;
    private TextMeshProUGUI encounterText;
    private TimerBar timerBar;
    private int actualIndex = -1;

    private float timerAnimMax = 1.1f;
    private float timerAnim;


    void Awake()
    {
        inputPanel = GameObject.Find("HUD/InputPanel");
        blackPanel = GameObject.Find("HUD/PanelAssombrissement");
        encounterText = GameObject.Find("HUD/InputPanel/EncounterText").GetComponent<TextMeshProUGUI>();
        timerBar = GameObject.Find("HUD/InputPanel/TimerBar/Timer").GetComponent<TimerBar>();
        scoreText = GameObject.Find("HUD/ScorePanel/ScoreText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        EndDisplay();
        UpdateScoreText();

    }

    public void UpdateScoreText()
    {
        scoreText.text = GameManager.score.ToString();
    }

    public void DisplayInputs(EnemyBehavior enemy)
    {
        inputPanel.SetActive(true);
        blackPanel.SetActive(true);
        DisplayInputPatternUI(enemy);
        encounterText.text = enemy.name + " vous attaque ! Défendez vous !";
        timerBar.SetTimer(enemy.timeDuringDefense);
        actualIndex = 0;
        timerAnim = timerAnimMax;
    }

    public void EndDisplay()
    {
        inputPanel.SetActive(false);
        blackPanel.SetActive(false);
        StopAllCoroutines();
        ResetUIPattern();
        actualIndex = -1;
    }

    private void ResetUIPattern()
    {
        foreach (var item in inputsKeyRenderers)
        {
            item.GetComponent<TextMeshProUGUI>().SetText("?");
        }
    }

    private void DisplayInputPatternUI(EnemyBehavior enemy)
    {
        char[] chars = enemy.CharsOfChoosenPattern;
        for (int i = 0; i < inputsKeyRenderers.Length; i++)
        {

            inputsKeyRenderers[i].gameObject.SetActive(chars.Length > i);
            if (chars.Length <= i)
            {
                continue;
            }
        }
        StartCoroutine(CheckInputsInDefenseUI(enemy));

    }


    IEnumerator CheckInputsInDefenseUI(EnemyBehavior enemy)
    {
        char[] chars = enemy.CharsOfChoosenPattern;
        ScriptableInputsPattern pattern = enemy.ChoosenPattern;
        int index = 0;
        while (index != pattern.inputs.Count)
        {
            if (Input.GetKey(pattern.inputs[index]))
            {
                inputsKeyRenderers[index].GetComponent<TextMeshProUGUI>().SetText(chars[index].ToString());
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

