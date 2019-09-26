using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour {
    public new AudioClip audio;
    public AudioSource aSource;
    public void MouseClick(bool check = false) {
        if (check) {
            if (Data.SaveDataAvailable()) {
                aSource.clip = audio;
                aSource.Play();
            }
        }
        else {
            aSource.clip = audio;
            aSource.Play();
        }

    }

}




