using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
	public float damage = 1f;
	public float lifetime = 10f;


	void Start()
	{
		if (lifetime > 0)
		{
			GameObject.Destroy(gameObject, lifetime);
		}
	}


	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "LoadArea") return;
		IDamageable damageListener;
		if(collider.gameObject.TryGetComponent<IDamageable>(out damageListener)) {
			damageListener.DoDamage(damage, gameObject);
		}
		GameObject.Destroy(gameObject);
	}
}