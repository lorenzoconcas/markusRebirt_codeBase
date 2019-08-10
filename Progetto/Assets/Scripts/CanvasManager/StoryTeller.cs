using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTeller : MonoBehaviour
{
    public GameObject overlay;
    public Text message;
    void Start() {
        
        message.text = "Ciao";
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            overlay.SetActive(!overlay.activeSelf);
          //  message.text = "èjrlhgfowrhaoghoh";
        }
    }
}
