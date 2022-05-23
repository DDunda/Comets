using UnityEditor;
using UnityEngine;
using TMPro;

[CustomEditor(typeof(UIFormat))]
public class UIFormatEditor : Editor {
	[MenuItem("GameObject/UI/Formatted String", false)]
	public static void AddElement(MenuCommand menuCommand)
	{
		GameObject obj = new GameObject("Formatted String", typeof(RectTransform), typeof(UIFormat), typeof(CanvasRenderer), typeof(TextMeshProUGUI));

		RectTransform trans = obj.transform as RectTransform;

		GameObject parent = menuCommand.context as GameObject;
		if (parent != null) trans.SetParent(parent.transform);
		trans.localPosition = Vector2.zero;

		UIFormat fmt = obj.GetComponent<UIFormat>();
		fmt.formatString = "[Text here]: {0} {1} {2} {3}";
		fmt.textUI = obj.GetComponent<TextMeshProUGUI>();
		fmt.textUI.text = fmt.formatString;

		Selection.activeGameObject = obj;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UIFormat obj = target as UIFormat;
		if(obj == null) return;

		obj.textUI.text = obj.formatString;
	}
}