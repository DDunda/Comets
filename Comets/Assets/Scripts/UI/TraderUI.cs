using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TraderUI : Selectable
{
	private List<GameObject> upgradeElements = new List<GameObject>();

	public TraderScript trader;
	public GameObject upgradeElementPrefab;

	public RectTransform root;
	public ScrollRect scroll;
	public Sound selectSound;

	public Selectable selectableRoot;

	[System.NonSerialized]
	public Transform parent = null;
	[System.NonSerialized]
	public Selectable selectableParent;

    private GameObject selectedObject = null;

	private float buttonOffset {
		get {
			var rt = upgradeElementPrefab.transform as RectTransform;
			return (rt.rect.size * rt.pivot - rt.rect.max).y;
		}
	}

	public void AddUpgrade(Upgrade upgrade) {
		if(parent == null) parent = root;
		if(selectableParent == null) selectableParent = selectableRoot;
		GameObject obj = Instantiate(
			upgradeElementPrefab,
			parent.transform.position,
			Quaternion.identity,
			parent.transform
		);

		var t = obj.transform as RectTransform;
		t.anchoredPosition = Vector2.zero;
		var r = t.rect;

		Vector3 sd = scroll.content.sizeDelta;
		sd.y += buttonOffset;
		scroll.content.sizeDelta = sd;

		var scr = obj.GetComponent<UpgradeScript>();
		scr.SetUpgrade(upgrade, this);
		scr.SetNav(selectableParent, selectableParent.FindSelectableOnDown());

		selectableParent = scr.button;
		parent = t;

		upgradeElements.Add(obj);
	}


	public void SetUpgrade(int index, Upgrade upgrade) {
		upgradeElements[index].GetComponent<UpgradeScript>().SetUpgrade(upgrade, this);
	}
	

	public void RemoveUpgrade(int i) {
		if(i >= upgradeElements.Count) return;

		GameObject obj = upgradeElements[i];
		var t = obj.transform as RectTransform;
		var scr = obj.GetComponent<UpgradeScript>();

		if(obj.transform != parent) {
			var child = t.GetChild(t.childCount - 1);
			child.transform.SetParent(t.parent);
		} else {
			selectableParent = scr.button.navigation.selectOnUp;
		}

		Vector3 sd = scroll.content.sizeDelta;
		sd.y -= buttonOffset;
		scroll.content.sizeDelta = sd;

		scr.RemoveNav();
		upgradeElements.RemoveAt(i);

		Destroy(obj);
	}


	protected override void OnEnable() {
		base.Select();
	}


	public override Selectable FindSelectableOnDown() => selectableRoot.FindSelectableOnUp();
	public override Selectable FindSelectableOnUp() => selectableRoot.FindSelectableOnDown();


	void Update() {
        var e = EventSystem.current;
		if (e.currentSelectedGameObject != null && e.currentSelectedGameObject != selectedObject)
		{
			selectedObject = e.currentSelectedGameObject;
			if (selectedObject != gameObject) {
				AudioManager.PlaySound(selectSound);
			}
		}
		else if (e != null && e.currentSelectedGameObject == null)
			e.SetSelectedGameObject(selectedObject);

		if(upgradeElements.Contains(selectedObject)) {
			var _rect = new Vector3[4];
			var _vrect = new Vector3[4];
			Rect rect = new Rect();
			Rect vrect = new Rect();
			(selectedObject.transform as RectTransform).GetWorldCorners(_rect);
			scroll.viewport.GetWorldCorners(_vrect);
			for (int i = 0; i < 4; i++)
			{
				_rect[i] = scroll.content.worldToLocalMatrix.MultiplyPoint(_rect[i]);
				_vrect[i] = scroll.content.worldToLocalMatrix.MultiplyPoint(_vrect[i]);
				if (i == 0)
				{
					rect.min = _rect[0];
					rect.max = _rect[0];
					vrect.min = _vrect[0];
					vrect.max = _vrect[0];
				} else {
					if(_rect[i].x < rect.xMin) rect.xMin = _rect[i].x;
					if(_rect[i].x > rect.xMax) rect.xMax = _rect[i].x;
					if(_rect[i].y < rect.yMin) rect.yMin = _rect[i].y;
					if(_rect[i].y > rect.yMax) rect.yMax = _rect[i].y;
					if(_vrect[i].x < vrect.xMin) vrect.xMin = _vrect[i].x;
					if(_vrect[i].x > vrect.xMax) vrect.xMax = _vrect[i].x;
					if(_vrect[i].y < vrect.yMin) vrect.yMin = _vrect[i].y;
					if(_vrect[i].y > vrect.yMax) vrect.yMax = _vrect[i].y;
				}
			}
			if(rect.yMax > vrect.yMax) {
				scroll.content.localPosition = scroll.content.localPosition - Vector3.up * (rect.yMax - vrect.yMax + 5);
			}
			if(rect.yMin < vrect.yMin) {
				scroll.content.localPosition = scroll.content.localPosition + Vector3.up * (vrect.yMin - rect.yMin + 5);
			}
		}
	}
}
