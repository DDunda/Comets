using UnityEditor;

[CustomEditor(typeof(UIFormat))]
public class UIFormatEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UIFormat obj = target as UIFormat;
		if(obj == null) return;

		obj.textUI.text = obj.formatString;
	}
}