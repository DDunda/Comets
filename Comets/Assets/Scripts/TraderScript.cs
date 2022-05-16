using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TraderScript : MonoBehaviour, IResourceAdder, IResourceInventory
{
	public enum ShipMode {
		Standyby,
		Docking,
		Holding,
		Ejecting
	}


	public GameObject UI;
	public CircleCollider2D deactivateArea;
	public Suction suction;

	public float reenableDelay;
	public float ejectSpeed;

	public ShipMode mode = ShipMode.Standyby;
	
	public ShipController ship;
	public PolygonCollider2D shipCollider;

	public Transform shipTransform { get => ship.transform; }
	public ShipInventory inventory { get => ship.inventory; }
	private new Rigidbody2D rigidbody  { get => ship.rigidbody; }
	private ShipInput input { get => ship.input; }

	public uint resourceCount {get => resources.Count;}

	[SerializeField]
	private ResourceDict resources = new ResourceDict();

	public UIValue<object[]> itemStatsUI;


	void Start() {
		suction.mask = LayerMask.GetMask("Ship");
		SetUI();
	}


	void Update() {
		switch (mode)
		{
			case ShipMode.Standyby:
				if (shipCollider != null && deactivateArea.IsTouching(shipCollider))
				{
					Dock();
				}
				break;
		}
	}


	public void SellEverything() {
		if(inventory == null) return;
		Resource[] shipR = inventory.GetResources();
		foreach (Resource r in shipR) {
			inventory.TransferResources(this, r);
		}
	}


	public void Repair() {
		if(ship == null) return;
		ship.health = ship.maxHealth;
	}


	void Dock() {
		if(ship != null) {
			rigidbody.velocity = Vector2.zero;
			rigidbody.angularVelocity = 0;
			rigidbody.transform.position = transform.position;
			rigidbody.simulated = false;
			input.enabled = false;
		}

		UI.SetActive(true);

		mode = ShipMode.Holding;
	}


	void ReEnable() {
		deactivateArea.enabled = true;
		suction.enabled = true;
		if(ship != null) input.enabled = true;
		mode = ShipMode.Standyby;
	}


	public void Leave() {
		deactivateArea.enabled = false;
		suction.enabled = false;
		UI.SetActive(false);
		mode = ShipMode.Ejecting;

		if(ship != null) {
			rigidbody.simulated = true;
			rigidbody.velocity = Utility.RandomDirection() * ejectSpeed;
			rigidbody.rotation = Vector2.SignedAngle(Vector2.up, rigidbody.velocity);
		}

    	Invoke("ReEnable", reenableDelay);
	}

	private void SetUI() {
		object[] vals = new object[4];
		vals[0] = resources[ResourceType.Copper];
		vals[1] = resources[ResourceType.Gold];
		vals[2] = resources[ResourceType.Platinum];
		vals[3] = resources[ResourceType.Plutonium];

		itemStatsUI.Set(vals);
	}

	public bool CanAddResources(Resource resource) => true;
	public bool AddResources(Resource resource) {
		resources += resource;
		SetUI();
		return true;
	}

	public uint GetResources(ResourceType type) => resources[type];
	public Resource[] GetResources() => resources.ToArray();
}
