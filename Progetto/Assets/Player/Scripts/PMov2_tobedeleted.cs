using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMov2 : MonoBehaviour
{
    private enum PlayerStatus { IDLE, WALKING, RUNNING, JUMPING, FALLING,LANDING, ATTACKING, DEFENDING, FALLINGAWAY }
    

    private Camera cam;
    private CharacterController controller;
    private Animator animator;
    private PlayerStatus playerStatus;

    private float jumpingCoolDown = 0.5f;
    private float camCoolDown = 0.8f; //sec prima di rigirare la cam
    private float verticalUnity = 1.0f / 90;
    private float horizontalUnity = 1.0f / 45;

    //variabili pubbliche
    public float attackCoolDown = 0.5f;  
    public float JumpHeight = 100.0f; //1 metro?
    public float runSpeed = 5;
    public float walkSpeed = 1;

    void Start()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
        // animation = gameObject.GetComponent<Animation>();
        animator = GetComponent<Animator>();
    }



    void FixedUpdate() {
        Vector3 movement = new Vector3(0,0,0);
        //raccolgo i dati di movimento
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        //calcolo la velocità
        float speed = 0;

        bool jumpPressed = Input.GetButton("Jump");
        bool runPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("RightTrigger") > 0.4;
        bool walkPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        if (controller.isGrounded) {
            if(playerStatus != PlayerStatus.FALLING) {
                Debug.Log("Tasti premuti (W,R, J) " + walkPressed + runPressed + jumpPressed);
                if (!walkPressed && !runPressed && !jumpPressed)
                    playerStatus = PlayerStatus.IDLE;
                else {
                    
                    if (walkPressed) {
                        speed = walkSpeed;
                        playerStatus = PlayerStatus.WALKING;
                     
                    }


                    if (runPressed) {
                        speed = runSpeed;
                        playerStatus = PlayerStatus.RUNNING;
                       
                    }

                    movement = inputZ * cam.transform.forward + inputX * cam.transform.right;
                    if (jumpPressed) {
                        if (!walkPressed && !runPressed) {

                            movement += new Vector3(0, JumpHeight, 0);
                        }
                        else
                            movement += new Vector3(0, JumpHeight, 0);

                        playerStatus = PlayerStatus.JUMPING;
                    }
                    Debug.Log(movement);
                    controller.Move(movement * speed * Time.deltaTime * 10);
                }   
            }
            else {
                playerStatus = PlayerStatus.LANDING;
            }
        }
        else {         
            switch (playerStatus) {
                case PlayerStatus.JUMPING: {
                        playerStatus = PlayerStatus.FALLING;                        
                        break;
                    }
                case PlayerStatus.FALLING: {
                        
                        movement = inputZ * cam.transform.forward + inputX * cam.transform.right;
                        movement.y -= 9.81f * Time.fixedDeltaTime;
                        transform.position += movement;
                       // controller.Move(movement * speed * Time.deltaTime * 10);
                        break;
                    }
                case PlayerStatus.FALLINGAWAY: {
                        ExitGame exeo = new ExitGame();
                        if (transform.position.y < 350)
                            exeo.Exit(); //codice valido per ora, richiede più testing, da modificare quando ci sarà il salvataggio
                        break;
                    }
                default: {
                        playerStatus = PlayerStatus.FALLINGAWAY;
                        break;
                    }
            }
        }
        PlayAnimation();
    }

    void PlayAnimation() {
        Debug.Log(playerStatus.ToString());
        switch (playerStatus) {
            case PlayerStatus.JUMPING: {
                    animator.Play("JumpStart");
                    break;
                }
            case PlayerStatus.FALLING: {
                    animator.Play("Fall");
                    break;
                }
            case PlayerStatus.RUNNING: {
                    animator.Play("Run");
                    break;
                }
            case PlayerStatus.WALKING: {
                    animator.Play("Walk");
                    break;
                }
            case PlayerStatus.IDLE: {
                    animator.Play("Idle");
                    break;
                }
            case PlayerStatus.ATTACKING: {
                    animator.Play("JumpStart");
                    break;
                }
            case PlayerStatus.LANDING: {
                    animator.Play("JumpEnd");
                    break;
                }
            case PlayerStatus.FALLINGAWAY: {
                    animator.Play("Fall");
                    break;
                }
        }
    }

}      