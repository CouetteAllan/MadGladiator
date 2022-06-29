using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inputs Pattern", menuName = "Inputs Pattern")]
public class ScriptableInputsPattern : ScriptableObject {
    public string inputsString;
    public List<KeyCode> inputs;

    private void OnValidate() {
        if(inputsString.Length > 8) {
            inputsString = inputsString.Remove(8);
        }
        if(inputs.Count > 8) {
            inputs.RemoveRange(8, inputs.Count - 8);
        }
    }
}