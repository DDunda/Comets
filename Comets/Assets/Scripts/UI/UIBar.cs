using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar : UIValue<float>
{
	public GameObject bar;
	public Vector2 minScale;
	public Vector2 maxScale;
	[Range(0, 1)]
	public float startValue;

	public Vector2 GetScale(float value) {
		return Vector2.Lerp(minScale, maxScale, value);
	}

    public override float Set(float value) {
		value = Mathf.Clamp01(value);
		bar.transform.localScale = GetScale(value);
		return value;
	}
}
