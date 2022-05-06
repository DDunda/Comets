using UnityEngine;

public class ShipCollector : MonoBehaviour, ICollector
{
	public ShipInventory inventory;


	public bool CollectResource(ResourceType type, int amount) {
		return inventory.CollectResource(type, amount);
	}


	public bool CollectPowerup(Powerup powerup) {
		return inventory.CollectPowerup(powerup);
	}
}
