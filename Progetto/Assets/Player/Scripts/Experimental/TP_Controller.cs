using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(TP_Motor))]
[RequireComponent(typeof(TP_Animator))]
public class TP_Controller : MonoBehaviour {

    #region PUBLIC_VARIABLES

    public static CharacterController characterController;			// Reference to the CharacterController componenet.
    public float runSpeed = 10.0f;
    public float walkSpeed = 5.0f;
    #endregion

    #region PRIVATE_VARIABLES

    const string TAG = "TP_Controller";                             // TAG for debugging purposes.

    #endregion

    #region UNITY_FUNCTIONS

    // Use this for initialization
    void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (Camera.main == null) {
            //Debug.LogError(TAG + " No main camera in scene!");
            return;
        }
        // Read input.
        GetLocomotionInput();
        // Handle action input.
        HandleActionInput();
        // Move the character.
        TP_Motor.instance.UpdateMotor();
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    // Function to read the character input and update movement in TP_Motor class.
    void GetLocomotionInput() {
        TP_Motor.instance.VerticalVelocity = TP_Motor.instance.MoveVector.y;

        // First initialise MoveVector to (0,0,0).
        TP_Motor.instance.MoveVector = Vector3.zero;
        // Read Horizontal Axis.
        float h = Input.GetAxis("Horizontal");
        // Read Vertical Axis.
        float v = Input.GetAxis("Vertical");

        // Add read values to MoveVector.
        if (v > 0.1 || v < -0.1)
            TP_Motor.instance.MoveVector += new Vector3(0, 0, v);

        if (h > 0.1 || h < -0.1)
            TP_Motor.instance.MoveVector += new Vector3(h, 0, 0);

        TP_Animator.instance.DetermineCurrentMoveDirection();

        // If the player is pressing left shit ...
        if (Input.GetKey(KeyCode.LeftShift)) {
            // ... then the character is running.
            TP_Motor.instance.forwardSpeed = runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            // ... else, it is walking.
            TP_Motor.instance.forwardSpeed = walkSpeed;
        }

    }

    void HandleActionInput() {
        // If the player has pressed the jump button then jump!
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
        /*
		if(Input.GetKey(KeyCode.E)) {
			Use ();
		}*/

        if (Input.GetMouseButton(0)) {
            Debug.Log("SX");
            Attack();
        }
        else if (Input.GetMouseButton(1)) {
            Debug.Log("DX");
            Defense();
        }
        else if (Input.GetMouseButton(2)) { //un solo power coinvolge l'animazione
            Debug.Log("Centrale");
          
        }

    }

    void Jump() {
        // Call the motor to jump!
        TP_Motor.instance.Jump();
    }

    void RotoAttack() {
        TP_Animator.instance.RotatingAttack();
    }

    void Defense() {
        TP_Animator.instance.Defend();
    }

    void Attack() {
        TP_Animator.instance.Attack();
    }

    #endregion
}
