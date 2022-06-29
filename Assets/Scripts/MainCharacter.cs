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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Lives);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.Damaged(1);
            Debug.Log(Lives);
            Debug.Log("Dead is" + Dead);
        }
    }
}
