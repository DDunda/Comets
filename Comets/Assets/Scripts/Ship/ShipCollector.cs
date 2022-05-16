using UnityEngine;

public class ShipCollector : Suction, IResourceAdder, IPowerupAdder
{
	public ShipInventory inventory;

	public void Start() => mask = LayerMask.GetMask("Resources") | LayerMask.GetMask("Powerups");

	public override bool CanAttract(Collider2D obj) {
		ResourceScript resource;
		if(obj.TryGetComponent(out resource)) {
			return CanAddResources(resource.resource);
		}
		PowerupScript powerup;
		if(obj.TryGetComponent(out powerup)) {
			return CanAddPowerup();
		}
		return false;
	}

	public bool CanAddResources(Resource resource) => inventory.CanAddResources(resource);
	public bool AddResources(Resource resource) => inventory.AddResources(resource);
	public bool CanAddPowerups(uint amount) => inventory.CanAddPowerups(amount);
	public bool CanAddPowerup() => inventory.CanAddPowerup();

	public bool AddPowerups(Powerup[] powerupsToAdd) => inventory.AddPowerups(powerupsToAdd);
	public bool AddPowerup(Powerup powerup) => inventory.AddPowerup(powerup);
}