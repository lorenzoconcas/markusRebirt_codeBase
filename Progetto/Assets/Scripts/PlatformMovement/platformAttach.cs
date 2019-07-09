using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformAttach : MonoBehaviour
{
    public GameObject Player;

    /*Questo metodo fa in modo che il giocatore segua il percorso della piattaforma*/
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player)
        {
            Player.transform.parent = transform;
        }
    }

    /*Eseguo l'opposto del precedente metodo*/
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }
    }
}
