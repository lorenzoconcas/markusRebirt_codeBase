using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour {

	#region PUBLIC_VARIABLES

	public Transform pivot;												// A reference to the point the camera shuold be looking at and orbiting around.
																		//
	[Range (0f, 10f)] public float startingDistance = 3f;				// How far the camera should be when the player is standing (idle).
	[Range (0f, 10f)] public float walkingDistance = 4f;				// How far the camera should be when the player is walking.
	[Range (0f, 10f)] public float runningDistance = 4.75f;				// How far the camera should be when the player is running.
	public float minDistance = 0.5f;									// Minimum distance the camera is allowed to be close to the pivot point.
	public float maxDistance = 10f;										// Maximum distance the camera is allowed to be further to the pivot point.
	public float minY = -30f;											// The Y lower limit of the camera.
	public float maxY = 60f;											// The Y upper limit of the camera.
	public bool invertX = false;										// Whether the player wants X axis inverted.
	public bool invertY = false;										// Whether the player wants Y axis inverted.
																		//
	[Range(0f,10f)] public float x_mouseSensitivity = 5f;				// The sensitivity of the X axis input.
	[Range(0f,10f)] public float y_mouseSensitivity = 5f;				// The sensitivity of the Y axis input.
	[Range(0f,1f)]  public float smoothness = 0.1f;						// The smoothness for the Slerp movement.
	[Range(0f,1f)]  public float smoothX = 0.05f;						// The smoothness for the SmoothDamp movement.
	[Range(0f,1f)]  public float smoothY = 0.1f;						// The smoothness for the SmoothDamp movement.
	[Range(0f,1f)]  public float smoothDistance = 0.05f;				// The smoothness for Distance Lerp movement between the camera and the pivot point.
	[Range(0f,1f)]  public float standingSmoothDistance = 0.01f;		// The smoothness for the standing distance between the camera and the pivot point.
																		//
	public float occlusionDistanceStep = 0.1f;							// The amount of distance the camera will slide forward when occluded.
	public int maxOcclusionCheckIterations = 10;						// The maximum amount of iterations allowed to check for occlusion.															//
	public bool hideCursor = true;										// Whether the cursor should be locked or not.
	public bool smoothDamp = false;										// Whether the camera movement should use SmoothDamp or Slerp.
																		//
	public bool FOVKick = true;											// Whether the camera should kick the FOV when runnning or not.
	public float runningFOV = 80f;										// The Flied of View of the camera when the player is running.
	[Range(0f,1f)] public float smoothFOV = 0.054f;						// The smoothness of the change in the Field of Vision of the camera.
																		//
	public bool HeadBob = false;										// Whether the camera should bob while moving.
	public float strideLength = 1f;										// The length of each stride in the time domain.
	public TP_HeadBob headBobMotion = new TP_HeadBob();					// The Headbobbing behaviour.

	#endregion

	#region PRIVATE_VARIABLES

	private float distance = 0f;										// The distance between the camera and the pivot point.
	private float desiredDistance = 0f;									// The desired distance between the camera and the pivot point.
	private float mouseX = 0f, mouseY = 0;								// The input variables for the mouse axis.
	private float velocityX = 0f;										// Reference variable for the velocity of the SmoothDamp function.
	private float velocityY = 0f;										// Reference variable for the velocity of the SmoothDamp function.
	private float velocityZ = 0f;										// Reference variable for the velocity of the SmoothDamp function.
	private Vector3 desiredPosition = Vector3.zero;						// The desired position of where the camera should be at.
	private float preOcclusionDistance = 0;								// The distance of the camera before it was occluded.
	private float startingSmoothDistance = 0;							// The value of the smoothDistance public variable at the start of the game.
	private float startingFOV = 0;										// The value of the initial Field of View of the Camera.
	private float desiredFOV = 0;										// The desired Field of View of the camera.
	private float currentFOV = 0;										// The value of the current Flied of View of the camera.
	private float bobbingSpeed = 1f;									// The value of the bobbing speed.
	private bool shouldDoHeadBob = false;								// Whether the camera should bob depening on the state of the player.

	#endregion

	#region UNITY_FUNCTIONS

	// Use this for initialization
	void Start () {
		if(pivot == null)
			FindPivotPoint();
		if(pivot == null)
			return;
		
		// Lock or hide the cursor.
		Cursor.lockState = hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = hideCursor;
		// Setup the initial values.
		Init();
	}

	// Update function gets called once on the start of each frame.
	void Update () {
		// Get input.
		GetPlayerInput();
	}
	
	// LateUpdate is called once at the end of each frame.
	void LateUpdate () {
		// Update position of the camera.
		int count = 0;
		do {
			ResetCameraPosition ();
			ComputeDesiredPosition ();
			count++;
		}while (CheckIfOccluded(count));

		UpdatePosition();
	}

	#endregion

	#region PRIVATE_FUNCTIONS

	/// <summary>
	/// Gets the player input.
	/// </summary>
	void GetPlayerInput () {
		// Read X and Y position of the mouse.
		mouseX += Input.GetAxis("Mouse X") * x_mouseSensitivity * (invertX ? -1 : 1);
		mouseY -= Input.GetAxis("Mouse Y") * y_mouseSensitivity * (invertY ? -1 : 1);

		// Limit the Y rotation between the specified min and max limits.
		mouseY = TP_Helper.ClampAngle(mouseY,minY,maxY);

		if(Input.GetKeyDown(KeyCode.F)){
			smoothDamp = !smoothDamp;
			print ("SmoothDamp: " + smoothDamp);
		}

		if(Input.GetKeyDown(KeyCode.V)){
			FOVKick = !FOVKick;
			print ("FOVKick: " + FOVKick);
		}

		if(Input.GetKeyDown(KeyCode.R)){
			HeadBob = !HeadBob;
			print ("HeadBob: " + HeadBob);
		}

		if(Input.GetKeyDown(KeyCode.C)){
			hideCursor = !hideCursor;
			Cursor.lockState = hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = hideCursor;
			print ("HideCursor: " + hideCursor);
		}
	}

	/// <summary>
	/// Resets the camera position to where it should be in accordance to character movement state.
	/// </summary>
	void ResetCameraPosition () {
		/*
		// Get the state of the player and find the new desired distance of the camera.
		float newDistance = ComputeDistanceFromPlayerState ();
		// Create a new position of where the camera should go to.
		Vector3 newPosition = ComputePosition (newDistance);
		// Check whether the new position is occluded or not.
		float newPositionNearestDistance = CheckCameraPoints (pivot.position, newPosition, Color.blue);
		// If the new position is safe (not occluded) ... 
		if (newPositionNearestDistance == -1 || newPositionNearestDistance > newDistance) {
			// ... set the pre-occlusion distance and the desired distance to the new distance.
			preOcclusionDistance = newDistance;
			desiredDistance = preOcclusionDistance;
		}
		*/

		// Check if the player is walking, running or standing and set the pre-occlusion distance to it.
		preOcclusionDistance = DetermineDistanceFromPlayerState ();
		// If the camera is not occluded ...
		if(preOcclusionDistance < desiredDistance) {
			// ... then set the desired distance to the pre-occlusion distance.
			desiredDistance = preOcclusionDistance;
		}
		// Otherwise, if the camera is currently occluded ...
		else {
			// ... create a new point of where the camera should go to.
			Vector3 newPosition = ComputePosition (preOcclusionDistance);
			// ... check whether the new position is occluded or not.
			float newPositionNearestDistance = CheckCameraPoints (pivot.position, newPosition,Color.green);
			// ... if the new position is safe (not occluded) ...
			if (newPositionNearestDistance == -1 || newPositionNearestDistance > preOcclusionDistance) {
				// ... set the desired distance to the pre-occlusion distance.
				desiredDistance = preOcclusionDistance;
			}
		}
		 
//		print ("preOcclusionDistance = " + preOcclusionDistance + " desiredDistance = " + desiredDistance + " distance = " + distance);
	}

	/// <summary>
	/// Computes the supposed distance between the camera and the player depending on the movement state of the player.
	/// </summary>
	/// <returns>The distance supposed distance from the player</returns>
	float DetermineDistanceFromPlayerState () {
		//TODO: Change the function to depend on the state machine in the TP_ANIMATOR or TP_CONTROLLER
		float newDistance = 0f;
		float newFOV = startingFOV;
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
			// ... setting the desried distance to the walking distance.
			newDistance = walkingDistance;
			// ... do headbob.
			shouldDoHeadBob = true;
			// ... set the bobbing speed to 1.
			bobbingSpeed = 1f;
			// ... set the vertical and horizontal ranges of the HeadBobbing and the stride length to their initial values.
			headBobMotion.ResetRange ();
			// ... if the player is running ...
			if (Input.GetKey(KeyCode.LeftShift)) {
				// ... set the desired distance to the running distance.
				newDistance = runningDistance;
				// ... set the desired Field of View to the running FOV.
				newFOV = runningFOV;
				// ... set the bobbing speed to 5.
				bobbingSpeed = 5f;
				// ... change the horizontal and vertical ranges of the HeadBobbing, along with the length of the stride.
				headBobMotion.ChangeRange (0.15f, 0.20f, 1.3f);
			}
		} else {
			// else if the player is standing still, then reset the values.
			newDistance = startingDistance;
			shouldDoHeadBob = false;
			headBobMotion.ResetRange ();
		}
		desiredFOV = newFOV;
		return newDistance;
	}

	/// <summary>
	/// Computes the desired position of the camera.
	/// </summary>
	void ComputeDesiredPosition ()
	{
		// If the difference between the current distance of the camera and the desired distance is noticeable ...
		if(Mathf.Abs(distance - desiredDistance) > float.Epsilon && desiredDistance > minDistance) {
			// ... then use the Lerp function to change the current distance to the desired distance.
			if(desiredDistance == startingDistance) {
				smoothDistance = standingSmoothDistance;
			} else {
				smoothDistance = startingSmoothDistance;
			}
			distance = Mathf.Lerp(distance,desiredDistance,smoothDistance);
		}

		// Calculate the desired position of the camera.
		desiredPosition = ComputePosition(distance);
	}

	/// <summary>
	/// Computes the supposed position of the camera that is distance units away from the pivot point.
	/// </summary>
	/// <returns>The computed position.</returns>
	/// <param name="distance">The distance of the position to be computed.</param>
	Vector3 ComputePosition (float distance) {
		// Calculate the desired rotation of the camera.
		Quaternion desiredRotation = Quaternion.Euler(mouseY,mouseX,0);

		Vector3 p = Vector3.zero;
		if(HeadBob) {
			if(shouldDoHeadBob) {
				p = headBobMotion.DoHeadBob(bobbingSpeed);
			}
		}
		p.z = -distance;

		// Calculate the desired position of the camera.
		return pivot.position + (desiredRotation * p);
	}
		
	/// <summary>
	/// Checks whether the camera is occluded count times.
	/// </summary>
	/// <returns><c>true</c>, if if occluded was checked, <c>false</c> otherwise.</returns>
	/// <param name="count">How many times should it check.</param>
	bool CheckIfOccluded (int count){
		bool isOccluded = false;

		float nearestDistance = CheckCameraPoints(pivot.position, desiredPosition,Color.white);

		if(nearestDistance > 0f){
			if(count < maxOcclusionCheckIterations){
				isOccluded = true;
				desiredDistance -= occlusionDistanceStep;

				if(desiredDistance < minDistance)
					desiredDistance = minDistance;
			}
			else {
				desiredDistance = nearestDistance - Camera.main.nearClipPlane;
			}

			distance = desiredDistance;
		} 

		return isOccluded;
	}

	/// <summary>
	/// Checks whether the pivot point can see the camera and its near clip plane.
	/// </summary>
	/// <param name="from">The position of the pivot point.</param>
	/// <param name="to">The supposed position of the camera.</param>
	float CheckCameraPoints (Vector3 from, Vector3 to, Color color) {
		float nearestDistance = -1f;

		RaycastHit hitInfo;
		TP_Helper.ClipPlanePoints clipPlanePoints = TP_Helper.NearClipPlane(to, startingFOV);
		Vector3 bumperPoint = to + transform.forward * -Camera.main.nearClipPlane;
		// Draw lines to debug.
		Debug.DrawLine(from, bumperPoint,color);

		Debug.DrawLine(from, clipPlanePoints.lowerLeft, color);
		Debug.DrawLine(from, clipPlanePoints.lowerRight, color);
		Debug.DrawLine(from, clipPlanePoints.upperLeft, color);
		Debug.DrawLine(from, clipPlanePoints.upperRight, color);

		Debug.DrawLine(clipPlanePoints.upperLeft , clipPlanePoints.upperRight, color);
		Debug.DrawLine(clipPlanePoints.upperRight , clipPlanePoints.lowerRight, color);
		Debug.DrawLine(clipPlanePoints.lowerRight , clipPlanePoints.lowerLeft, color);
		Debug.DrawLine(clipPlanePoints.lowerLeft , clipPlanePoints.upperLeft, color);


		// LineCast to check whether the pivot point can see the near clip plane.
		if(Physics.Linecast(from,clipPlanePoints.upperLeft, out hitInfo) && hitInfo.collider.tag != "Player"){
			nearestDistance = hitInfo.distance;
		}
		if(Physics.Linecast(from,clipPlanePoints.lowerLeft, out hitInfo) && hitInfo.collider.tag != "Player"){
			if(hitInfo.distance < nearestDistance || nearestDistance == -1)
				nearestDistance = hitInfo.distance;
		}
		if(Physics.Linecast(from,clipPlanePoints.upperRight, out hitInfo) && hitInfo.collider.tag != "Player"){
			if(hitInfo.distance < nearestDistance || nearestDistance == -1)
				nearestDistance = hitInfo.distance;
		}
		if(Physics.Linecast(from,clipPlanePoints.lowerRight, out hitInfo) && hitInfo.collider.tag != "Player"){
			if(hitInfo.distance < nearestDistance || nearestDistance == -1)
				nearestDistance = hitInfo.distance;
		}
		if(Physics.Linecast(from,bumperPoint, out hitInfo) && hitInfo.collider.tag != "Player"){
			if(hitInfo.distance < nearestDistance || nearestDistance == -1)
				nearestDistance = hitInfo.distance;
		}

		return nearestDistance;
	}

	/// <summary>
	/// Updates the position of the camera to orbit arount pivot.
	/// </summary>
	void UpdatePosition () {

		// Move the camera.
		Vector3 newPosition = Vector3.zero;
		if(smoothDamp) {
			// Use the SmoothDamp function to change the position of the camera.
			float x = Mathf.SmoothDamp(transform.position.x, desiredPosition.x, ref velocityX, smoothX);
			float y = Mathf.SmoothDamp(transform.position.y, desiredPosition.y, ref velocityY, smoothY);
			float z = Mathf.SmoothDamp(transform.position.z, desiredPosition.z, ref velocityZ, smoothX);
			newPosition = new Vector3(x,y,z);
		} else {
			// Or use the Slerp function to change the position of the camera.
			newPosition = Vector3.Slerp(transform.position,desiredPosition,smoothness);
		}
		
		transform.position = newPosition;

		// Look at the character.
		transform.LookAt(pivot);

		// Change FOV.
		if(FOVKick)
			UpdateFOV ();
	}

	/// <summary>
	/// Changes the value of the Field of View of the camera depening on the movement of the player.
	/// </summary>
	void UpdateFOV () {
		// If the difference in the Field of Vision is noticable ...
		if(Mathf.Abs(currentFOV - desiredFOV) > float.Epsilon){
			// ... then keep changing it till it becomes unnoticable.
			currentFOV = Mathf.Lerp(currentFOV, desiredFOV, smoothFOV);
			Camera.main.fieldOfView = currentFOV;
		}
		// else, make sure it goes back to the desired value. 
		else {
			currentFOV = desiredFOV;
			Camera.main.fieldOfView = currentFOV;
		}
	}
		
	/// <summary>
	/// Resets the desired distance and the pre-occlusion distance of the camera to the starting (idle) distance.
	/// </summary>
	void Init () {
		distance = desiredDistance = preOcclusionDistance = startingDistance;
		currentFOV = startingFOV = Camera.main.fieldOfView;
		startingSmoothDistance = smoothDistance;
		headBobMotion.Setup(strideLength);
	}

	/// <summary>
	/// Finds the pivot point automatically.
	/// </summary>
	void FindPivotPoint(){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if(player == null){
			Debug.LogError("ThirdPersonCamera" + " Can't find player object!");
			return;
		}
		pivot = player.GetComponentInChildren<Transform>();
		if(pivot == null){
			Debug.LogError("ThirdPersonCamera" + " Can't find pivot in player object!");
			return;
		}
		if (pivot.name != "Pivot"){
			Debug.LogWarning("ThirdPersonCamera" + " bad reference for pivot!");
		}

	}

	#endregion

}
