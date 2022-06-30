using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public int Lives { get => GameManager.Instance.GetLives(); } //Les vies dépendent des vies stockées dans le game manager où tout le monde peut y avoir accès sans trop de problème

    private bool Dead //Dead = true si plus de vies
    {
        get => Lives <= 0;
    }
    
    void Start()
    {
        Debug.Log(Lives);
    }
    
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.Damaged(1);
            Debug.Log(Lives);
            Debug.Log("Dead is" + Dead);
        }*/

        if (Dead) //Si on a plus de vie, on passe le jeu en mode Game Over
        {
            GameManager.Instance.CurrentGameStates = GameManager.GameStates.GameOver;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBehavior enemy;
        if(collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject.GetComponent<EnemyBehavior>();
            GameManager.Instance.CurrentGameStates = GameManager.GameStates.Defense;
            StartCoroutine(CheckInputsInDefense(enemy));
            StartCoroutine(InitDefenseTimer(enemy));

            GameManager.Instance.EnemyTriggered(enemy);//Envoie l'information de l'ennemi dans la zone au Game Manager.
        }
    }

    IEnumerator CheckInputsInDefense(EnemyBehavior enemy) {
        ScriptableInputsPattern pattern = enemy.ChoosenPattern;
        int index = 0;
        while(index != pattern.inputs.Count) {
            if (Input.GetKey(pattern.inputs[index])) {
                index++;
            }
            yield return new WaitForEndOfFrame();
        }
        GameManager.Instance.EndDefense(true, enemy);
    }

    IEnumerator InitDefenseTimer(EnemyBehavior enemy) {
        yield return new WaitForSecondsRealtime(enemy.timeDuringDefense);
        StopCoroutine(CheckInputsInDefense(enemy));
        GameManager.Instance.EndDefense(false, enemy);
    }
}
