using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometScript : MonoBehaviour, IDamageable
{
	public SpriteRenderer cometRenderer;
	public Rigidbody2D cometRigidbody;
	public GameObject explosionPrefab;

	public WeightedArray<Sprite> sprites = new WeightedArray<Sprite>();
	public LootTable lootTable;
	public Utility.Range rewardRange;
	public Utility.Range spawnRadius;
	public Utility.Range randomScale;
	public Utility.Range splitSpinSpeed;
	public Utility.Range inheritSpinAmount;
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

		GameObject child = Instantiate(prefab, transform.position + (Vector3)randomPos, transform.rotation);

		child.transform.localScale *= randomScale;

		Rigidbody2D rb;
		if(!child.TryGetComponent(out rb)) return child;

		rb.velocity = randomPos / spawnRadius.max * splitSpeed;
		rb.angularVelocity = splitSpinSpeed;

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
			for(int i = 0; i < drop.count; i++)
			{
				Rigidbody2D rb = SpawnChild(drop.prefab).GetComponent<Rigidbody2D>();
				totalMass += rb.mass;
				momentum += rb.mass * rb.velocity;

				childBodies.Add(rb);
			}
		}

		if(inheritSpeed) {
			momentum -= cometRigidbody.mass * cometRigidbody.velocity;
		}

		Vector2 dV = momentum / totalMass;

		foreach(var rb in childBodies) {
			rb.velocity -= dV;
			rb.angularVelocity += cometRigidbody.angularVelocity * inheritSpinAmount;
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