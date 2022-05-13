using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Rigidbody2D cameraRigidbody;
	public ShipController shipController;
	public float smoothingFactor = 2f;

	private Vector2 acceleration = Vector2.zero;
	private Vector2 targetPosition = Vector2.zero;
	private Rigidbody2D shipRigidbody { get => shipController.shipRigidbody; }
	private Transform shipTransform { get => shipController.transform; }


    void FixedUpdate()
    {
		float smoothDelta = 1 - Mathf.Pow(smoothingFactor, -Time.fixedDeltaTime);

		if(shipController != null) {
			targetPosition = shipTransform.position;

			Vector2 adiff = shipController.acceleration - acceleration;
			acceleration += adiff * smoothDelta;
			cameraRigidbody.velocity += acceleration * Time.fixedDeltaTime;
			cameraRigidbody.position -= acceleration * Time.fixedDeltaTime * Time.fixedDeltaTime / 2f;

			Vector2 vdiff = shipRigidbody.velocity - cameraRigidbody.velocity;
			cameraRigidbody.velocity += vdiff * smoothDelta;
		} else {
			acceleration -= acceleration * smoothDelta;
			cameraRigidbody.velocity += acceleration * Time.fixedDeltaTime;
			cameraRigidbody.position -= acceleration * Time.fixedDeltaTime * Time.fixedDeltaTime / 2f;
			
			cameraRigidbody.velocity -= cameraRigidbody.velocity * smoothDelta;
		}

		Vector2 pdiff = targetPosition - cameraRigidbody.position;
		cameraRigidbody.position += pdiff * smoothDelta;
    }
}