using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderScript : MonoBehaviour
{
	public enum ShipMode {
		Standyby,
		Docking,
		Holding,
		Ejecting
	}


	public GameObject ship;
	public GameObject UI;
	public CircleCollider2D suctionArea;
	public CircleCollider2D deactivateArea;

	public float attractionStrength = 2f;
	public float reenableDelay;
	public float ejectSpeed;

	public ShipMode mode = ShipMode.Standyby;
	
	private Transform shipTransform;
	private Rigidbody2D shipRigidbody;
	private ShipController shipController;
	private ShipInventory shipInventory;
	private PolygonCollider2D shipCollider;


	void Start() {
		shipTransform = ship.transform;
		shipRigidbody = ship.GetComponent<Rigidbody2D>();
		shipController = ship.GetComponent<ShipController>();
		shipInventory = ship.GetComponent<ShipInventory>();
		shipCollider = ship.GetComponent<PolygonCollider2D>();
	}


	void Update() {
		switch (mode)
		{
			case ShipMode.Standyby:
				if(shipCollider != null && suctionArea.IsTouching(shipCollider)) {
					mode = ShipMode.Docking;
				}
				break;
			case ShipMode.Docking:
				if(shipCollider != null) {
					if(!suctionArea.IsTouching(shipCollider)) {
						mode = ShipMode.Standyby;
					} else if(deactivateArea.IsTouching(shipCollider)) {
						Dock();
					} else {
						shipRigidbody.velocity -= (shipRigidbody.position - (Vector2)transform.position).normalized * attractionStrength * Time.deltaTime;
					}
				} else {
					mode = ShipMode.Standyby;
				}
				break;
		}
	}


	public void SellEverything() {
		if(shipInventory == null) return;
		shipInventory.resources.Clear();
		shipInventory.powerups.Clear();
	}


	public void Repair() {
		if(shipController == null) return;
		shipController.health = shipController.maxHealth;
	}


	void Dock() {
		if(ship != null) {
			shipRigidbody.velocity = Vector2.zero;
			shipTransform.position = transform.position;
			shipController.DisableShip();
		}

		UI.SetActive(true);

		mode = ShipMode.Holding;
	}


	void ReEnable() {
		deactivateArea.enabled = true;
		suctionArea.enabled = true;
		if(ship != null) shipController.EnableShip();
		mode = ShipMode.Standyby;
	}


	public void Leave() {
		deactivateArea.enabled = false;
		suctionArea.enabled = false;
		UI.SetActive(false);
		mode = ShipMode.Ejecting;

		if(ship != null) {
			shipRigidbody.simulated = true;
			shipRigidbody.velocity = Utility.RandomDirection() * ejectSpeed;
			shipRigidbody.rotation = Vector2.SignedAngle(Vector2.up, shipRigidbody.velocity);
		}

    	Invoke("ReEnable", reenableDelay);
	}
}
