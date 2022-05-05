using System.Collections.Generic;
using UnityEngine;

public class CometCleaner : MonoBehaviour
{
	public Transform shipTransform;
	public float maxRange;
	public float cleanupInterval;
	private float timeRemaining = 0;

	public static List<GameObject> allComets = new List<GameObject>();

	void Cleanup() {
		for(int i = 0; i < allComets.Count; i++)
		{
			GameObject obj = allComets[i];
			if(obj == null || Vector2.Distance(shipTransform.position, obj.transform.position) > maxRange) {
				Destroy(obj);
				allComets.RemoveAt(i);
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