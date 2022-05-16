using UnityEngine;

public class UIFormat : UIValue<object[]>
{
	[TextArea(5, 10)]
	[Tooltip("Write {0}, {1}, etc. in your string to insert a values")]
	public string formatString;
	public TMPro.TextMeshProUGUI textUI;

	public string GetText(object[] values) {
		return string.Format(formatString, values);
	}

    public override object[] Set(object[] values) {
		textUI.text = GetText(values);
		return values;
	}
}