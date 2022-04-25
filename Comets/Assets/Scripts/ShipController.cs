using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour, ShipControls.IShipActions, ShipControls.IShipDebugActions
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

	public float maxHealth = 100;
	public float health;

	public float angularAcceleration = 0;
	public Vector2 acceleration = Vector2.zero;

	private ShipControls controls;


	void Awake() {
		controls = new ShipControls();
		controls.Ship.SetCallbacks(this);
		controls.ShipDebug.SetCallbacks(this);
	}


	void OnEnable() {
		controls.Enable();
	}


	void OnDisable() {
		controls.Disable();
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
			acceleration = (Vector2)shipRigidbody.transform.up * thrustAcceleration;
		}
		else if(controls.Ship.Brake.ReadValue<float>() == 1) {
			acceleration = -(Vector2)shipRigidbody.velocity.normalized * brakeAcceleration;
		}
    }


	void FixedUpdate() {
		shipRigidbody.velocity += acceleration * Time.fixedDeltaTime;
		shipRigidbody.angularVelocity += angularAcceleration * Time.fixedDeltaTime;
		shipRigidbody.angularVelocity = Mathf.Clamp(shipRigidbody.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
	}


	public void OnDamage((Collision2D collision, BulletManager bullet) dat) {
		health -= dat.bullet.damage;
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
		if(context.phase == InputActionPhase.Started) {
			GameObject bullet = Instantiate(
				bulletPrefab,
				transform.position + transform.TransformVector(bulletOffset),
				transform.rotation
			);
			bullet.GetComponent<Rigidbody2D>().velocity = shipRigidbody.velocity + (Vector2)transform.TransformDirection(bulletVelocity);
		}
	}

	
	public void OnFreezeSpeed(InputAction.CallbackContext context) {
		shipRigidbody.velocity = Vector2.zero;
		shipRigidbody.angularVelocity = 0;
		acceleration = Vector2.zero;
		angularAcceleration = 0;
	}


	public void OnWarpToCenter(InputAction.CallbackContext context) {
		shipRigidbody.position = Vector2.zero;
		shipRigidbody.rotation = 0;
	}
}