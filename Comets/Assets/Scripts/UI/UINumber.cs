using UnityEngine;

public class UINumber : UIValue<float>
{
	[TextArea(5, 10)]
	[Tooltip("Write {0} in your string to insert a number")]
	public string formatString;
	public float startValue;
	public TMPro.TextMeshProUGUI textUI;

	public string GetText(float value) {
		return string.Format(formatString, value);
	}

    public override float Set(float value) {
		textUI.text = GetText(value);
		return value;
	}
}