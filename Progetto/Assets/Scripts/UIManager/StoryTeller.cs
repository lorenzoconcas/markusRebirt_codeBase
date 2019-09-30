using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTeller : MonoBehaviour {

    public GameObject overlay;
    public Text messageObject;
    public string message;
    public Text continueButton;
    public string continueButtonText;

    private void OnTriggerEnter(Collider other) {
      
        if (other.gameObject.CompareTag("Player")) {
            continueButton.text = continueButtonText;
            messageObject.text = message;
            Cursor.visible = false;
            other.gameObject.SetActive(false);
            overlay.SetActive(true);
         
        //    if (!name.Contains("Markus")) //il player non può essere eliminato
                Destroy(this.gameObject);
        }
    }

    public void ShowMessage(string message, string cbText) {
        continueButton.text = cbText;
        messageObject.text = message;
        Cursor.visible = true;
        GameObject.Find("Scripts").GetComponent<OverlayShower>().SetVolume(true);
        overlay.SetActive(true);
        // Destroy(this);
    }
    public void HideMessage() {
        GameObject.Find("Scripts").GetComponent<OverlayShower>().SetVolume(false);
        overlay.SetActive(true);
    }
}
