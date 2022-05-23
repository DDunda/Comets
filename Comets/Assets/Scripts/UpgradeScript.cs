using UnityEngine;
using UnityEngine.UI;

public class UpgradeScript : MonoBehaviour
{
	public Image image;
	public TMPro.TextMeshProUGUI description;
	public UIValue<float> copperCost;
	public UIValue<float> goldCost;
	public UIValue<float> platinumCost;
	public UIValue<float> plutoniumCost;

	private TraderScript trader = null;
	private Upgrade upgrade = null;

	public void SetCost() {
		ResourceGroup cost = upgrade.Cost;

		copperCost.Set((int)cost.copper);
		goldCost.Set((int)cost.gold);
		platinumCost.Set((int)cost.platinum);
		plutoniumCost.Set((int)cost.plutonium);
	}

	public void SetUpgrade(Upgrade upgrade, TraderScript trader) {
		this.upgrade = upgrade;
		this.trader = trader;
		upgrade.trader = trader;
		upgrade.ship = trader.ship;

		image.sprite = upgrade.sprite;
		description.text = upgrade.Description;
		
		SetCost();
	}

	public void BuyUpgrade() {
		trader.BuyUpgrade(upgrade);
		SetCost();
	}
}
