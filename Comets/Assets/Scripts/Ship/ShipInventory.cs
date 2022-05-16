using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceDict
{
	[SerializeField]
	private List<Resource> _resources = new List<Resource>();
	private uint _count = 0;
	public uint Count { get => _count; }

	private Resource Get(ResourceType type)
	{
		Resource r = _resources.Find(x => x.type == type);
		if (r == null)
			_resources.Add(r = new Resource(type));

		return r;
	}

	public uint this[ResourceType type]
	{
		get => Get(type).amount;
		set
		{
			var pair = Get(type);
			_count += value - pair.amount;
			pair.amount = value;
		}
	}

	public static ResourceDict operator+(ResourceDict dict, Resource resource) {
		dict[resource.type] += resource.amount;
		return dict;
	}

	public static ResourceDict operator-(ResourceDict dict, Resource resource) {
		if(resource.amount > dict[resource.type]) {
			dict[resource.type] = 0;
		} else {
			dict[resource.type] -= resource.amount;
		}
		return dict;
	}

	public Resource[] ToArray() => _resources.ToArray();

	public void Clear()
	{
		_resources.Clear();
		_count = 0;
	}
}

public class ShipInventory : MonoBehaviour, IResourceInventory, IPowerupInventory, IResourceDealer, IPowerupDealer
{
	[Header("Resources")]
	public UIValue<float> cargoUI;

	[SerializeField]
	private ResourceDict resources = new ResourceDict();
	public uint maxResources;
	
	[SerializeField]
	private List<Powerup> powerups = new List<Powerup>();
	public uint maxPowerups;
	public List<GameObject> powerupSlots = new List<GameObject>();

	public uint resourceCount {get => resources.Count;}
	public uint resourceSpace {get => maxResources - resourceCount;}

	private uint _powerupCount = 0;
	public uint powerupCount {get => _powerupCount;}
	public uint powerupSpace {get => maxPowerups - powerupCount;}

	private void SetCargo() => cargoUI.Set(resourceCount / (float)maxResources);

	public bool CanAddResources(Resource resource) => resourceSpace >= resource.amount;

	public bool AddResources(Resource resource) {
		if (!CanAddResources(resource)) return false;

		resources += resource;
		SetCargo();

		return true;
	}

	public uint GetResources(ResourceType type) => resources[type];
	public Resource[] GetResources() => resources.ToArray();

	public bool CanRemoveResources(Resource resource) => GetResources(resource.type) >= resource.amount;
	public bool CanRemoveResources(ResourceType type) => GetResources(type) > 0;
	public bool CanRemoveResources() => resourceCount > 0;

	public bool RemoveResources(Resource resource) {
		if(!CanRemoveResources(resource)) return false;

		resources -= resource;
		SetCargo();

		return true;
	}
	public bool RemoveResources(ResourceType type) {
		if(!CanRemoveResources(type)) return false;

		resources[type] = 0;
		SetCargo();

		return true;
	}
	public bool RemoveResources() {
		if(!CanRemoveResources()) return false;

		resources.Clear();
		SetCargo();

		return true;
	}

	public bool TransferResources(IResourceAdder target, Resource resource) {
		if(!CanRemoveResources(resource) || !target.CanAddResources(resource)) return false;

		target.AddResources(resource);
		RemoveResources(resource);
		SetCargo();

		return true;
	}

		
	public bool CanAddPowerups(uint amount) => powerupSpace >= amount;
	public bool CanAddPowerup() => powerupSpace > 0;

	public bool AddPowerups(Powerup[] powerupsToAdd) {
		if(!CanAddPowerups((uint)powerupsToAdd.Length)) return false;

		foreach (Powerup powerup in powerupsToAdd)
			AddPowerup(powerup);

		return true;
	}
	public bool AddPowerup(Powerup powerup) {
		if(!CanAddPowerup()) return false;

		int i = 0;
		for (; i < maxPowerups; i++)
		{
			if(i == powerups.Count) {
				powerups.Add(powerup);
				break;
			}
			else if (powerups[i] == null) {
				powerups[i] = powerup;
				break;
			}
		}

		if(i == maxPowerups) return false;

		powerup.OnCollect(this);
		powerup.SetUI(Instantiate(powerup.UIPrefab, powerupSlots[i].transform));
		_powerupCount++;

		return true;
	}

	public Powerup GetPowerup(uint index) 
		=> index < powerups.Count ? powerups[(int)index] : null;
	public Powerup[] GetPowerups()
		=> powerups.ToArray();

	public bool CanRemovePowerup(uint index) => GetPowerup(index) != null;
	public bool CanRemovePowerups()	=> powerupCount > 0;

	public bool RemovePowerup(uint index) {
		if(!CanRemovePowerup(index)) return false;

		powerups[(int)index] = null;
		_powerupCount--;

		return true;
	}
	public bool RemovePowerups() {
		if(!CanRemovePowerups()) return false;

		powerups.Clear();
		_powerupCount = 0;

		return true;
	}

	public bool TransferPowerup(IPowerupAdder target, uint index) {
		if(!CanRemovePowerup(index) || !target.CanAddPowerup()) return false;

		Powerup powerup = GetPowerup(index);
		RemovePowerup(index);
		target.AddPowerup(powerup);

		return true;
	}
}