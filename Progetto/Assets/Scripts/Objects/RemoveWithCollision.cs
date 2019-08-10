using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWithCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Markus") //in relazione al player quindi
        {
            this.gameObject.SetActive(false); //"disattiva" l'oggetto
        }
    }
}
