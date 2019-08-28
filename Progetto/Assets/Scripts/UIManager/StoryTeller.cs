using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTeller : MonoBehaviour
{
  
    public GameObject overlay;
    public Text messageObject;
    public string message;
    public Text continueButton;
    public string continueButtonText;
   
    private void OnTriggerEnter(Collider other) {
        if (other.name.Equals("Markus")) {
           
            continueButton.text = continueButtonText;
            messageObject.text = message;
            Cursor.visible = true;
            other.gameObject.SetActive(false);
            overlay.SetActive(!overlay.activeSelf);
            if(!name.Contains("Markus")) //il player non può essere eliminato
                 Destroy(this.gameObject);
        }
    }

    public void ShowMessage(string message, string cbText) {
        continueButton.text = cbText;
        messageObject.text = message;
        Cursor.visible = true;
       
        overlay.SetActive(!overlay.activeSelf);
       // Destroy(this);
    }
    public void HideMessage() {
        overlay.SetActive(!overlay.activeSelf);
    }
}
