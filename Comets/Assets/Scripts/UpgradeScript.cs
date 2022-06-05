using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeScript : MonoBehaviour
{
	public Image image;
	public TMPro.TextMeshProUGUI description;
	public UIValue<float> copperCost;
	public UIValue<float> goldCost;
	public UIValue<float> platinumCost;
	public UIValue<float> plutoniumCost;
	public Button button;

	private TraderUI trader = null;
	private Upgrade upgrade = null;

	public void SetNext(Selectable s) {
		var nav = button.navigation;
		nav.selectOnDown = s;
		button.navigation = nav;

		nav = s.navigation;
		nav.selectOnUp = button;
		s.navigation = nav;
	}

	public void SetLast(Selectable s) {
		var nav = button.navigation;
		nav.selectOnUp = s;
		button.navigation = nav;

		nav = s.navigation;
		nav.selectOnDown = button;
		s.navigation = nav;
	}

	public void SetNav(Selectable last, Selectable next) {
		SetLast(last);
		SetNext(next);
	}

	public void RemoveNav() {
		var nav = button.navigation.selectOnUp.navigation;
		nav.selectOnDown = button.navigation.selectOnDown;
		button.navigation.selectOnUp.navigation = nav;

		nav = button.navigation.selectOnDown.navigation;
		nav.selectOnUp = button.navigation.selectOnUp;
		button.navigation.selectOnDown.navigation = nav;

		nav = button.navigation;
		nav.selectOnUp = null;
		nav.selectOnDown = null;
		button.navigation = nav;
	}

	public void SetCost() {
		ResourceGroup cost = upgrade.Cost;

		copperCost.Set((int)cost.copper);
		goldCost.Set((int)cost.gold);
		platinumCost.Set((int)cost.platinum);
		plutoniumCost.Set((int)cost.plutonium);
	}

	public void SetUpgrade(Upgrade upgrade, TraderUI trader) {
		this.upgrade = upgrade;
		this.trader = trader;
		upgrade.trader = trader.trader;
		upgrade.ship = trader.trader.ship;

		image.sprite = upgrade.sprite;
		description.text = upgrade.Description;
		
		SetCost();
	}

	public void BuyUpgrade() {
		trader.trader.BuyUpgrade(upgrade);
		SetCost();
	}
}
