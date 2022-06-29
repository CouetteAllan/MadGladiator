using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
    #region Fields
    [Header("Movement")]
    public Vector3 targetPosition = new Vector2(0f, 0f);
    [SerializeField] float speed = 0.1f;


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
        InitInputsPattern();
    }

    void Update() {
        this.transform.position = Vector2.Lerp(this.transform.position, targetPosition, speed * Time.deltaTime);
    }

    void InitAnim() {
        Vector3 direction = targetPosition - this.transform.position;
        animator.SetFloat("DirectionX", direction.x);
        animator.SetFloat("DirectionY", direction.y);
    }

    void InitInputsPattern(int index = -1) {
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
}
