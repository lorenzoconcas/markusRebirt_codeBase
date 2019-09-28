using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Markus"))
        {
            anim.Play("shake");
            Invoke("Drop", 1f);
        }
    }

    void Drop()
    {
        anim.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
