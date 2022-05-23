using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIBar))]
public class UIBarEditor : Editor {

    private static void SetInstant(UIBar bar, float value) {
		value = Mathf.Clamp01(value);
		bar.bar.transform.localScale = bar.GetScale(value);
	}

	private static UIBar CreateBar(string name, MenuCommand menuCommand) {
		GameObject obj1 = new GameObject(name, typeof(RectTransform), typeof(UIBar), typeof(CanvasRenderer), typeof(Image));
		GameObject obj2 = new GameObject("Bar overlay", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

		RectTransform trans1 = obj1.transform as RectTransform;
		RectTransform trans2 = obj2.transform as RectTransform;
		UIBar bar = obj1.GetComponent<UIBar>();
		Image img1 = obj1.GetComponent<Image>();
		Image img2 = obj2.GetComponent<Image>();

		GameObject parent = menuCommand.context as GameObject;
		if (parent != null) trans1.SetParent(parent.transform);

		trans1.pivot = Vector2.one / 2;
		trans1.localPosition = Vector3.zero;
		trans1.anchoredPosition = Vector2.zero;

		trans2.SetParent(trans1);
		trans2.anchorMin = Vector2.zero;
		trans2.anchorMax = Vector2.one;
		trans2.sizeDelta = Vector2.zero;
		trans2.localPosition = Vector3.zero;
		trans2.anchoredPosition = Vector2.zero;

		bar.bar = obj2;
		bar.startValue = 0.5f;

		img1.color = Color.HSVToRGB(0, 1, 0.5f);
		img2.color = Color.HSVToRGB(0, 1, 1.0f);

		Selection.activeGameObject = bar.gameObject;

		return bar;
	}

	[MenuItem("GameObject/UI/Horizontal Bar", false)]
	public static void AddHorizontal(MenuCommand menuCommand)
	{
		UIBar bar = CreateBar("Horizontal Bar", menuCommand);

		RectTransform trans = bar.bar.transform as RectTransform;
		trans.pivot = Vector2.up / 2;

		bar.minScale = Vector2.up;
		bar.maxScale = Vector2.one;
		SetInstant(bar, 0.5f);
	}

	[MenuItem("GameObject/UI/Vertical Bar", false)]
	public static void AddVertical(MenuCommand menuCommand)
	{
		UIBar bar = CreateBar("Vertical Bar", menuCommand);

		RectTransform trans = bar.bar.transform as RectTransform;
		trans.pivot = Vector2.right / 2;

		bar.minScale = Vector2.right;
		bar.maxScale = Vector2.one;
		SetInstant(bar, 0.5f);
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UIBar obj = target as UIBar;
		if(obj == null) return;

		SetInstant(obj, obj.startValue);
	}
}