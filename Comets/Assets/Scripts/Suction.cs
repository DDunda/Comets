using UnityEngine;

public class Suction : MonoBehaviour
{
	public Vector2 offset;
	public float radius;

	public float attractionForce = 2f;
	public int mask;

	private bool _active = false;
	public bool active { get => _active; }

	public virtual bool CanAttract(Collider2D obj) => true;

	void Update() {
		_active = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(
			(Vector2)transform.TransformPoint(offset),
			radius,
			mask
		);

		foreach(var collider in colliders) {
			Rigidbody2D rb = collider.attachedRigidbody;

			if(rb == null) continue;
			if(!CanAttract(collider)) continue;

			_active = true;
			rb.AddForce(((Vector2)transform.TransformPoint(offset) - rb.position).normalized * attractionForce);
		}
    }

    void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Utility.DrawCircleGizmo(transform.TransformPoint(offset), radius);
	}
}
