using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] Animator animator;
    public int Lives { get => GameManager.Instance.GetLives(); } //Les vies dépendent des vies stockées dans le game manager où tout le monde peut y avoir accès sans trop de problème

    public CamShake camShakeScript;
    public float shakeAmount = 0.1f;
    private bool Dead //Dead = true si plus de vies
    {
        get => Lives <= 0;
    }

    private AudioSource oofSound;

    private void Awake()
    {
        camShakeScript = Camera.main.GetComponent<CamShake>();
        oofSound = this.GetComponent<AudioSource>();
    }

    void Start()
    {
        Debug.Log(Lives);
        GameManager.Instance.SetPlayer(this);
    }
    public void InitAnim() {
        Debug.Log(Lives);
        animator.SetInteger("Lives", Lives);
    }
    
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.Damaged(1);
            Debug.Log(Lives);
            Debug.Log("Dead is" + Dead);
        }*/

        if (Dead && !animDone) //Si on a plus de vie, on passe le jeu en mode Game Over
        {
            animDone = true;
            StartCoroutine(Deadge());
        }
    }
    public bool animDone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBehavior enemy;
        if(collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject.GetComponent<EnemyBehavior>();
            animator.SetFloat("DirectionX", enemy.transform.position.x - this.transform.position.x);
            animator.SetFloat("DirectionY", enemy.transform.position.y - this.transform.position.y);


            GameManager.Instance.CurrentGameStates = GameManager.GameStates.Defense;
            StartCoroutine(CheckInputsInDefense(enemy));
            StartCoroutine(InitDefenseTimer(enemy));

            GameManager.Instance.EnemyTriggered(enemy);//Envoie l'information de l'ennemi dans la zone au Game Manager.
        }
    }

    IEnumerator CheckInputsInDefense(EnemyBehavior enemy) {
        ScriptableInputsPattern pattern = enemy.ChoosenPattern;
        int index = 0;
        while (index != pattern.inputs.Count) {
            if (Input.GetKey(pattern.inputs[index])) {
                index++;
                yield return new WaitForSecondsRealtime(GameManager.Instance.updateTime);
            } else if (Input.anyKey) {
                yield return new WaitForSecondsRealtime(GameManager.Instance.delayTimeAfterFailed);
            } else {
                yield return new WaitForSecondsRealtime(GameManager.Instance.updateTime);
            }
        }
        GameManager.Instance.EndDefense(true, enemy);
        StopAllCoroutines();
        animator.SetTrigger("Attack");
        enemy.Kill(true);
    }

    IEnumerator InitDefenseTimer(EnemyBehavior enemy) {
        yield return new WaitForSecondsRealtime(enemy.timeDuringDefense);
        StopAllCoroutines();
        oofSound.Play();
        enemy.Kill(false);
        GameManager.Instance.EndDefense(false, enemy);
    }

    IEnumerator Deadge() {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorClipInfo(0).Length + 1f);
        GameManager.Instance.CurrentGameStates = GameManager.GameStates.GameOver;
    }
}
