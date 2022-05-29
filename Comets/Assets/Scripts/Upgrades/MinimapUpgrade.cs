using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Upgrade/Minimap upgrade")]
public class MinimapUpgrade : Upgrade
{
	[TextArea(5, 10)]
	public string descriptionFormat;
	public FloatModifier modifier = new FloatModifier();
	public ResourceCost costPerRange = new ResourceCost();
	private float totalDelta = 0;

	public float RangeDelta { get => modifier.Modify(ship.minimap.worldRadius) - ship.minimap.worldRadius; }

	public override string Description { get => string.Format(descriptionFormat, ship.minimap.worldRadius, ship.minimap.worldRadius + RangeDelta); }
	public override ResourceGroup Cost { get => (ResourceGroup)(costPerRange * (totalDelta + RangeDelta) + baseCost); }

	public override Upgrade OnBuy() {
		ship.minimap.worldRadius += RangeDelta;
		totalDelta += RangeDelta;
		return this;
	}
}