using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
[RequireComponent (typeof (TP_Controller))]
[RequireComponent (typeof (TP_Animator))]
public class TP_Motor : MonoBehaviour {

	#region PUBLIC_VARIABLES

	public static TP_Motor instance;						// Reference to the instance of this object.
															//
	public float gravity = 20f;								// The gravity applied to the player.
	public float terminalVelocity = 10f;					// The maximum speed the cahracter is allowed to fall in.
	public float forwardSpeed = 2f;							// The forward movement speed.
	public float backwardSpeed = 1f;						// The backward movement speed.
	public float strafingSpeed = 1.5f;						// The left and right movement speed.
	public float slidingSpeed = 2f;							// The sliding speed.
	[Range(0f,1f)] public float smoothRotation = 0.1f;		// The smoothness of rotation for the Slerp function.
	public float jumpForce = 6f;							// The force used to jump.
	[Range(0f,1f)] public float slideThreshold = 0.6f;		// Determines how steep an angle is.
	public float maxControllableSlideMagnitude = 0.4f;		// Determines how the maximum sliding distance allowes movement.

	#endregion

	#region PRIVATE_VARIABLES

	private Vector3 slideDirection;

	#endregion

	#region PUBLIC_PROPERTIES

	public Vector3 MoveVector {get; set;}					// The desired vector to move to.
	public float VerticalVelocity {get; set;}				// The vertical velocity of the character.

	#endregion

	#region UNITY_FUNCTIONS

	// Use this for initialization
	void Awake () {
		if(instance != this)
			instance = this;
	}

	#endregion

	#region PUBLIC_FUNCTIONS

	// Function to update movement, called by TP_Controller.
	public void UpdateMotor () {
		SnapAlignCharacterWithCamera();
		ProcessMotion();
	}

	public void Jump () {
		if(TP_Controller.characterController.isGrounded) {
			VerticalVelocity = jumpForce;
            TP_Animator.instance.State = TP_Animator.CharacterState.Jumping;
		}
	}

	#endregion

	#region PRIVATE_FUNCTIONS

	// Function to rotate the character with the camera if it's moving, so it will be looking the same way the camera is looking at.
	void SnapAlignCharacterWithCamera () {
		// If the character is moving ...
		if(CheckMovement()){
			// .. change the rotation of the character to the camera's using Slerp.
			Quaternion desiredRotation = Quaternion.Euler(transform.eulerAngles.x,Camera.main.transform.eulerAngles.y,transform.eulerAngles.z);
			transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothRotation);
		}
	}

	// Function to move the character.
	void ProcessMotion () {
		// Transform MoveVector to World Space.
		MoveVector = transform.TransformDirection(MoveVector);

		// Normalise MoveVector if Magnitude > 1
		if(MoveVector.magnitude > 1)
			MoveVector.Normalize();

		// Apply sliding if applicable.
		ApplySlide ();

		// Multiply MoveVector by MoveSpeed.
		MoveVector *= MoveSpeed ();

		// Reapply vertical velocity to the player.
		MoveVector = new Vector3 (MoveVector.x, VerticalVelocity, MoveVector.z);

		// Apply Gravity.
		ApplyGravity ();

		// Rotate the character if it's moving diagonally.
//		RotateCharacter ();
		// Move the character in WorldSpace.
		TP_Controller.characterController.Move(MoveVector * Time.deltaTime);
	}

	float MoveSpeed () {
		float moveSpeed = 0;


		switch (TP_Animator.instance.MoveDirection) {
		case TP_Animator.Direction.Forward :
		case TP_Animator.Direction.LeftForward :
		case TP_Animator.Direction.RightForward :
			moveSpeed = forwardSpeed;
			break;
		case TP_Animator.Direction.Backward :
		case TP_Animator.Direction.LeftBackward :
		case TP_Animator.Direction.RightBackward :
			moveSpeed = backwardSpeed;
			break;
		case TP_Animator.Direction.Left :
		case TP_Animator.Direction.Right :
			moveSpeed = strafingSpeed;
			break;
		case TP_Animator.Direction.Stationary :
			moveSpeed = 0f;
			break;
		}

		if(slideDirection.magnitude > 0){
			moveSpeed = slidingSpeed;
		}
			
		return moveSpeed;
	}

	void RotateCharacter () {
		switch (TP_Animator.instance.MoveDirection) {
		case TP_Animator.Direction.LeftForward :
			transform.Rotate(0,315f,0);
			break;
		case TP_Animator.Direction.RightForward :
			transform.Rotate(0,45f,0);
			break;
		}
		
	}

	void ApplyGravity () {

		if(MoveVector.y > -terminalVelocity){
			MoveVector = new Vector3(MoveVector.x, MoveVector.y - gravity * Time.deltaTime, MoveVector.z);
		}

		// Check if the player is touching the ground and that the vertical movement is less than -1
		if (TP_Controller.characterController.isGrounded && MoveVector.y < -1) {
			MoveVector = new Vector3(MoveVector.x, -1f, MoveVector.z);
		}

	}

	void ApplySlide () {
		if(!TP_Controller.characterController.isGrounded)
			return;

		slideDirection = Vector3.zero;
		RaycastHit hitInfo;

		if(Physics.Raycast(transform.position + Vector3.up, Vector3.down , out hitInfo)) {
			// If the angle is steep then we slide.
			if(hitInfo.normal.y < slideThreshold){
				slideDirection = new Vector3 (hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
			}
		}
			
		if(slideDirection.magnitude < maxControllableSlideMagnitude) {
			MoveVector += slideDirection;
		}
		else {
			MoveVector = slideDirection;
		}
	}

	// Function to check if the character is moving or not.
	bool CheckMovement() {
		return MoveVector.x != 0 || MoveVector.z != 0;
	}

	#endregion
}
