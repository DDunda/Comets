using UnityEngine;

public class ShipController : MonoBehaviour, IDamageable
{
	public GameController game;

	[Header("Ship parts")]
	public GameObject ship;
	public new Rigidbody2D rigidbody;
	public MinimapController minimap;
	public ShipCollector suction;
	public ShipInput input;

	[Header("Health")]
	public UIValue<float> healthUI;
	[SerializeField]
	private float _health;
	public float _maxHealth = 100;

	[Header("Stats")]
	public ShipInventory inventory;
	public float bulletDamage = 1f;
	public bool radiationProof = false;

	[Header("Spawning/Destruction")]
	public Transform spawnpoint;
	public GameObject explosionParticles;
	public Vector2 explosionOffset;
	public Sound damageSound;
	[Min(0.01f)]
	public float maxDamage;
	public Sound explosionSound;
	
	private float maxVolume;

	public float health {
		get => _health;
		set {
			_health = Mathf.Clamp(value, 0, maxHealth);
			healthUI.Set(_health / _maxHealth);
		}
	}
	public float maxHealth {
		get => _maxHealth;
		set {
			_maxHealth = Mathf.Max(value, 0);
			healthUI.Set(_health / _maxHealth);
		}
	}


	public void Start() {
		health = maxHealth;
		maxVolume = damageSound.volume;
	}


	public void OnEnable() => EnableShip();
	public void OnDisable() => DisableShip();


	public void EnableShip() {
		ship.SetActive(true);
		rigidbody.simulated = true;
	}


	public void DisableShip() {
		rigidbody.simulated = false;
		ship.SetActive(false);
	}


	public void Respawn() {
		transform.position = spawnpoint.position;
		transform.rotation = spawnpoint.rotation;

		inventory.RemoveResources();
		inventory.RemovePowerups();
		health = maxHealth;
		EnableShip();
	}


	public void DestroyShip() {
		Rigidbody2D rb = Instantiate(explosionParticles, transform.TransformPoint(explosionOffset), transform.rotation).GetComponent<Rigidbody2D>();

		if(rb != null) {
			rb.velocity = rigidbody.velocity;
			rb.angularVelocity = rigidbody.angularVelocity;
		}

		AudioManager.PlaySound(explosionSound);

		rigidbody.velocity = Vector2.zero;
		rigidbody.angularVelocity = 0;
		DisableShip();
	}


	public void DoDamage(float damage, GameObject source) {
		health -= damage;
		if(health == 0) DestroyShip();
		else if(source != game) {
			damageSound.volume = Mathf.Clamp01(damage / maxDamage) * maxVolume;
			AudioManager.PlaySound(damageSound);
		}
	}
}
