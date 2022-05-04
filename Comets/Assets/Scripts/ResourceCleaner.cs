using UnityEngine;

public class ResourceCleaner : MonoBehaviour
{
	void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.tag != "Resource") return;

		Destroy(other.gameObject);
	}
}