
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class BasicCameraMovement 
	: MonoBehaviour
{

	public float	MaxMovementSpeed	= 3.0f;
	public float	TurnSpeedMult		= 3.0f;

	private Vector3 _previousPosDelta	= Vector3.zero;
	
	void Update ()
	{
		var forwardMult = Input.GetAxis("Vertical");
		var rightMult	= Input.GetAxis("Horizontal");
		var turnH		= Input.GetAxis("Mouse X");
		var turnV		= Input.GetAxis("Mouse Y");


		var startingRot = transform.rotation;
		var rotDelta	= Quaternion.Euler(TurnSpeedMult * new Vector3(-turnV, turnH, 0.0f));
		var finalRot	= startingRot * rotDelta;

		var posDelta = Vector3.zero;

		if (!Mathf.Approximately(forwardMult, 0.0f) || !Mathf.Approximately(rightMult, 0.0f))
		{
			var baseDelta = (forwardMult * transform.forward) + (rightMult * transform.right);

			if (baseDelta.sqrMagnitude > 1.0f)
				baseDelta.Normalize();

			posDelta = Quaternion.Lerp(Quaternion.identity, rotDelta, 0.5f) * (Time.deltaTime * MaxMovementSpeed * baseDelta);
		}

		transform.position += Vector3.Lerp(posDelta, _previousPosDelta, 0.5f);
		transform.rotation = finalRot;

		_previousPosDelta = posDelta;
	}
}
