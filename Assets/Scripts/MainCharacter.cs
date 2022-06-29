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
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.Damaged(1);
            Debug.Log(Lives);
            Debug.Log("Dead is" + Dead);
        }

        if (Dead) //Si on a plus de vie, on passe le jeu en mode Game Over
        {
            GameManager.Instance.CurrentGameStates = GameManager.GameStates.GameOver;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBehavior enemy;
        if(collision.gameObject.GetComponent<EnemyBehavior>()!= null)
        {
            enemy = collision.gameObject.GetComponent<EnemyBehavior>();

            GameManager.Instance.EnemyTriggered(enemy);//Envoie l'information de l'ennemi dans la zone au Game Manager.
        }
    }

}
