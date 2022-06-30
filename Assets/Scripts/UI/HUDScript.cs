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

    // Start is called before the first frame update
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
                index++;
                actualIndex++;
            }
            yield return new WaitForEndOfFrame();
        }
    }


    private void Update()
    {
        if(actualIndex > -1)
        {
            if(timerAnim > timerAnimMax * 0.5f)
                inputsKeyRenderers[actualIndex].transform.localScale += Vector3.one * 1.1f * Time.deltaTime;
            else
                inputsKeyRenderers[actualIndex].transform.localScale -= Vector3.one * 1.1f * Time.deltaTime;

            timerAnim -= Time.deltaTime;

        }
    }

    IEnumerator BounceText()
    {
        while(timerAnim > 0)
        {
            if (timerAnim > timerAnimMax * 0.5f)
                inputsKeyRenderers[actualIndex].transform.localScale += Vector3.one * 1.1f * Time.deltaTime;
            else
                inputsKeyRenderers[actualIndex].transform.localScale -= Vector3.one * 1.1f * Time.deltaTime;

            timerAnim -= 0.1f;

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}

