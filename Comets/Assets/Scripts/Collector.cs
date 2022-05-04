using UnityEngine;

[System.Serializable]
public enum ResourceType {
	Copper,
	Gold,
	Platinum,
	Plutonium
};

[System.Serializable]
public class Powerup {
	public string description;
	public Sprite sprite;

	public virtual void OnCollect() {}
	public virtual void OnActivate(GameObject parent) {}
	public virtual bool IsReady() {
		return true;
	}
	public virtual int UsesRemaining() {
		return -1;
	}
}

public interface ICollector
{
	// Returns true if the resource was picked up
	bool CollectResource(ResourceType type, int amount);
	// Returns true if the resource was picked up
	bool CollectPowerup(Powerup powerup);
}
