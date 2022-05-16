using UnityEngine;

public class UIPercentage : UIValue<float>
{
	[TextArea(5, 10)]
	[Tooltip("Write {0} in your string to insert a percentage")]
	public string formatString;
	[Range(0, 1)]
	public float startValue;
	public TMPro.TextMeshProUGUI textUI;

	public string GetText(float value) {
		return string.Format(formatString, value * 100);
	}

    public override float Set(float value) {
		value = Mathf.Clamp01(value);
		textUI.text = GetText(value);
		return value;
	}
}