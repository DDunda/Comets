using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform cameraTransform;
	public ShipController shipController;
	public float smoothingFactor = 2f;

	private Rigidbody2D shipRigidbody;
	private Vector2 velocity = Vector2.zero;
	private Vector2 acceleration = Vector2.zero;


	void Start()
	{
		shipRigidbody = shipController.shipRigidbody;
	}


    void FixedUpdate()
    {
		float smoothDelta = 1 - Mathf.Pow(smoothingFactor, -Time.fixedDeltaTime);

		Vector2 adiff = shipController.acceleration - acceleration;
		acceleration += adiff * smoothDelta;
		velocity += acceleration * Time.fixedDeltaTime;

		Vector2 vdiff = (shipRigidbody.velocity + shipController.acceleration * Time.fixedDeltaTime) - velocity;
		velocity += vdiff * smoothDelta;
		cameraTransform.position += (Vector3)velocity * Time.fixedDeltaTime;

		Vector2 pdiff = (shipRigidbody.position + shipRigidbody.velocity * Time.fixedDeltaTime) - (Vector2)cameraTransform.position;
		cameraTransform.position += (Vector3)pdiff * smoothDelta;
    }
}