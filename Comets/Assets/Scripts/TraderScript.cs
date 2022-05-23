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
	public GameObject gameOverlay;
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

	public UIValue<float> copperAmount;
	public UIValue<float> goldAmount;
	public UIValue<float> platinumAmount;
	public UIValue<float> plutoniumAmount;

	[SerializeField]
	private ResourceDict resources = new ResourceDict();

	public List<Upgrade> startingUpgrades = new List<Upgrade>();
	public GameObject upgradeElementPrefab;
	public RectTransform upgradeContainer;
	public RectTransform contentContainer;
	private List<Upgrade> upgrades = new List<Upgrade>();
	private List<GameObject> upgradeElements = new List<GameObject>();


	void SetUpgrade(int index, Upgrade upgrade) {
		if(index >= upgrades.Count) return;

		if(upgrade == null){
			RemoveUpgrade(index);
			return;
		}

		upgradeElements[index].GetComponent<UpgradeScript>().SetUpgrade(upgrade, this);
	}

	void AddUpgrade(Upgrade upgrade) {
		Transform parent = upgradeElements.Count == 0
			? upgradeContainer.transform
			: upgradeElements[upgradeElements.Count - 1].transform;

		GameObject obj = Instantiate(
			upgradeElementPrefab,
			parent.position,
			Quaternion.identity,
			parent
		);

		var t = obj.transform as RectTransform;
		t.anchoredPosition = Vector2.zero;
		var r = t.rect;

		var sd = contentContainer.sizeDelta;
		sd.y += r.yMax - r.yMin;
		contentContainer.sizeDelta = sd;

		obj.GetComponent<UpgradeScript>().SetUpgrade(upgrade, this);

		upgradeElements.Add(obj);
		upgrades.Add(upgrade);
	}

	void RemoveUpgrade(int i) {
		if(i >= upgradeElements.Count) return;

		upgrades.RemoveAt(i);

		GameObject obj = upgradeElements[i];

		if(i < upgradeElements.Count - 1) {
			var child = obj.transform.GetChild(0);
			child.transform.SetParent(obj.transform.parent);
		}

		var t = obj.transform as RectTransform;
		var r = t.rect;

		var sd = upgradeContainer.sizeDelta;
		sd.y -= r.yMax - r.yMin;
		upgradeContainer.sizeDelta = sd;

		upgradeElements.RemoveAt(i);
		Destroy(obj);
	}


	void Start() {
		suction.mask = LayerMask.GetMask("Ship");
		SetUI();
		foreach (var upgrade in startingUpgrades) {
			AddUpgrade(upgrade);
		}
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
			input.enabled = false;
			ship.enabled = false;
		}

		UI.SetActive(true);
		gameOverlay.SetActive(false);

		mode = ShipMode.Holding;
	}


	void ReEnable() {
		deactivateArea.enabled = true;
		suction.enabled = true;
		if (ship != null) input.enabled = true;
		mode = ShipMode.Standyby;
	}


	public void Leave() {
		deactivateArea.enabled = false;
		suction.enabled = false;
		UI.SetActive(false);
		gameOverlay.SetActive(true);

		if(ship != null) {
			ship.enabled = true;
			rigidbody.velocity = Utility.RandomDirection() * ejectSpeed;
			rigidbody.rotation = Vector2.SignedAngle(Vector2.up, rigidbody.velocity);
		}
		
		mode = ShipMode.Ejecting;

    	Invoke("ReEnable", reenableDelay);
	}

	private void SetUI() {
		copperAmount.Set(resources[ResourceType.Copper]);
		goldAmount.Set(resources[ResourceType.Gold]);
		platinumAmount.Set(resources[ResourceType.Platinum]);
		plutoniumAmount.Set(resources[ResourceType.Plutonium]);
	}

	public bool CanAddResources(Resource resource) => true;
	public bool AddResources(Resource resource) {
		resources += resource;
		SetUI();
		return true;
	}

	public uint GetResources(ResourceType type) => resources[type];
	public Resource[] GetResources() => resources.ToArray();

	public void BuyUpgrade(Upgrade upgrade) {
		if(upgrade == null) return;

		ResourceGroup cost = upgrade.Cost;
		if(resources < cost) return;

		resources -= cost;

		int index = upgrades.FindIndex(u => u == upgrade);
		SetUpgrade(index, upgrade.OnBuy());
		SetUI();
	}
}