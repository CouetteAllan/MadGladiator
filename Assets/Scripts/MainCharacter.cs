using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public int Lives { get => GameManager.Instance.GetLives(); } //Les vies d�pendent des vies stock�es dans le game manager o� tout le monde peut y avoir acc�s sans trop de probl�me

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

    
}
