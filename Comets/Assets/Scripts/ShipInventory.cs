using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipInventory : MonoBehaviour
{
	[System.Serializable]
	public class ResourcePair {
		public ResourceType type;
		public int amount;

		public ResourcePair(ResourceType type, int amount = 0) {
			this.type = type;
			this.amount = amount;
		}
	}

	public TextMeshProUGUI cargoText;
	public ShipSuction attractor;

	public List<ResourcePair> resources = new List<ResourcePair>();
	public int maxResources;
	public List<Powerup> powerups = new List<Powerup>();
	public int maxPowerups;


	private int resourceCount = 0;


	public void Update() {
		resourceCount = 0;
		foreach(var pair in resources) {
			resourceCount += pair.amount;
		}
		attractor.maxAmount = maxResources - resourceCount;
		cargoText.text = $"Cargo: {Mathf.Round(resourceCount / maxResources * 100)}%";
	}


	public bool CollectResource(ResourceType type, int amount) {
		if (resourceCount + amount > maxResources) return false;

		int slot = resources.FindIndex(i => {return i.type == type;});
		if(slot == -1) {
			resources.Add(new ResourcePair(type, amount));
		} else {
			resources[slot].amount += amount;
		}

		return true;
	}


	public bool CollectPowerup(Powerup powerup) {
		if(powerups.Count > maxPowerups) return false;

		powerups.Add(powerup);
		return true;
	}

	public float TakeResource(ResourceType type, int amount) {
		int slot = resources.FindIndex(i => {return i.type == type;});
		if(slot == -1) return 0;
		if(amount >= resources[slot].amount) {
			amount = resources[slot].amount;
			resources.RemoveAt(slot);
		} else {
			resources[slot].amount -= amount;
		}

		return amount;
	}
}
