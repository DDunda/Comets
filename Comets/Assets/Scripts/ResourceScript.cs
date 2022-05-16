using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{
	public Resource resource;

	public static List<GameObject> allResources = new List<GameObject>();

	void Start() {
		allResources.Add(gameObject);
	}


	void OnTriggerEnter2D(Collider2D collider) {
		IResourceAdder adder;
		if(!collider.gameObject.TryGetComponent(out adder)) return;
		if(!adder.AddResources(resource)) return;

		Destroy(gameObject);
	}
}
