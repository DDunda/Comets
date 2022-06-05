using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Upgrade/Radiation upgrade")]
public class RadiationUpgrade : Upgrade
{
	[TextArea(5, 10)]
	public string descriptionFormat;

	public override string Description { get => descriptionFormat; }
	public override ResourceGroup Cost { get => (ResourceGroup)baseCost; }

	public override Upgrade OnBuy() {
		ship.radiationProof = true;
		return null;
	}
}