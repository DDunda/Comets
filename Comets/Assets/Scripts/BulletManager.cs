using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
	public float damage = 1f;
	public float lifetime = 10f;
	public object customData = null;


	void Start()
	{
		if (lifetime > 0)
		{
			GameObject.Destroy(gameObject, lifetime);
		}
	}


	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Damageable") {
			(Collision2D collision, BulletManager bullet) package = (collision, this);
			collision.gameObject.SendMessageUpwards("OnDamage", package);
		}
		GameObject.Destroy(gameObject);
	}
}