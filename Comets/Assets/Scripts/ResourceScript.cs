using UnityEngine;

public class ResourceScript : MonoBehaviour
{
	public ResourceType type;
	public int amount = 1;

	void OnTriggerEnter2D(Collider2D collider) {
		ICollector collector;
		if(!collider.gameObject.TryGetComponent(out collector)) {
			Debug.Log("No collector!");
			return;
		}
		if(!collector.CollectResource(type, amount)) return;

		Destroy(gameObject);
	}
}
