using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Instances;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("UIManager Instance not found.");

            return instance;
        }

    }


    #endregion

    private void OnEnable()
    {
        instance = this;
    }
}
