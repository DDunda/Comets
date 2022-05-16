using UnityEngine;

[CreateAssetMenu(menuName = "Powerup/Limited")]
public class LimitedPowerup : Powerup
{
	public uint maximumUses;
	private uint usesRemaining;
	private GameObject barObj;
	private UIValue<float> usageBar;

	public override bool isReady { get => true; }
	public override bool isDepleted { get => usesRemaining == 0; }

	public override void SetUI(GameObject UIElement) {
		base.SetUI(UIElement);
		barObj = UIElement.transform.GetChild(1).gameObject;
		usageBar = barObj.GetComponent<UIValue<float>>();
	}

	public override void OnCollect(IResourceAdder collector)
	{
		usesRemaining = maximumUses;
	}
	public override void OnActivate(GameObject parent) {
		barObj.SetActive(true);
		usageBar.Set(usesRemaining-- / (float)maximumUses);
	}
}
