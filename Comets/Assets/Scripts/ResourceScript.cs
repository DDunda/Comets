using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{
	public ResourceType type;
	public int amount = 1;

	public static List<GameObject> allResources = new List<GameObject>();

	void Start() {
		allResources.Add(gameObject);
	}


	void OnTriggerEnter2D(Collider2D collider) {
		ICollector collector;
		if(!collider.gameObject.TryGetComponent(out collector)) return;
		if(!collector.CollectResource(type, amount)) return;

		Destroy(gameObject);
	}
}
