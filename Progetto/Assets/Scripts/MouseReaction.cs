using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MouseReaction : MonoBehaviour
{
   
    public float LBound = -46.0f;
    public float RBound = 25.5f;
    public float TBound = -12.0f;
    public float BBound = 12.0f;
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update() {
        Vector3 relativePos = Input.mousePosition - transform.position;

        if ((relativePos.x >= LBound && relativePos.x <= RBound) && (relativePos.y >= TBound && relativePos.y <= BBound)) {
            if (this.name.Equals("CaricaPartita")) {
                if (Data.SaveDataAvailable())
                    text.fontStyle = FontStyle.Bold;
            }
            else
                text.fontStyle = FontStyle.Bold;
      
            if (name.Equals("Esci")) 
                text.color = Color.Lerp(Color.white, Color.HSVToRGB(1.0f,1.0f, 0.8f), 5);
         }        
        else {
            text.fontStyle = FontStyle.Normal;
            if (name.Equals("Esci")) 
                text.color = Color.Lerp(Color.HSVToRGB(1.0f, 1.0f, 0.8f), Color.white, 5);
            
        }
    }
}
