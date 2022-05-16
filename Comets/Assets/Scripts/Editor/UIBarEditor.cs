using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIBar))]
public class UIBarEditor : Editor {
	SerializedProperty barProp;
	SerializedProperty valProp;

	void OnEnable()
	{
		barProp = serializedObject.FindProperty("bar");
		valProp = serializedObject.FindProperty("startValue");
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		Object obj = barProp.objectReferenceValue;
		if(obj == null) return;
		
		((GameObject)obj).transform.localScale = ((UIBar)target).GetScale(valProp.floatValue);
	}
}