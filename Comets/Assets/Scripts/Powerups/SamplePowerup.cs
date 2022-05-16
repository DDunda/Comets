using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Powerup/Sample")]
public class SamplePowerup : Powerup
{
	public Color colour1 = Color.white;
	public Color colour2 = Color.red;

	[System.NonSerialized]
	protected Image image;
	
	public override bool isReady { get => true; }
	public override bool isDepleted { get => false; }

	public override void SetUI(GameObject UIElement) {
		base.SetUI(UIElement);
		image = UIElement.GetComponent<Image>();
		image.color = colour1;
	}

	public override void OnActivate(GameObject parent) {
		image.color = image.color == colour1 ? colour2 : colour1;
	}
}
