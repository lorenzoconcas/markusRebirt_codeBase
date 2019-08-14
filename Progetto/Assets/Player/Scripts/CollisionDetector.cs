using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{

    public AudioClip audio;
    private AudioSource aSource;
    private void Start() {
        aSource = GetComponent<AudioSource>();
       
    }
    public void OnTriggerEnter(Collider collider) {        
        if (collider.gameObject.tag.Contains("collectible")) {
            Destroy(collider.gameObject);
            aSource.clip = audio;
            aSource.Play();
        }
    }
}
