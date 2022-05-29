using UnityEngine;

public class Suction : MonoBehaviour
{
	public Vector2 offset;
	public float startRadius;

	public float attractionForce = 2f;
	public int mask;

	private bool _active = false;
	public bool active { get => _active; }

	protected float _radius;
	public virtual float radius {
		get => _radius;
		set => _radius = value;
	}

	public virtual bool CanAttract(Collider2D obj) => true;

	void Start() {
		radius = startRadius;
	}

	void Update() {
		_active = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(
			(Vector2)transform.TransformPoint(offset),
			radius * transform.lossyScale.z,
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
		Utility.DrawCircleGizmo(transform.TransformPoint(offset), startRadius * transform.lossyScale.z);
	}
}
