using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class ShipController : MonoBehaviour, IDamageable, ShipControls.IShipActions, ShipControls.IShipDebugActions
{
	public Camera shipCamera;
	public Rigidbody2D shipRigidbody;

	public float thrustAcceleration = 1f;
	public float brakeAcceleration = 0.2f;
	// Time to get up to speed
	public float turnTime = 0.2f;
	public float turnSpeed = 360f;
	public bool mouseTurningEnabled;
	public float mouseMaxAccRadius;
	public float mouseDeadzoneRadius;

	public ParticleSystem engineParticles;
	public SpriteRenderer engineSprite;

	public GameObject bulletPrefab;
	public Vector2 bulletOffset = new Vector2(0, 1);
	public Vector2 bulletVelocity = new Vector2(0, 5);
	public float bulletCooldown;

	public float maxHealth = 100;
	public TextMeshProUGUI healthText;

	public GameObject explosionParticles;

	public GameObject[] debugCometPrefabs;
	public float debugSpawnRadius;
	public int debugSpawnAmount;

	[System.NonSerialized]
	public float angularAcceleration = 0;
	[System.NonSerialized]
	public Vector2 acceleration = Vector2.zero;

	private ShipControls controls;
	private bool inputEnabled = true;
	private float timeLastFired = 0;
	private float _health;
	private Vector2 targetDirection;
	
	public float health {
		get => _health;
		set {
			_health = Mathf.Clamp(value, 0, maxHealth);
			healthText.text = $"Health: {Mathf.Round(_health / maxHealth * 100)}%";
		}
	}
	private bool CanFire {get => Time.time - timeLastFired > bulletCooldown;}
	

	void Awake() {
		controls = new ShipControls();
		controls.Ship.SetCallbacks(this);
		controls.ShipDebug.SetCallbacks(this);
	}


	void OnEnable() => EnableShip();
	void OnDisable() => DisableShip();


	public void EnableShip() {
		controls.Ship.Enable();
		controls.ShipDebug.Enable();
		shipRigidbody.simulated = true;
		inputEnabled = true;
	}


	public void DisableShip() {
		controls.Ship.Disable();
		controls.ShipDebug.Disable();
		engineParticles.Stop();
		shipRigidbody.simulated = false;
		inputEnabled = false;
	}


    void Start()
    {
        engineParticles.Stop();
		health = maxHealth;
    }


	Vector2 GetMouseDelta() {
		Vector2 mousePos = Mouse.current.position.ReadValue();
		mousePos = shipCamera.ScreenToWorldPoint((Vector3)mousePos + new Vector3(0,0,-shipCamera.transform.position.z));
		return mousePos - shipRigidbody.position;
	}


    void Update()
    {
		if(!inputEnabled) return;
		if(Mouse.current.delta.ReadValue().magnitude > 0) {
			Vector2 mouseDelta = GetMouseDelta();
			if(mouseDelta.magnitude > mouseDeadzoneRadius) {
				targetDirection = Vector2.ClampMagnitude(mouseDelta / mouseMaxAccRadius, 1);
			} else {
				targetDirection = Vector2.zero;
			}
		}
		acceleration = Vector2.zero;

		if(controls.Ship.Accelerate.ReadValue<float>() == 1) {
			acceleration = shipRigidbody.transform.up * thrustAcceleration;
			if(mouseTurningEnabled) {
				if(targetDirection.magnitude > 0) {
					acceleration *= targetDirection.magnitude;
				}
			}
		}
    }


	void FixedUpdate() {
		float maxAngAcc = turnSpeed / (turnTime > 0 ? turnTime : Time.fixedDeltaTime);

		// The ship will cancel rotation by default
		angularAcceleration = -shipRigidbody.angularVelocity / Time.fixedDeltaTime;

		if(inputEnabled) {
			if(targetDirection.magnitude > 0) {
				float targetAngle = Vector2.SignedAngle(shipRigidbody.transform.up, targetDirection);

				// Stopping time: v = u + at (v=0) -> t = -u/a
				float td = turnSpeed / maxAngAcc;
				// Time spent deccelerating, calculated based on angle remaining
				float curStopTime = turnTime - Mathf.Sqrt(2 * Mathf.Abs(targetAngle) / maxAngAcc);
				float delTime = Time.fixedDeltaTime;
				
				float targetVel;

				if(curStopTime + delTime > 0) {
					if(curStopTime + delTime > td) {
						delTime = td - curStopTime;
					}
					targetVel = (turnSpeed - maxAngAcc * (curStopTime + delTime));
				} else {
					targetVel = turnSpeed;
				}

				angularAcceleration = (targetVel * Mathf.Sign(targetAngle) - shipRigidbody.angularVelocity) / Time.fixedDeltaTime;

			} else if(controls.Ship.Turn.IsPressed()) {
				angularAcceleration = maxAngAcc * -controls.Ship.Turn.ReadValue<float>();
			}

			if(controls.Ship.Brake.ReadValue<float>() == 1) {
				acceleration = Vector2.ClampMagnitude(-shipRigidbody.velocity / Time.fixedDeltaTime, brakeAcceleration);
			}
		}

		float _av = shipRigidbody.angularVelocity;
		angularAcceleration = Mathf.Clamp(angularAcceleration, -maxAngAcc, maxAngAcc);

		shipRigidbody.velocity += acceleration * Time.fixedDeltaTime;
		shipRigidbody.angularVelocity += angularAcceleration * Time.fixedDeltaTime;
		shipRigidbody.angularVelocity = Mathf.Clamp(shipRigidbody.angularVelocity, -turnSpeed, turnSpeed);

		// Math is pain
		angularAcceleration = (shipRigidbody.angularVelocity - _av) / Time.fixedDeltaTime;
		shipRigidbody.rotation -= angularAcceleration * Time.fixedDeltaTime * Time.fixedDeltaTime / 2f;
	}


	void Explode() {
		GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation);
		Rigidbody2D rb;
		if(explosion.TryGetComponent<Rigidbody2D>(out rb)) {
			rb.velocity = shipRigidbody.velocity;
		}

		engineParticles.transform.parent = null;
		engineParticles.Stop();
		ParticleSystem.MainModule main = engineParticles.GetComponent<ParticleSystem>().main;
		main.stopAction = ParticleSystemStopAction.Destroy;

		Destroy(gameObject);
	}


	public void DoDamage(float damage, GameObject source) {
		health -= damage;
		if(health == 0) Explode();
	}


	public void OnAccelerate(InputAction.CallbackContext context) {
		if(!inputEnabled) return;
		switch (context.phase)
		{
			case InputActionPhase.Started:
				if(!controls.Ship.Brake.IsPressed()) {
					engineParticles.Play();
					engineSprite.enabled = true;
				}
				break;
			case InputActionPhase.Canceled:
        		engineParticles.Stop();
				engineSprite.enabled = false;
				break;
		}
	}


	public void OnBrake(InputAction.CallbackContext context) {
		if(!inputEnabled) return;
		switch (context.phase)
		{
			case InputActionPhase.Started:
				engineParticles.Stop();
				engineSprite.enabled = false;
				break;
			case InputActionPhase.Canceled:
				if(controls.Ship.Accelerate.IsPressed()) {
					engineParticles.Play();
					engineSprite.enabled = true;
				}
				break;
		}
	}


	public void OnTurn(InputAction.CallbackContext context) {}


	public void OnDirection(InputAction.CallbackContext context) {
		targetDirection = context.ReadValue<Vector2>();
	}


	public void OnFire(InputAction.CallbackContext context) {
		if(!inputEnabled) return;
		if(!context.started) return;
		if(!CanFire) return;
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