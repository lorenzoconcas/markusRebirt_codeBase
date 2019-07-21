using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public float sensibility = 10;
    public float horizontalField = 90;
    public float verticalField = 45;

    public Camera cam; 
    void Start()
    {
        // Cursor.visible = false;    //opzione inserita in altri luoghi
      
    }
    // Update is called once per frame
    void FixedUpdate()
    {       
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        var verticalUnity = 1.0f / verticalField;
        var horizontalUnity = 1.0f / horizontalField;
        var angleHorizontal = (1/sensibility)* x / horizontalUnity;
        var angleVertical = (1 / sensibility) * y / verticalUnity;
        
        transform.Rotate(0, angleHorizontal, 0);

       // Debug.Log(y);


        if (y >= -1.0f && y <= 1.0f) 
         cam.transform.Rotate(-angleVertical, 0, 0);

        
    }   
}


