using UnityEngine;

public class ChunkScript : MonoBehaviour
{
	public int size;
	public int x, y;


	void Start() {
		transform.DetachChildren();
		Destroy(gameObject);
	}


    void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Utility.DrawSquareGizmo(new Vector2(x, y), Vector2.one * size);
	}
}