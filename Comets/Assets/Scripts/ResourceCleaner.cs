using UnityEngine;

public class ResourceCleaner : MonoBehaviour
{
	public Transform shipTransform;
	public float maxRange;
	public float cleanupInterval;
	private float timeRemaining = 0;

	void Cleanup() {
		for(int i = 0; i < ResourceScript.allResources.Count; i++)
		{
			GameObject obj = ResourceScript.allResources[i];
			if(obj == null || Vector2.Distance(shipTransform.position, obj.transform.position) > maxRange) {
				Destroy(obj);
				ResourceScript.allResources.RemoveAt(i);
				i--;
			}
		}
	}

	void Update() {
		timeRemaining -= Time.deltaTime;
		if(timeRemaining <= 0) {
			timeRemaining += cleanupInterval;
			Cleanup();
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Utility.DrawCircleGizmo(shipTransform.position, maxRange);
	}
}