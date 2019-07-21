using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
{
    public AudioClip audio;
    public AudioSource aSource;
    public void MouseClick()
    {
        aSource.clip = audio;
        aSource.Play();
    }
   

   

}




