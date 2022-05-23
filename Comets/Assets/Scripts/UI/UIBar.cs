using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/Bar")]
public class UIBar : UIValue<float>
{
	public GameObject bar;
	public Vector2 minScale = Vector2.up;
	public Vector2 maxScale = Vector2.one;
	public float delay = 0.5f;
	public float smoothFactor = 2f;
	[Range(0, 1)]
	public float startValue = 0;
	private float _oldValue = 0;
	private float _targetValue = 0;
	private float _timeRemaining = 0;

	public Vector2 GetScale(float value) {
		return Vector2.Lerp(minScale, maxScale, value);
	}

    public override float Set(float value) {
		_timeRemaining = delay;
		_oldValue = _targetValue;
		return _targetValue = Mathf.Clamp01(value);
	}

	public void Update() {
		_timeRemaining -= Time.deltaTime;
		if(_timeRemaining < 0) _timeRemaining = 0;

		float x = Mathf.Pow(_timeRemaining / delay, smoothFactor);
		bar.transform.localScale = GetScale(Mathf.Lerp(_targetValue, _oldValue, x));
	}
}
