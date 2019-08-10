using UnityEngine;
using System;

[Serializable]
public class TP_HeadBob {

	#region PUBLIC_VARIABLES

	public AnimationCurve bobbingCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0.5f),
															new Keyframe(2f, 0f), new Keyframe(3f, 0.5f),
															new Keyframe(0.4f, 0f)); 
																// A curve of the function Abs(Sin(x)) specifying the headbobbing animation.
	public float verticalBobRange = 0.05f;						// Value specifying the vertical range of the bobbing animation.
	public float horizontalBobRange = 0.1f;						// Value specifying the horizontal range of the bobbing animation.
	public float horizontalToVerticalRatio = 2f;				// Value specifying the ratio of the horizontal range to the vertical range.

	#endregion

	#region PRIVATE_VARIABLES

	private float bobInterval = 0f;								// The stride length in time domain.
	private float curveTime = 0f;								// The total length of the curve in the time domain.
	private float cyclePosX = 0f;								// X cursor of the curve.
	private float cyclePosY = 0f;								// Y cursor of the curve.
																//
	private float initialHorizontalBobRange = 0f;				// The initial value of the Horizontal Bob Range.
	private float initialVerticalBobRange = 0f;					// The initial value of the Vertical Bob Range.
	private float initialHorizontalToVerticalRatio = 0f;		// The initial value of the Horizontal To Vertical Range Ratio.
	private float initialBobInterval = 0f;						// The initial value of the Bob Interval (stride length).

	#endregion

	#region PUBLIC_FUNCTIONS

	/// <summary>
	/// Initialises and readies the Headbobber with initial values.
	/// </summary>
	/// <param name="initialStrideLength">Initial stride length.</param>
	public void Setup (float initialStrideLength) {
		initialBobInterval = bobInterval = initialStrideLength;
		curveTime = bobbingCurve[bobbingCurve.length - 1].time; // The time of the last keyframe in the curve, which is the total time of the curve.
		initialHorizontalBobRange = horizontalBobRange;
		initialVerticalBobRange = verticalBobRange;
		initialHorizontalToVerticalRatio = horizontalToVerticalRatio;
	}
	
	/// <summary>
	/// Does the head bobbing.
	/// </summary>
	/// <returns>A Vector3 of the calculated HeadBob.</returns>
	/// <param name="speed">HeadBobbing speed.</param>
	public Vector3 DoHeadBob (float speed) {

		// Get the bobbing x and y positions of the camera by evaluating the curve.
		float posX = (bobbingCurve.Evaluate(cyclePosX) * horizontalBobRange);
		float posY = (bobbingCurve.Evaluate(cyclePosY) * verticalBobRange);

		// Move the x and y cursor of the curve.
		cyclePosX += (speed * Time.deltaTime ) / bobInterval;
		cyclePosY += ((speed * Time.deltaTime ) / bobInterval) * horizontalToVerticalRatio;
 
		// Make sure the cycle x and y positions are inside the bobbing curve.
		if(cyclePosX > curveTime) {
			cyclePosX -= curveTime;
		}
		if(cyclePosY > curveTime) {
			cyclePosY -= curveTime;
		}

		return new Vector3(posX,posY,0f);
	}

	/// <summary>
	/// Setter function to change the values of horizontal and vertical ranges, along with the length of the stride.
	/// </summary>
	/// <param name="horizontalRange">Horizontal range.</param>
	/// <param name="verticalRange">Vertical range.</param>
	/// <param name="stride">The length of the stride.</param>
	public void ChangeRange (float horizontalRange, float verticalRange, float stride) {
		if (horizontalRange == horizontalBobRange && verticalRange == verticalBobRange)
			return;
		
		bobInterval = stride;
		horizontalBobRange = horizontalRange;
		verticalBobRange = verticalRange;
		horizontalToVerticalRatio = horizontalBobRange / verticalBobRange;
	}

	/// <summary>
	/// Resets the Ranges and the Stride length to their initial values.
	/// </summary>
	public void ResetRange () {
		bobInterval = initialBobInterval;
		horizontalBobRange = initialHorizontalBobRange;
		verticalBobRange = initialVerticalBobRange;
		horizontalToVerticalRatio = initialHorizontalToVerticalRatio;
	}

	#endregion

}
