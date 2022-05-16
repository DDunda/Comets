using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIPercentage))]
public class UIPercentageEditor : Editor {
	SerializedProperty textProp;
	SerializedProperty valProp;

	void OnEnable()
	{
		textProp = serializedObject.FindProperty("textUI");
		valProp = serializedObject.FindProperty("startValue");
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		Object obj = textProp.objectReferenceValue;
		if(obj == null) return;

		((TMPro.TextMeshProUGUI)obj).text = ((UIPercentage)target).GetText(valProp.floatValue);
	}
}