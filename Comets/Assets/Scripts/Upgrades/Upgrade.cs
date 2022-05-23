using UnityEngine;

[System.Serializable]
public class UintModifier {
	public float multiplier;
	public uint addition;
	public uint Modify(uint value) {
		return (uint)Mathf.RoundToInt(value * multiplier) + addition;
	}
}

[System.Serializable]
public class FloatModifier {
	public float multiplier;
	public float addition;
	public float Modify(float value) {
		return value * multiplier + addition;
	}
}

[System.Serializable]
public class ResourceCost
{
	public float copper;
	public float gold;
	public float platinum;
	public float plutonium;

	public ResourceCost(float copper = 0, float gold = 0, float platinum = 0, float plutonium = 0)
	{
		this.copper = copper;
		this.gold = gold;
		this.platinum = platinum;
		this.plutonium = plutonium;
	}
	
	public static ResourceCost operator*(ResourceCost cost, float mult) {
		return new ResourceCost(
			cost.copper * mult,
			cost.gold * mult,
			cost.platinum * mult,
			cost.plutonium * mult
		);
	}
	public static ResourceCost operator*(ResourceCost cost, uint mult) {
		return new ResourceCost(
			cost.copper * mult,
			cost.gold * mult,
			cost.platinum * mult,
			cost.plutonium * mult
		);
	}

	public static ResourceCost operator+(ResourceCost a, ResourceCost b) {
		return new ResourceCost(
			a.copper + b.copper,
			a.gold + b.gold,
			a.platinum + b.platinum,
			a.plutonium + b.plutonium
		);
	}
	public static ResourceCost operator-(ResourceCost a, ResourceCost b) {
		return new ResourceCost(
			a.copper - b.copper,
			a.gold - b.gold,
			a.platinum - b.platinum,
			a.plutonium - b.plutonium
		);
	}

	private static uint norm(float x) => (uint)Mathf.RoundToInt(Mathf.Max(x, 0));

	public static explicit operator ResourceGroup(ResourceCost cost) {
		return new ResourceGroup(
			norm(cost.copper),
			norm(cost.gold),
			norm(cost.platinum),
			norm(cost.plutonium)
		);
	}
}

public abstract class Upgrade : ScriptableObject {
	public Sprite sprite;
	public ResourceCost baseCost;
	[System.NonSerialized]
	public ShipController ship;
	[System.NonSerialized]
	public TraderScript trader;
	
	public virtual string Description { get => ""; }
	public virtual ResourceGroup Cost { get => (ResourceGroup)baseCost; }

	// Returns which upgrade will replace the object
	public abstract Upgrade OnBuy();
}