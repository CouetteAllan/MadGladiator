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
        encounterText.text = enemy.name + " vous attaque ! Défendez vous !";
        timerBar.SetTimer(enemy.timeDuringDefense);
    }

    public void EndDisplay()
    {
        inputPanel.SetActive(false);
        blackPanel.SetActive(false);
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
            inputsKeyRenderers[i].GetComponent<TextMeshProUGUI>().SetText(chars[i].ToString());
        }
    }

}

