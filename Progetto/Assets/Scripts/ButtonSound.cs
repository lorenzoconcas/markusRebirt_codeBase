using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
{
    public AudioClip audio;
    public AudioSource aSource;
    public void MouseClick(bool check = false)
    {
        if (check) {
            if (!CheckSaved.IsGameSaveAvaible()) {
                Debug.Log("again..");
            }
            else {
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




