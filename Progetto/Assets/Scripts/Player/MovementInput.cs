
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour {

	private float InputX;
    private float InputZ;
    private Vector3 desiredMoveDirection;
	
	
    private float Speed;

    [Header("Valori Modificabili")]
    public Animator anim;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed = 0.1f;
    public float WalkSpeed = 1.0f;
    public float RunSpeed = 10.0f;
    public float VerticalSensivity = 0.4f;
    public float HorizontalSensitivty = 0.4f;
    public float jumpForce = 5.0f;
    public float gravity = 9.8f;

    public float allowPlayerRotation = 0.1f;
	public Camera cam;
	public CharacterController controller;
	public bool isGrounded;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;


    private float verticalVel;
    private Vector3 moveVector;
    private bool jumping;

    // Use this for initialization
    void Start () {
		anim = this.GetComponent<Animator> ();
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.y < 360)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit(0);
#endif
        }
        InputMagnitude ();
        
		//If you don't need the character grounded then get rid of this part.
		/*isGrounded = controller.isGrounded;
        if (isGrounded)
            anim.SetTrigger("Fall");
		/*
		moveVector = new Vector3 (0, verticalVel, 0);
		controller.Move (moveVector);*/
        
		//Updater
	}

	void PlayerMoveAndRotation() {
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");
        if(check(InputX, HorizontalSensitivty) || check(InputZ, VerticalSensivity))
        {

           // Debug.Log(InputX + " " + InputZ);
            var camera = Camera.main;
            var forward = cam.transform.forward;
            var right = cam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            desiredMoveDirection = forward * InputZ + right * InputX;
           

            if (blockRotationPlayer == false) {
                if (Input.GetButton("Jump") && !jumping)
                {
                    jumping = false;
                    desiredMoveDirection.y += jumpForce;
                    anim.SetTrigger("Jump");
                }else
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("RightTrigger") > 0.4)
                   {
                        // toggleMode("Run");
                    
                        anim.SetTrigger("Run");
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);

                        controller.Move(desiredMoveDirection * Time.deltaTime * RunSpeed*10);
                    }
                    else
                    {
                        // toggleMode("Walk");
                        anim.SetBool("Walk", true);
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);                   
                        controller.Move(desiredMoveDirection * Time.deltaTime * WalkSpeed*10);
                    }
                   desiredMoveDirection.y -= gravity * Time.deltaTime;
              
            }
        }
        else
            anim.SetTrigger("Idle");
        //toggleMode("Idle");


    }

    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

	void InputMagnitude() {
		//Calculate Input Vectors
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

		//anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
		//anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

		//Calculate the Input Magnitude
		Speed = new Vector2(InputX, InputZ).sqrMagnitude;

		//Physically move player
		if (Speed > allowPlayerRotation) {
			//anim.SetFloat ("InputMagnitude", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} else if (Speed < allowPlayerRotation) {
			//anim.SetFloat ("InputMagnitude", Speed, StopAnimTime, Time.deltaTime);
		}
	}

    private void toggleMode(string onlyTrue)
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);

        anim.SetBool(onlyTrue, true);
    }


    private bool check(float value, float sensivity)
    {
        return value < -sensivity || value > sensivity;
    }
}
