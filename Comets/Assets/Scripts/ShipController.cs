using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class ShipController : MonoBehaviour, IDamageable, ShipControls.IShipActions, ShipControls.IShipDebugActions
{
	public Rigidbody2D shipRigidbody;
	public float thrustAcceleration = 1f;
	public float brakeAcceleration = 0.2f;
	public float turnAcceleration = 0.05f;
	public float maxAngularVelocity;

	public ParticleSystem engineParticles;

	public GameObject bulletPrefab;
	public Vector2 bulletOffset = new Vector2(0, 1);
	public Vector2 bulletVelocity = new Vector2(0, 5);
	public float bulletCooldown;

	public float maxHealth = 100;
	public float health;
	public TextMeshProUGUI healthText;

	public float angularAcceleration = 0;
	public Vector2 acceleration = Vector2.zero;

	public GameObject[] debugCometPrefabs;
	public float debugSpawnRadius;
	public int debugSpawnAmount;

	private ShipControls controls;
	private float timeLastFired = 0;
	

	void Awake() {
		controls = new ShipControls();
		controls.Ship.SetCallbacks(this);
		controls.ShipDebug.SetCallbacks(this);
	}


	void OnEnable() {
		controls.Ship.Enable();
		controls.ShipDebug.Enable();
	}


	void OnDisable() {
		controls.Ship.Disable();
		controls.ShipDebug.Disable();
	}


    void Start()
    {
        engineParticles.Stop();
		health = maxHealth;
    }


    void Update()
    {

		if(controls.Ship.Accelerate.WasPressedThisFrame()) {
			engineParticles.Play();
		}
		if(controls.Ship.Accelerate.WasReleasedThisFrame()) {
        	engineParticles.Stop();
		}

		acceleration = Vector2.zero;
		angularAcceleration = -controls.Ship.Turn.ReadValue<float>() * turnAcceleration;

		if(controls.Ship.Accelerate.ReadValue<float>() == 1) {
			acceleration = shipRigidbody.transform.up * thrustAcceleration;
		}
		else if(controls.Ship.Brake.ReadValue<float>() == 1) {
			acceleration = -shipRigidbody.velocity.normalized * brakeAcceleration;
		}

		healthText.text = $"Health: {Mathf.Round(health / maxHealth * 100)}%";
    }


	void FixedUpdate() {
		shipRigidbody.velocity += acceleration * Time.fixedDeltaTime;
		shipRigidbody.angularVelocity += angularAcceleration * Time.fixedDeltaTime;
		shipRigidbody.angularVelocity = Mathf.Clamp(shipRigidbody.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
	}


	public void DoDamage(float damage, GameObject source) {
		health -= damage;
	}


	public void OnAccelerate(InputAction.CallbackContext context) {
		switch (context.phase)
		{
			case InputActionPhase.Started:
				engineParticles.Play();
				break;
			case InputActionPhase.Performed:
			case InputActionPhase.Canceled:
        		engineParticles.Stop();
				break;
		}
	}


	public void OnBrake(InputAction.CallbackContext context) {
		
	}


	public void OnTurn(InputAction.CallbackContext context) {
		
	}


	public void OnFire(InputAction.CallbackContext context) {
		if(!context.started) return;
		if(Time.time - timeLastFired < bulletCooldown) return;
		timeLastFired = Time.time;
		GameObject bullet = Instantiate(
			bulletPrefab,
			transform.position + transform.TransformVector(bulletOffset),
			transform.rotation
		);
		bullet.GetComponent<Rigidbody2D>().velocity = shipRigidbody.velocity + (Vector2)transform.TransformDirection(bulletVelocity);
	}

	
	public void OnFreezeSpeed(InputAction.CallbackContext context) {
		if(!context.started) return;

		shipRigidbody.velocity = Vector2.zero;
		shipRigidbody.angularVelocity = 0;
		acceleration = Vector2.zero;
		angularAcceleration = 0;
	}


	public void OnWarpToCenter(InputAction.CallbackContext context) {
		if(!context.started) return;

		shipRigidbody.position = Vector2.zero;
		shipRigidbody.rotation = 0;
	}


	public void OnSpawnComets(InputAction.CallbackContext context) {
		if(!context.started) return;

		for(int i = 0; i < debugSpawnAmount; i++) {
			int randType = Random.Range(0, debugCometPrefabs.Length);
			GameObject child = Instantiate(
				debugCometPrefabs[randType],
				transform.position + (Vector3)Utility.RandomWithinCircle(5, debugSpawnRadius),
				debugCometPrefabs[randType].transform.rotation
			);
			child.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * Random.Range(0, 5);
		}
	}
}