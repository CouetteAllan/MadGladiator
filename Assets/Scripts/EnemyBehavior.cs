using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
    #region Fields
    [Header("Movement")]
    public Vector3 targetPosition = new Vector2(0f, 0f);
    [SerializeField] float speed = 0.1f;
    [SerializeField] LayerMask memorizeZoneMask;

    [Header("Stats")]
    public new string name = "EnemyName";
    public int scoreEarned = 100;
    public int damages = 1;
    public float timeDuringDefense = 4f;

    [Header("Inputs")]
    [SerializeField] List<ScriptableInputsPattern> scriptableInputsPatterns;
    [SerializeField] Transform[] inputsKeyRenderers;
    public ScriptableInputsPattern ChoosenPattern { get; private set; }
    public char[] CharsOfChoosenPattern { 
        get {
            return ChoosenPattern.inputsString.ToCharArray();
        }
    }


    [Header("Animation")]
    [SerializeField] Animator animator;
    #endregion
    void Start() {
        InitAnim();
    }

    void Update() {
        this.transform.position = Vector2.Lerp(this.transform.position, targetPosition, speed * Time.deltaTime);
    }

    void InitAnim() {
        Vector3 direction = targetPosition - this.transform.position;
        animator.SetFloat("DirectionX", direction.x);
        animator.SetFloat("DirectionY", direction.y);
    }

    public void InitInputsPattern(int index = -1) {
        if(index <= -1) {
            index = Random.Range(0, scriptableInputsPatterns.Count);
        }
        DisplayInputsPattern(scriptableInputsPatterns[index]);
        ChoosenPattern = scriptableInputsPatterns[index];
    }

    void DisplayInputsPattern(ScriptableInputsPattern pattern) {
        char[] chars = GetChars(pattern);
        for (int i = 0; i < inputsKeyRenderers.Length; i++) {

            inputsKeyRenderers[i].gameObject.SetActive(chars.Length > i);
            if (chars.Length <= i) {
                continue;
            }

            inputsKeyRenderers[i].GetChild(0).GetComponent<TextMeshProUGUI>().SetText(chars[i].ToString());
        }
    }

    char[] GetChars(ScriptableInputsPattern pattern) {
        return pattern.inputsString.ToCharArray();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(memorizeZoneMask == (memorizeZoneMask | (1 << collision.gameObject.layer))) {
            StartCoroutine(HideInputsPattern());
        }
    }

    IEnumerator HideInputsPattern() {
        foreach (var item in inputsKeyRenderers) {
            item.gameObject.SetActive(false);
        }
        yield return null;
    }

    public void Kill(bool defenseSuceeded) {
        if (defenseSuceeded) {
            animator.SetTrigger("Death");
        } else {
            animator.SetTrigger("Attack");
        }
        Destroy(this.gameObject, 0.5f);
    }
}
