using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public enum ResourceType {
	Copper,
	Gold,
	Platinum,
	Plutonium
};

[System.Serializable]
public class Resource
{
	public ResourceType type;
	public uint amount;

	public Resource(ResourceType type, uint amount = 0)
	{
		this.type = type;
		this.amount = amount;
	}
}

[System.Serializable]
public class ResourceGroup
{
	public uint copper;
	public uint gold;
	public uint platinum;
	public uint plutonium;

	public ResourceGroup(uint copper = 0, uint gold = 0, uint platinum = 0, uint plutonium = 0)
	{
		this.copper = copper;
		this.gold = gold;
		this.platinum = platinum;
		this.plutonium = plutonium;
	}
	
	public static ResourceGroup operator*(ResourceGroup cost, float mult) {
		return new ResourceGroup(
			(uint)Mathf.RoundToInt(cost.copper * mult),
			(uint)Mathf.RoundToInt(cost.gold * mult),
			(uint)Mathf.RoundToInt(cost.platinum * mult),
			(uint)Mathf.RoundToInt(cost.plutonium * mult)
		);
	}
	public static ResourceGroup operator*(ResourceGroup cost, uint mult) {
		return new ResourceGroup(
			cost.copper * mult,
			cost.gold * mult,
			cost.platinum * mult,
			cost.plutonium * mult
		);
	}

	public static ResourceGroup operator+(ResourceGroup a, ResourceGroup b) {
		return new ResourceGroup(
			a.copper + b.copper,
			a.gold + b.gold,
			a.platinum + b.platinum,
			a.plutonium + b.plutonium
		);
	}
	public static ResourceGroup operator-(ResourceGroup a, ResourceGroup b) {
		return new ResourceGroup(
			a.copper - b.copper,
			a.gold - b.gold,
			a.platinum - b.platinum,
			a.plutonium - b.plutonium
		);
	}
}

public interface IResourceAdder {
	/// <summary>Checks if there is enough space to add a certain resource</summary>
	/// <param name="type">Type of resource</param>
	/// <param name="amount">Amount to check</param>
	bool CanAddResources(Resource resource);
	/// <summary>Tries to add a resource</summary>
	/// <param name="type">Type of resource</param>
	/// <param name="amount">Amount to add</param>
	bool AddResources(Resource resource);
}
public interface IResourceRemover {
	/// <summary>Checks if a certain amount of a resource can be removed</summary>
	/// <param name="type">Type of resource</param>
	/// <param name="amount">Amount to remove</param>
	bool CanRemoveResources(Resource resource);
	/// <summary>Checks if all of a resource can be removed</summary>
	/// <param name="type">Type of resource</param>
	bool CanRemoveResources(ResourceType type);
	/// <summary>Checks if all resources can be removed</summary>
	bool CanRemoveResources();
	/// <summary>Removes a certain amount of a resource</summary>
	/// <param name="type">Type of resource</param>
	/// <param name="amount">Amount to remove</param>
	bool RemoveResources(Resource resource);
	/// <summary>Removes all of a certain resource</summary>
	/// <param name="type">Type of resource</param>
	bool RemoveResources(ResourceType type);
	/// <summary>Removes all resources</summary>
	bool RemoveResources();
	/// <summary>Attempt to transfer a certain amount of resources to a taker</summary>
	/// <param name="collector">The recipient of the powerup</param>
	/// <param name="index">The index of the powerup</param>
	bool TransferResources(IResourceAdder taker, Resource resource);
}
public interface IResourceInventory {
	uint resourceCount {get;}
	/// <summary>Gets the amount of a certain resource</summary>
	/// <param name="type">Type of resource</param>
	uint GetResources(ResourceType type);
	/// <summary>Gets the amount of all resources</summary>
	Resource[] GetResources();
}
public interface IResourceDealer : IResourceAdder, IResourceRemover {}


public interface IPowerupAdder {
	/// <summary>Checks if there is enough space to add powerups</summary>
	/// <param name="amount">Amount to check</param>
	bool CanAddPowerups(uint amount);
	/// <summary>Checks if there is enough space to add a powerup</summary>
	bool CanAddPowerup();
	/// <summary>Tries to add powerups</summary>
	/// <param name="powerup">The powerus to add</param>
	bool AddPowerups(Powerup[] powerups);
	/// <summary>Tries to add a powerup</summary>
	/// <param name="powerup">The powerup to add</param>
	bool AddPowerup(Powerup powerup);
}
public interface IPowerupRemover {
	/// <summary>Checks if a powerup can be removed</summary>
	/// <param name="index">The index of the powerup</param>
	bool CanRemovePowerup(uint index);
	/// <summary>Checks if all powerups can be removed</summary>
	bool CanRemovePowerups();
	/// <summary>Tries to remove a powerup</summary>
	/// <param name="index">The index of the powerup</param>
	bool RemovePowerup(uint index);
	/// <summary>Remove all powerups</summary>
	bool RemovePowerups();
	/// <summary>Attempt to transfer a powerup to a taker</summary>
	/// <param name="collector">The recipient of the powerup</param>
	/// <param name="index">The index of the powerup</param>
	bool TransferPowerup(IPowerupAdder taker, uint index);
}
public interface IPowerupInventory
{
	uint powerupCount { get; }
	/// <summary>Tries to get a powerup</summary>
	/// <param name="index">The index of the powerup</param>
	/// <returns>A powerup, or null</returns>
	Powerup GetPowerup(uint index);
	/// <summary>Tries to get a powerup</summary>
	/// <returns>All powerups</returns>
	Powerup[] GetPowerups();
}
public interface IPowerupDealer : IPowerupAdder, IPowerupRemover {}