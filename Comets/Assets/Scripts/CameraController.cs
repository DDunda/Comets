using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Rigidbody2D cameraRigidbody;
	public ShipController shipController;
	public float smoothingFactor = 2f;
	public float dragAfterDestruction;

	private Rigidbody2D shipRigidbody;
	private Vector2 acceleration = Vector2.zero;


	void Start()
	{
		shipRigidbody = shipController.shipRigidbody;
	}


	void OnDestroy() {
		cameraRigidbody.drag = dragAfterDestruction;
	}


    void FixedUpdate()
    {
		float smoothDelta = 1 - Mathf.Pow(smoothingFactor, -Time.fixedDeltaTime);

		Vector2 adiff = shipController.acceleration - acceleration;
		acceleration += adiff * smoothDelta;
		cameraRigidbody.velocity += acceleration * Time.fixedDeltaTime;

		Vector2 vdiff = shipRigidbody.velocity - cameraRigidbody.velocity;
		cameraRigidbody.velocity += vdiff * smoothDelta;

		Vector2 pdiff = shipRigidbody.position - cameraRigidbody.position;
		cameraRigidbody.position += pdiff * smoothDelta;
    }
}