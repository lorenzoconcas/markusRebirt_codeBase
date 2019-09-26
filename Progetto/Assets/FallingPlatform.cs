using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Markus"))
        {
            anim.Play("shake");
            Invoke ("Drop", 1f);
        }
    }

    void Drop()
    {
       // anim.Play("New State");
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
