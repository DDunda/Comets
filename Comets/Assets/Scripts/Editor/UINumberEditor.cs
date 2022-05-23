using UnityEditor;
using UnityEngine;
using TMPro;

[CustomEditor(typeof(UINumber))]
public class UINumberEditor : Editor {
	[MenuItem("GameObject/UI/Number String", false)]
	public static void AddElement(MenuCommand menuCommand)
	{
		GameObject obj = new GameObject("Number String", typeof(RectTransform), typeof(UINumber), typeof(CanvasRenderer), typeof(TextMeshProUGUI));

		RectTransform trans = obj.transform as RectTransform;

		GameObject parent = menuCommand.context as GameObject;
		if (parent != null) trans.SetParent(parent.transform);
		trans.localPosition = Vector2.zero;

		UINumber fmt = obj.GetComponent<UINumber>();
		fmt.formatString = "[Text here]: {0}";
		fmt.startValue = 0;
		fmt.textUI = obj.GetComponent<TextMeshProUGUI>();
		fmt.Set(fmt.startValue);

		Selection.activeGameObject = obj;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UINumber obj = target as UINumber;
		if(obj == null) return;

		obj.Set(obj.startValue);
	}
}