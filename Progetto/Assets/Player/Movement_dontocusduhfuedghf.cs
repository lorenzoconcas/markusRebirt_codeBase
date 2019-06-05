using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;
    public float jumpSpeed = 10;
    public float gravity = 9.81f;

    public float rotationAngle = 15.0f;
    public float rotationCoolDown = 0.5f;
    private float rotationTime;


    public float VerticalSensivity = 0.4f;
    public float HorizontalSensitivty = 0.4f;
    private Vector3 movement;

    private Animator anim;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mov = new Vector3();
        float mV = Input.GetAxis("Vertical");      
        float mH = Input.GetAxis("Horizontal");


        if (check(mH, HorizontalSensitivty) || check(mV, VerticalSensivity))
        {
            
             if (Input.GetKey(KeyCode.LeftShift))
             {
                 mov = new Vector3(mH, 0.0f, mV) * runSpeed * 10;
                 toggleMode("Run");
             }
             else
             {
                 mov = new Vector3(mH, 0.0f, mV) * walkSpeed * 10;
                 toggleMode("Walk");
             }
        }
        else
        {
            toggleMode("Idle");
            Debug.Log("MV : " + mV + ", MH : " + mH);
        }

       

        //rotazione player
        if (Time.time > rotationTime)
        {
            rotationTime = Time.time + rotationCoolDown;

            if (Input.GetKey(KeyCode.A))
                transform.Rotate(0, -rotationAngle, 0);
            else if (Input.GetKey(KeyCode.D))
                transform.Rotate(0, rotationAngle, 0);
            else if (Input.GetKey(KeyCode.S))
                transform.Rotate(0, 180.0f, 0);
        }

        controller.Move(mov * Time.deltaTime);

    }

    private bool check(float value, float sensivity)
    {
        return value < -sensivity || value > sensivity;
    }
   private void toggleMode(string onlyTrue)
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);

        anim.SetBool(onlyTrue, true);
   }

 
    
}
