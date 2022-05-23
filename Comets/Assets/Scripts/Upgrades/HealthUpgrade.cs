using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Upgrade/Health upgrade")]
public class HealthUpgrade : Upgrade
{
	[TextArea(5, 10)]
	public string descriptionFormat;
	public FloatModifier modifier = new FloatModifier();
	public ResourceCost costPerHealth = new ResourceCost();

	public float HealthDelta { get => modifier.Modify(ship.maxHealth) - ship.maxHealth; }

	public override string Description { get => string.Format(descriptionFormat, ship.maxHealth, ship.maxHealth + HealthDelta); }
	public override ResourceGroup Cost { get => (ResourceGroup)(costPerHealth * HealthDelta + baseCost); }

	public override Upgrade OnBuy() {
		float delta = HealthDelta;
		ship.maxHealth += delta;
		ship.health += delta;
		return this;
	}
}