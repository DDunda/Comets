using UnityEngine;

public class Detectable : MonoBehaviour {
	public GameObject radarBlipPrefab;
	private GameObject _radarBlip = null;

	void OnDestroy() {
		if(_radarBlip != null) Destroy(_radarBlip);
		_radarBlip = null;
	}

	public GameObject radarBlip { get => _radarBlip; }

	public void Detect(Transform parent) {
		if(radarBlip != null) return;
		_radarBlip = Instantiate(radarBlipPrefab, parent);
	}
	public bool IsDetected() => _radarBlip != null;
	public void UnDetect() => Destroy(_radarBlip);
}