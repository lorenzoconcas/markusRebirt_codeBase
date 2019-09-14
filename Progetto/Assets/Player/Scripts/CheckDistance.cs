using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDistance : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);

        Physics.Raycast(downRay, out hit);
        Debug.Log("RayCast " + hit.distance);

    }
}
