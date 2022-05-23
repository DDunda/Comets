using UnityEditor;
using UnityEngine;
using TMPro;

[CustomEditor(typeof(UIPercentage))]
public class UIPercentageEditor : Editor {
	[MenuItem("GameObject/UI/Percentage String", false)]
	public static void AddElement(MenuCommand menuCommand)
	{
		GameObject obj = new GameObject("Percentage String", typeof(RectTransform), typeof(UIPercentage), typeof(CanvasRenderer), typeof(TextMeshProUGUI));

		RectTransform trans = obj.transform as RectTransform;

		GameObject parent = menuCommand.context as GameObject;
		if (parent != null) trans.SetParent(parent.transform);
		trans.localPosition = Vector2.zero;

		UIPercentage fmt = obj.GetComponent<UIPercentage>();
		fmt.formatString = "[Text here]: {0}%";
		fmt.startValue = 0;
		fmt.textUI = obj.GetComponent<TextMeshProUGUI>();
		fmt.Set(fmt.startValue);

		Selection.activeGameObject = obj;
	}
	
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UIPercentage obj = target as UIPercentage;
		if(obj == null) return;

		obj.Set(obj.startValue);
	}
}