using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Upgrade/Suction upgrade")]
public class SuctionUpgrade : Upgrade
{
	[TextArea(5, 10)]
	public string descriptionFormat;
	public FloatModifier modifier = new FloatModifier();
	public ResourceCost costPerRange = new ResourceCost();
	private float totalDelta = 0;

	public float RangeDelta { get => modifier.Modify(ship.suction.radius) - ship.suction.radius; }

	public override string Description { get => string.Format(descriptionFormat, ship.suction.radius, ship.suction.radius + RangeDelta); }
	public override ResourceGroup Cost { get => (ResourceGroup)(costPerRange * (RangeDelta + totalDelta) + baseCost); }

	void Start() {
		totalDelta = 0;
	}

	public override Upgrade OnBuy() {
		ship.suction.radius += RangeDelta;
		totalDelta += RangeDelta;
		return this;
	}
}