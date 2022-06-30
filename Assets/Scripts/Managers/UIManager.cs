using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    #region Instances;

    private static UIManager instance;
    public static UIManager Instance {
        get {
            if (instance == null)
                Debug.LogError("UIManager Instance not found.");

            return instance;
        }

    }


    #endregion

    private void OnEnable() {
        instance = this;
    }

    private HUDScript hud;
    private GameObject hudCanvas;


    private void Awake() {
        hudCanvas = GameObject.Find("HUD");
        hud = hudCanvas.GetComponent<HUDScript>();
    }

    public void EnemyTriggerUI(EnemyBehavior enemy) {
        hud.DisplayInputs(enemy);
    }

    public void EndDefenseUI() {
        hud.EndDisplay();
    }

    public void UpdateScore() {
        hud.UpdateScoreText();
    }

    public void UpdateUI(int previousState, int newState) {
        hud.UpdateUI(previousState, newState);
    }
}
