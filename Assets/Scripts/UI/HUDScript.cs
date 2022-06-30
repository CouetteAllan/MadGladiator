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
    private TextMeshPro scoreText;
    [SerializeField] Transform[] inputsKeyRenderers;
    private TextMeshProUGUI encounterText;
    private TimerBar timerBar;


    void Awake()
    {
        inputPanel = GameObject.Find("HUD/InputPanel");
        blackPanel = GameObject.Find("HUD/PanelAssombrissement");
        encounterText = GameObject.Find("HUD/InputPanel/EncounterText").GetComponent<TextMeshProUGUI>();
        timerBar = GameObject.Find("HUD/InputPanel/TimerBar/Timer").GetComponent<TimerBar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EndDisplay();

    }

    public void DisplayInputs(EnemyBehavior enemy)
    {
        inputPanel.SetActive(true);
        blackPanel.SetActive(true);
        DisplayInputPatternUI(enemy);
        encounterText.text = enemy.name + " vous attaque ! D�fendez vous !";
        timerBar.SetTimer(enemy.timeDuringDefense);
    }

    public void EndDisplay()
    {
        inputPanel.SetActive(false);
        blackPanel.SetActive(false);
        StopAllCoroutines();
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
            }
            yield return new WaitForEndOfFrame();
        }
    }
}

