using System.Collections.Generic;
using UnityEngine;

public class ShipCollector : Suction, IResourceAdder, IPowerupAdder
{
	public ShipInventory inventory;
	public GameObject glow;

	[Header("Collection sound")]
	public AudioClip collectionSound;
	public Utility.Range pitch;
	public Utility.Range volume;

	public override float radius {
		get => _radius;
		set {
			glow.transform.localScale = value * Vector3.one;
			_radius = value;
		}
	}

	private static float interval = Mathf.Pow(2f, 1f / 12f);

	public override void Start() {
		base.Start();
		mask = LayerMask.GetMask("Resources") | LayerMask.GetMask("Powerups");
	}

	public override bool CanAttract(Collider2D obj) {
		ResourceScript resource;
		if(obj.gameObject.TryGetComponent(out resource)) {
			return CanAddResources(resource.resource);
		}
		PowerupScript powerup;
		if(obj.gameObject.TryGetComponent(out powerup)) {
			return CanAddPowerup();
		}
		return false;
	}

	public bool CanAddResources(Resource resource) => inventory.CanAddResources(resource);
	public bool AddResources(Resource resource) {
		if (inventory.AddResources(resource))
		{
			AudioManager.PlaySound(
				new Sound(collectionSound, volume, pitch),
				this.gameObject
			);
			return true;
		} else {
			return false;
		}
	}
	public bool CanAddPowerups(uint amount) => inventory.CanAddPowerups(amount);
	public bool CanAddPowerup() => inventory.CanAddPowerup();

	public bool AddPowerups(Powerup[] powerupsToAdd) => inventory.AddPowerups(powerupsToAdd);
	public bool AddPowerup(Powerup powerup) => inventory.AddPowerup(powerup);
}