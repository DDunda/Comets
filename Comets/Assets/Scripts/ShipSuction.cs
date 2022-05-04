using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSuction : MonoBehaviour
{
	public Rigidbody2D shipRigidbody;
	public CircleCollider2D area;
	public float radius;
	public float attractionStrength = 2f;

    void Start()
    {
        area.radius = radius;
    }

	void OnTriggerStay2D(Collider2D collider)
    {
		Rigidbody2D rb = collider.attachedRigidbody;

		rb.velocity -= (rb.position - shipRigidbody.position).normalized * attractionStrength * Time.deltaTime;
    }
}