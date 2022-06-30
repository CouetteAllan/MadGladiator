using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public Gradient gradient;
    public Slider slider;
    public Image fill;
    private float timer;
    private float startTimer;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            UpdateTimerUI();
        }
    }

    public void SetTimer(float timerEnemy)
    {
        this.startTimer = timerEnemy;
        timer = startTimer;
        //StartCoroutine(InitDefenseTimer());
    }

    private void UpdateTimerUI()
    {
        slider.value = timer / startTimer;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    
}
