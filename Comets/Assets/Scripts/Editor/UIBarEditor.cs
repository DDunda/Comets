using UnityEditor;

[CustomEditor(typeof(UIBar))]
public class UIBarEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UIBar obj = target as UIBar;
		if(obj == null) return;

		obj.Set(obj.startValue);
	}
}