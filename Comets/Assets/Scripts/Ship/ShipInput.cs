using UnityEngine;
using UnityEngine.InputSystem;


public class ShipInput : MonoBehaviour, ShipControls.IShipActions
#if UNITY_EDITOR
	, ShipControls.IShipDebugActions
#endif
{
	public ShipController controller;

	[Header("Acceleration")]
	public SpriteRenderer engineGlow;
	public Transform engineFlame;
	public ParticleSystem engineParticles;
	public float thrustAcceleration = 1f;
	public float brakeAcceleration = 0.2f;
	public float maxEmitRate = 50.0f;
	public AudioSource engineSound;
	[Range(0f, 1f)]
	public float maxEngineVolume;
	[HideInInspector]
	public Vector2 acceleration = Vector2.zero;

	[Header("Rotation")]
	// Time taken to get up to speed
	[Min(0)]
	public float turnTime = 0.2f;
	[Min(0)]
	public float buttonTurnSpeed = 90f;
	[Min(0)]
	public float turnSpeed = 360f;
	[HideInInspector]
	public float angularAcceleration = 0;

	[Header("Mouse control")]
	public new Camera camera;
	public bool mouseTurningEnabled;
	[Min(0)]
	public float mouseMaxAccRadius;
	[Min(0)]
	public float mouseDeadzoneRadius;
	[HideInInspector]
	public Vector2 targetDirection;

	[Header("Shooting")]
	public GameObject bulletPrefab;
	public Vector2 bulletVelocity = new Vector2(0, 5);
	public Vector2 bulletOffset = new Vector2(0, 1);
	public Sound bulletSound;
	[Min(0)]
	public float bulletCooldown;
	private float timeLastFired = 0;

	[Header("Powerups")]
	public GameObject powerupTarget;
	private uint selectedPowerup = 0;

	#if UNITY_EDITOR
		[Header("Debug")]
		public GameObject[] cometPrefabs;
		[Min(0)]
		public float cometSpawnRadius;
		[Min(0)]
		public int cometSpawnAmount;
	#endif

	private ShipControls controls;
	
	private bool canFire {get => Time.time - timeLastFired > bulletCooldown;}
	private ShipInventory inventory { get => controller.inventory; }
	private new Rigidbody2D rigidbody { get => controller.rigidbody; }


	void Awake() {
		controls = new ShipControls();
        engineParticles.Stop();
		controls.Ship.SetCallbacks(this);
		#if UNITY_EDITOR
			controls.ShipDebug.SetCallbacks(this);
		#endif
	}


	void OnEnable() => Enable();
	void OnDisable() => Disable();


	public void Enable() {
		controls.Ship.Enable();
		#if UNITY_EDITOR
			controls.ShipDebug.Enable();
		#endif
	}


	public void Disable() {
		controls.Ship.Disable();
		#if UNITY_EDITOR
			controls.ShipDebug.Disable();
		#endif

		SetEngineStrength(0);
		SetEngineActive(false);

		angularAcceleration = 0;
		targetDirection = Vector2.zero;
	}


	void SetEngineActive(bool b) {
		if(b) engineParticles.Play();
		else engineParticles.Stop();

		engineGlow.enabled = b;
		engineFlame.gameObject.SetActive(b);
	}


	void SetEngineStrength(float x) {
		x = Mathf.Clamp01(x);

		var e = engineParticles.emission;
		e.rateOverTime = maxEmitRate * x;

		engineGlow.color = new Color(1, 1, 1, x);
		engineFlame.localScale = new Vector3(1, x, 1);

		engineSound.volume = x * maxEngineVolume;

		acceleration = rigidbody.transform.up * thrustAcceleration * x;
	}


	Vector2 GetMouseDelta() {
		Vector2 mousePos = Mouse.current.position.ReadValue();
		mousePos = camera.ScreenToWorldPoint((Vector3)mousePos + new Vector3(0,0,-camera.transform.position.z));
		return mousePos - rigidbody.position;
	}


    void Update() {
		if (mouseTurningEnabled && Mouse.current.delta.ReadValue() != Vector2.zero)
		{
			targetDirection = Vector2.zero;

			Vector2 mouseDelta = GetMouseDelta();
			if (mouseDelta.magnitude > mouseDeadzoneRadius)
			{
				targetDirection = Vector2.ClampMagnitude(mouseDelta / mouseMaxAccRadius, 1);
			}
		}
		else if (controls.Ship.Turn.IsPressed())
		{
			if(targetDirection.magnitude == 0) targetDirection = rigidbody.transform.up;
			else targetDirection /= targetDirection.magnitude;
			targetDirection = targetDirection.RotateDirection(buttonTurnSpeed * -controls.Ship.Turn.ReadValue<float>() * Mathf.Deg2Rad * Time.deltaTime);
		}

		SetEngineStrength(controls.Ship.Accelerate.ReadValue<float>() * targetDirection.magnitude);
	}


	void FixedUpdate() {
		float maxAngAcc = turnSpeed / (turnTime > 0 ? turnTime : Time.fixedDeltaTime);

		// The ship will cancel rotation by default
		angularAcceleration = -rigidbody.angularVelocity / Time.fixedDeltaTime;

		if (targetDirection.magnitude > 0)
		{
			float targetAngle = Vector2.SignedAngle(rigidbody.transform.up, targetDirection);

			// Stopping time: v = u + at (v=0) -> t = -u/a
			float td = turnSpeed / maxAngAcc;
			// Time spent deccelerating, calculated based on angle remaining
			float curStopTime = turnTime - Mathf.Sqrt(2 * Mathf.Abs(targetAngle) / maxAngAcc);
			float delTime = Time.fixedDeltaTime;

			float targetVel;

			if (curStopTime + delTime > 0)
			{
				if (curStopTime + delTime > td)
				{
					delTime = td - curStopTime;
				}
				targetVel = (turnSpeed - maxAngAcc * (curStopTime + delTime));
			}
			else
			{
				targetVel = turnSpeed;
			}

			angularAcceleration = (targetVel * Mathf.Sign(targetAngle) - rigidbody.angularVelocity) / Time.fixedDeltaTime;
		}

		if(controls.Ship.Brake.ReadValue<float>() == 1) {
			acceleration = Vector2.ClampMagnitude(-rigidbody.velocity / Time.fixedDeltaTime, brakeAcceleration);
		}

		float _av = rigidbody.angularVelocity;
		angularAcceleration = Mathf.Clamp(angularAcceleration, -maxAngAcc, maxAngAcc);

		rigidbody.velocity += acceleration * Time.fixedDeltaTime;
		rigidbody.angularVelocity += angularAcceleration * Time.fixedDeltaTime;
		rigidbody.angularVelocity = Mathf.Clamp(rigidbody.angularVelocity, -turnSpeed, turnSpeed);

		// Math is pain
		angularAcceleration = (rigidbody.angularVelocity - _av) / Time.fixedDeltaTime;
		rigidbody.rotation -= angularAcceleration * Time.fixedDeltaTime * Time.fixedDeltaTime / 2f;
	}


	public void OnAccelerate(InputAction.CallbackContext context) {
		switch (context.phase)
		{
			case InputActionPhase.Started:
				if(!controls.Ship.Brake.IsPressed()) {
					SetEngineActive(true);
				}
				break;
			case InputActionPhase.Canceled:
				SetEngineActive(false);
				break;
		}
	}


	public void OnBrake(InputAction.CallbackContext context) {
		switch (context.phase)
		{
			case InputActionPhase.Started:
				SetEngineActive(false);
				break;
			case InputActionPhase.Canceled:
				if(controls.Ship.Accelerate.IsPressed()) {
					SetEngineActive(true);
				}
				break;
		}
	}


	public void OnFire(InputAction.CallbackContext context) {
		if(!context.started) return;
		if(!canFire) return;
		timeLastFired = Time.time;
		GameObject bullet = Instantiate(
			bulletPrefab,
			transform.position + transform.TransformVector(bulletOffset),
			transform.rotation
		);
		bullet.GetComponent<Rigidbody2D>().velocity = rigidbody.velocity + (Vector2)transform.TransformDirection(bulletVelocity);
		bullet.GetComponent<BulletManager>().damage = controller.bulletDamage;
		AudioManager.PlaySound(bulletSound);
	}


	public void OnDirection(InputAction.CallbackContext context) {
		targetDirection = context.ReadValue<Vector2>();
	}


	public void OnTurn(InputAction.CallbackContext context) {}


	public void OnUsePowerup(InputAction.CallbackContext context) {
		if(!context.started) return;
		if(inventory.powerupCount == 0) return;

		selectedPowerup = selectedPowerup >= inventory.maxPowerups ? inventory.maxPowerups - 1 : selectedPowerup;

		Powerup powerup = inventory.GetPowerup(selectedPowerup);
		if(powerup == null) return;

		if (!powerup.isDepleted && powerup.isReady) {
			powerup.OnActivate(powerupTarget);

			#if UNITY_EDITOR
				Debug.Log($"Used powerup {selectedPowerup + 1}");
			#endif
		}

		if (powerup.isDepleted) {
			inventory.RemovePowerup(selectedPowerup);
			Destroy(powerup);
		}
	}

	
	public void OnChangePowerup(InputAction.CallbackContext context) {
		if(!context.started) return;

		uint max = inventory.maxPowerups;
		int change = Mathf.RoundToInt(context.ReadValue<float>());
		if(change > 0) {
			do
			{
				selectedPowerup = selectedPowerup + 1 == max ? 0 : selectedPowerup + 1;
				if(inventory.powerupCount == 0) break;
			} while (inventory.powerupSlots[(int)selectedPowerup] == null);
		} else if (change < 0) {
			do
			{
				selectedPowerup = selectedPowerup == 0 ? max - 1 : selectedPowerup - 1;
				if(inventory.powerupCount == 0) break;
			} while (inventory.powerupSlots[(int)selectedPowerup] == null);
		}

		#if UNITY_EDITOR
			Debug.Log($"Powerup changed to {selectedPowerup + 1}");
		#endif
	}


	public void OnSetPowerup(InputAction.CallbackContext context) {
		if(!context.started) return;

		selectedPowerup = (uint)Mathf.RoundToInt(context.ReadValue<float>());
		selectedPowerup = selectedPowerup >= inventory.maxPowerups ? inventory.maxPowerups - 1 : selectedPowerup;

		#if UNITY_EDITOR
			Debug.Log($"Powerup set to {selectedPowerup + 1}");
		#endif
	}


	#if UNITY_EDITOR
		public void OnFreezeSpeed(InputAction.CallbackContext context) {
			if(!context.started) return;

			rigidbody.velocity = Vector2.zero;
			rigidbody.angularVelocity = 0;
			SetEngineStrength(0);
			angularAcceleration = 0;
		}
		public void OnWarpToCenter(InputAction.CallbackContext context) {
			if(!context.started) return;

			rigidbody.position = Vector2.zero;
			rigidbody.rotation = 0;
		}
		public void OnSpawnComets(InputAction.CallbackContext context) {
			if(!context.started) return;

			for(int i = 0; i < cometSpawnAmount; i++) {
				int randType = Random.Range(0, cometPrefabs.Length);
				GameObject child = Instantiate(
					cometPrefabs[randType],
					transform.position + (Vector3)Utility.RandomWithinCircle(5, cometSpawnRadius),
					cometPrefabs[randType].transform.rotation
				);
				child.GetComponent<Rigidbody2D>().velocity = Utility.RandomWithinCircle(5);
			}
		}
	#endif
}