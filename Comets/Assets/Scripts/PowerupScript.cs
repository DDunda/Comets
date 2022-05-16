using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
	public Powerup powerup;
	private Powerup uniquePowerup;

	public static List<GameObject> allPowerups = new List<GameObject>();

	void Start() {
		allPowerups.Add(gameObject);
		uniquePowerup = Instantiate(powerup);
	}


	void OnDestroy() {
		if(uniquePowerup != null) Destroy(uniquePowerup);
	}


	void OnTriggerEnter2D(Collider2D collider) {
		IPowerupAdder adder;
		if(!collider.gameObject.TryGetComponent(out adder)) return;
		if(!adder.AddPowerup(uniquePowerup)) return;

		uniquePowerup = null;
		Destroy(gameObject);
	}
}
