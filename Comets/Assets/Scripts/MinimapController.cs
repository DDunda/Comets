using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
	public ShipController ship;
	public Detectable trader;
	public RectTransform mapContainer;
	public GameObject traderArrow;
	public GameObject shipBlip;

	public float worldRadius = 10.0f;
	public float scaleFactor = 2.0f;

	private int mask = 0;
	private List<Detectable> trackedObjs = new List<Detectable>();

	void Start() {
		mask |= LayerMask.GetMask("Resources");
		mask |= LayerMask.GetMask("Powerups");
		mask |= LayerMask.GetMask("Comet");
		mask |= LayerMask.GetMask("Trading Station");
	}

    void Update()
    {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(
			ship.transform.position,
			worldRadius,
			mask
		);

		trackedObjs.RemoveAll(obj => obj == null);
		List<Detectable> toDelete = new List<Detectable>(trackedObjs);

		foreach(var collider in colliders) {
			var detector = collider.GetComponent<Detectable>();
			if(detector == null) continue;
			if(detector.IsDetected()) {
				toDelete.Remove(detector);
				continue;
			}

			if(detector == trader) traderArrow.SetActive(false);

			detector.Detect(mapContainer);
			trackedObjs.Add(detector);
		}

		foreach (var detector in toDelete) {
			trackedObjs.Remove(detector);
			detector.UnDetect();
			if(detector == trader) traderArrow.SetActive(true);
		}

		float x = Mathf.Pow(2.0f, -scaleFactor);

		foreach (var obj in trackedObjs) {
			GameObject blip = obj.radarBlip;
			RectTransform t = blip.transform as RectTransform;
			Vector2 p = (obj.transform.position - ship.transform.position) / worldRadius;
			p = p * x + p.normalized * (1 - Mathf.Pow(x, p.magnitude));
			t.anchorMin = p;
			t.anchorMax = p;
		}

		mapContainer.pivot = ship.transform.position;

		if (traderArrow.activeInHierarchy) {
			traderArrow.transform.eulerAngles = new Vector3(0, 0,
				Vector2.SignedAngle(
					Vector2.up,
					trader.transform.position - ship.transform.position
				)
			);
		}

		shipBlip.transform.localEulerAngles = new Vector3(0,0,ship.transform.localEulerAngles.z);
		shipBlip.SetActive(ship.ship.activeInHierarchy);
	}
}
