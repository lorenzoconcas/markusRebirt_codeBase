using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformAttach : MonoBehaviour {

  
    private Vector3 newPosition;

     private void OnTriggerEnter(Collider other) {       

         if (other.name.Equals("Markus")) {          
            other.transform.SetParent(transform, true);
         }
     }


     private void OnTriggerExit(Collider other) {
         if (other.name.Equals("Markus")) {
            newPosition = other.transform.position;
            other.transform.SetParent(null, true);           
            other.transform.position = newPosition;
         }
     }


}
