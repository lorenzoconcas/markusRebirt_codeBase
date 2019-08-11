using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckSaved : MonoBehaviour
{
    //public static CheckSaved instance;

    public Text ContinueButton;

    public static bool IsGameSaveAvaible() { return false; } //only for test

    void Start()
    {
       
        if (IsGameSaveAvaible()) {
            ContinueButton.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
