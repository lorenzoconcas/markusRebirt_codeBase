using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformAttach : MonoBehaviour {
    /*Questo metodo fa in modo che il giocatore segua il percorso della piattaforma*/
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.transform.parent = transform;
            Vector3 attachPosition = transform.position;
            other.transform.position = attachPosition;
        }
    }

    /*Eseguo l'opposto del precedente metodo*/
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            other.transform.parent = null;
        }
    }
}
