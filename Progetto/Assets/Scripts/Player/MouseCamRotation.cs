using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamRotation : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 rot = new Vector3(Input.GetAxis("Mouse Y"), 0, Input.GetAxis("Mouse X"));
        
        if(Input.GetKey(KeyCode.LeftShift))
           transform.Rotate(rot, 3.0f);
        else
            transform.Rotate(rot, 1.75f);


        
    }

}
