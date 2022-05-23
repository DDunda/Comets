using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Upgrade/Damage upgrade")]
public class DamageUpgrade : Upgrade
{
	[TextArea(5, 10)]
	public string descriptionFormat;
	public FloatModifier modifier = new FloatModifier();
	public ResourceCost costPerDamage = new ResourceCost();

	public float DamageDelta { get => modifier.Modify(ship.bulletDamage) - ship.bulletDamage; }

	public override string Description { get => string.Format(descriptionFormat, ship.bulletDamage, ship.bulletDamage + DamageDelta); }
	public override ResourceGroup Cost { get => (ResourceGroup)(costPerDamage * DamageDelta + baseCost); }

	public override Upgrade OnBuy() {
		float delta = DamageDelta;
		ship.bulletDamage += delta;
		return this;
	}
}