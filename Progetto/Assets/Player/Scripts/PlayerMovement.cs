using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;
    private CharacterController controller;
    private Animator animator;
    private Animation animation;
    private bool jumping;
    private float speed;

    public float runSpeed = 5;
    public float walkSpeed = 1;
    public float jumpForce;
    public float gravity;

    public float coolDown = 0.4f; //sec prima di rigirare la cam

    private float verticalUnity = 1.0f / 90;
    private float horizontalUnity = 1.0f / 45;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
        // animation = gameObject.GetComponent<Animation>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        
        float speed = new Vector2(inputX, inputZ).sqrMagnitude;
        if (speed > 0.1f)
        {


            if (Time.time > Time.deltaTime + coolDown)
            {
                if (inputZ < 0)
                    transform.Rotate(0, -180, 0);
                else
                {
                    var rotationAngle = inputX / horizontalUnity;
                    transform.Rotate(0, rotationAngle, 0);
                }
            }
                       
            

            Vector3 movement = inputZ * cam.transform.forward + inputX * cam.transform.right;

                if (controller.isGrounded)
                {
                    //forse va modificato con un trigger
                    if (jumping)
                    {
                        animator.Play("JumpEnd");
                        jumping = false;
                    }

                }
                else
                {
                    if (inputX > 0.1 || inputZ > 0.1)
                        animator.Play("Fall");

                }
                movement.y -= gravity * Time.deltaTime;
                if (Input.GetButton("Jump") && !jumping)
                {
                    animator.Play("JumpStart");
                    jumping = true;
                    movement.y += jumpForce;
                }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("RightTrigger") > 0.4)
                {
                    speed = runSpeed;
                    animator.Play("Run");
                
                }
                else
                {
                    speed = walkSpeed;
                    animator.Play("Walk");
                }
                controller.Move(movement * speed * Time.deltaTime * 10);
            
        }
        else
            animator.Play("Idle");
    }
}
