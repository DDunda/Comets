using UnityEditor;

[CustomEditor(typeof(UIPercentage))]
public class UIPercentageEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UIPercentage obj = target as UIPercentage;
		if(obj == null) return;

		obj.Set(obj.startValue);
	}
}