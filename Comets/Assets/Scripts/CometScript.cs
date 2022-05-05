using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometScript : MonoBehaviour, IDamageable
{
	[System.Serializable]
	public class SpawnPair {
		public GameObject child;
		public int count;
		public float rewardWeight;
	}

	[System.Serializable]
	public class WeightedArray<T> {
		[System.Serializable]
		public class WeightedItem {
			public T item;
			public float weight;
		}

		public delegate bool Filter(T item);

		public WeightedItem[] items;

		public T SelectRandom() {
			float weightSum = 0;
			foreach(var item in items) {
				weightSum += item.weight;
			}

			float weight = Random.Range(0, weightSum);

			foreach (var pair in items)
			{
				if(weight < pair.weight) {
					return pair.item;
				}

				weight -= pair.weight;
			}

			return default(T);
		}

		public T SelectRandom(Filter filter) {
			float weightSum = 0;
			foreach(var item in items) {
				if(filter(item.item)) {
					weightSum += item.weight;
				}
			}

			float weight = Random.Range(0, weightSum);

			foreach (var item in items)
			{
				if(!filter(item.item)) continue;

				if(weight < item.weight) {
					return item.item;
				}

				weight -= item.weight;
			}

			return default(T);
		}
	}

	public SpriteRenderer cometRenderer;
	public Rigidbody2D cometRigidbody;
	public GameObject explosionPrefab;

	public WeightedArray<Sprite> sprites = new WeightedArray<Sprite>();
	public LootTable lootTable;
	public Utility.Range rewardRange;
	public Utility.Range spawnRadius;
	public float splitSpeed;
	public bool inheritSpeed = true;

	public static float collisionDamageFactor = 2f;
	public static float collisionDamageRatio = 0.9f;

	public float health;
	public static float rewardMultiplier = 1;


    void Start() {
        cometRenderer.sprite = sprites.SelectRandom();
		CometCleaner.allComets.Add(gameObject);
    }


	public GameObject SpawnChild(GameObject prefab) {
		Vector2 randomPos = Utility.RandomWithinCircle(spawnRadius);
		Debug.Log(randomPos);

		GameObject child = Instantiate(prefab, transform.position + (Vector3)randomPos, transform.rotation);

		Rigidbody2D rb;
		if(!child.TryGetComponent(out rb)) return child;

		rb.velocity = randomPos.normalized * splitSpeed;

		return child;
	}


	public void Split() {
		float reward = rewardRange * rewardMultiplier;
		var drops = lootTable.CreateDrops(reward);

		List<Rigidbody2D> childBodies = new List<Rigidbody2D>();
		float totalMass = 0;
		Vector2 momentum = Vector2.zero;

		foreach (var drop in drops)
		{
			GameObject child = SpawnChild(drop);
			Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
			totalMass += rb.mass;
			momentum += rb.mass * rb.velocity;
			
			childBodies.Add(rb);
		}

		if(inheritSpeed) {
			momentum -= cometRigidbody.mass * cometRigidbody.velocity;
		}

		Vector2 dV = momentum / totalMass;

		foreach(var rb in childBodies) {
			rb.velocity -= dV;
		}
	}


	public void DoDamage(float damage, GameObject source) {
		health -= damage;
		if(health > 0) return;
		
		Split();
		GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
		explosion.GetComponent<Rigidbody2D>().velocity = cometRigidbody.velocity;
		explosion.SetActive(true);
		Destroy(gameObject);
	}

	
	public void OnCollisionEnter2D(Collision2D other) {
		IDamageable damageListener;
		if(other.gameObject.TryGetComponent(out damageListener)) {
			float damage = other.relativeVelocity.magnitude * collisionDamageFactor;
			damageListener.DoDamage(damage * collisionDamageRatio, gameObject);
			DoDamage(damage * (1 - collisionDamageRatio), other.gameObject);
		}
	}
}