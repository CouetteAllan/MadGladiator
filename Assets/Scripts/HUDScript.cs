using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HUDScript : MonoBehaviour
{
    private GameObject inputPanel;
    private GameObject blackPanel;


    void Awake()
    {
        inputPanel = GameObject.Find("HUD/InputPanel");
        blackPanel = GameObject.Find("HUD/PanelAssombrissement");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(inputPanel);
        Debug.Log(blackPanel);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
