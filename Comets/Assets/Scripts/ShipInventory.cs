using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipInventory : MonoBehaviour
{
	[System.Serializable]
	public class ResourcePair {
		public ResourceType type;
		public float amount;

		public ResourcePair(ResourceType type, float amount = 0) {
			this.type = type;
			this.amount = amount;
		}
	}

	public TextMeshProUGUI cargoText;

	public List<ResourcePair> resources = new List<ResourcePair>();
	public float maxResources;
	public List<Powerup> powerups = new List<Powerup>();
	public int maxPowerups;


	private float resourceCount;


	public void Start() {
		resourceCount = 0;
		foreach(var pair in resources) {
			resourceCount += pair.amount;
		}
	}


	public void Update() {
		cargoText.text = $"Cargo: {Mathf.Round(resourceCount / maxResources * 100)}%";
	}


	public bool CollectResource(ResourceType type, float amount) {
		if (resourceCount + amount > maxResources) return false;

		int slot = resources.FindIndex(i => {return i.type == type;});
		if(slot == -1) {
			resources.Add(new ResourcePair(type, amount));
		} else {
			resources[slot].amount += amount;
		}
		resourceCount += amount;

		return true;
	}


	public bool CollectPowerup(Powerup powerup) {
		if(powerups.Count > maxPowerups) return false;

		powerups.Add(powerup);
		return true;
	}
}
