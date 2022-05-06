using UnityEngine;

public class ShipSuction : MonoBehaviour
{
	public Rigidbody2D shipRigidbody;
	public CircleCollider2D area;
	public float radius;
	public float attractionStrength = 2f;

	[System.NonSerialized]
	public int maxAmount = 0;

    void Start()
    {
        area.radius = radius;
    }

	void OnTriggerStay2D(Collider2D collider)
    {
		ResourceScript resource = collider.gameObject.GetComponent<ResourceScript>();
		if(resource.amount > maxAmount) return;

		Rigidbody2D rb = collider.attachedRigidbody;

		rb.velocity -= (rb.position - shipRigidbody.position).normalized * attractionStrength * Time.deltaTime;
    }
}