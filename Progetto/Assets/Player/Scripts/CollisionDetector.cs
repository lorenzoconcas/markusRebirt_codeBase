using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public GameObject ScriptHolder;
    public AudioClip audio;
    public AudioSource aSource;
    private Data dS;
  
    public Material checkPointMaterial;
    private void Start() {

        
        dS = ScriptHolder.GetComponent<Data>();
        if (dS == null)
            Debug.LogError("Script not found");
       
    }
    public void OnTriggerEnter(Collider collider) {
        
        if (collider.gameObject.tag.Contains("collectible")) {
            Destroy(collider.gameObject);
            aSource.clip = audio;
            aSource.Play();
        }
        if (collider.gameObject.tag.Contains("checkpoint")) {
            //cambio la texture della tela
            collider.gameObject.transform.Find("Plane").gameObject.GetComponent<Renderer>().material = checkPointMaterial;
           
            dS.SetLastSpawn(transform.position);
            dS.SaveGame();
          
        }
    }
}
