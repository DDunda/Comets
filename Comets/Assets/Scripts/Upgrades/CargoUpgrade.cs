using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Upgrade/Cargo upgrade")]
public class CargoUpgrade : Upgrade
{
	[TextArea(5, 10)]
	public string descriptionFormat;
	public UintModifier modifier = new UintModifier();
	public ResourceCost costPerCargo = new ResourceCost();

	public uint CargoDelta { get => modifier.Modify(ship.inventory.maxResources) - ship.inventory.maxResources; }

	public override string Description { get => string.Format(descriptionFormat, ship.inventory.maxResources, ship.inventory.maxResources + CargoDelta); }
	public override ResourceGroup Cost { get => (ResourceGroup)(costPerCargo * CargoDelta + baseCost); }

	public override Upgrade OnBuy() {
		ship.inventory.maxResources += CargoDelta;
		return this;
	}
}