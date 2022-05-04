using UnityEngine;

public class CometCleaner : MonoBehaviour
{
	void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.tag != "Comet") return;

		Destroy(other.gameObject);
	}
}