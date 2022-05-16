using UnityEditor;

[CustomEditor(typeof(UINumber))]
public class UINumberEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UINumber obj = target as UINumber;
		if(obj == null) return;

		obj.Set(obj.startValue);
	}
}