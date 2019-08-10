using UnityEngine;
using System.Collections;

public class TP_Helper : MonoBehaviour {

	public struct ClipPlanePoints {
		public Vector3 upperRight;
		public Vector3 upperLeft;
		public Vector3 lowerRight;
		public Vector3 lowerLeft;
	}

	/// <summary>
	/// Computes the near ClipPlanePoints of the camera of a given position.
	/// </summary>
	/// <returns>The computed clip plane points structure.</returns>
	/// <param name="cameraPosition">Supposed camera position.</param>
	public static ClipPlanePoints NearClipPlane (Vector3 cameraPosition, float cameraFieldOfVision)
	{
		ClipPlanePoints clipPlanePoints = new ClipPlanePoints ();

		if(Camera.main == null)
			return clipPlanePoints;

		Transform cameraTransform = Camera.main.transform;
		float halfFOV = 60f / 2 * Mathf.Rad2Deg;
		float aspectRatio = Camera.main.aspect;
		float distance = Camera.main.nearClipPlane;
		float height = Mathf.Tan(halfFOV) * distance + 0.03f;
		float width = height * aspectRatio + 0.03f;

		clipPlanePoints.lowerRight = (cameraPosition + cameraTransform.right * width) - (cameraTransform.up * height) + cameraTransform.forward * distance;
		clipPlanePoints.lowerLeft = (cameraPosition - cameraTransform.right * width) - (cameraTransform.up * height) + cameraTransform.forward * distance;
		clipPlanePoints.upperRight = (cameraPosition + cameraTransform.right * width) + (cameraTransform.up * height) + cameraTransform.forward * distance;
		clipPlanePoints.upperLeft = (cameraPosition - cameraTransform.right * width) + (cameraTransform.up * height) + cameraTransform.forward * distance;

		return clipPlanePoints;
	}

	// Function to clamp an angle between a lower limit and a max limit.
	/// <summary>
	///  Function to clamp an angle between a lower limit and a max limit.
	/// </summary>
	/// <returns>The clamped angle.</returns>
	/// <param name="angle">Angle to be clamped.</param>
	/// <param name="minAngle">The lower limit.</param>
	/// <param name="maxAngle">The upper limit.</param>
	public static float ClampAngle (float angle, float minAngle, float maxAngle){

		// Make sure the angle is not overflowing one circle (360 degress)
		do {
			if(angle < -360) {
				angle += 360;
			}
			if (angle > 360) {
				angle -= 360;
			}

		} while (angle < -360 || angle > 360);

		// Clamp the value into the desired min and max limits.
		return Mathf.Clamp(angle, minAngle, maxAngle);
	}
}
