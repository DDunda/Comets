using System.Collections.Generic;
using UnityEngine;

public class CometFragments : MonoBehaviour
{
	[System.Serializable]
	public class Fragment {
		public Rigidbody2D rb;
		public SpriteRenderer[] sprites;
		public Utility.Range direction;
	}

	public float fadeDuration;
	public Utility.Range fragmentVelocity;
	public Fragment[] fragments;
	
	private float elapsedTime = 0;
	private float _a = 1;

	void Start() {
        foreach(var frag in fragments)
		{
			frag.rb.velocity =  transform.TransformDirection(Utility.FromAngle(frag.direction * Mathf.Deg2Rad)) * fragmentVelocity;
		}
    }

    void Update()
    {
		elapsedTime += Time.deltaTime;
		if(elapsedTime >= fadeDuration) {
			Destroy(gameObject);
			return;
		}
		float a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
		foreach(var frag in fragments) {
			foreach (var sr in frag.sprites) {
				Color c = sr.color;
				c.a = c.a / _a * a;
				sr.color = c;
			}
		}
		_a = a;
	}
}
