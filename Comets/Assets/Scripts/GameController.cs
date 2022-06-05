using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public enum GameState {
		Starting,
		Playing,
		Paused,
		SwitchingScene,
		Won
	}

	public GameState state = GameState.Starting;
	public ShipController ship;
	public TraderScript trader;

	[Header("UI")]
	public GameObject pauseOverlay;
	public GameObject gameOverlay;
	public GameObject deathOverlay;
	public GameObject winOverlay;
	public Image radiationOverlay;

	[Header("Transitions")]
	public SceneSwitcher switcher;
	public int startSwitchID;
	public int exitSwitchID;
	public int reloadSwitchID;

	[Header("Radiation")]
	public Utility.Range radiationTransition;
	public float radiationMaxDamageRate;
	public float radiationMaxAlpha = 0.1f;
	public Sound geigerClick;
	public float maxClickRate;
	public float winRadius;
	private float geigerPoints = 0;

	public void Start() {
		StartGame();
	}

	public void StartCallback() {
		state = GameState.Playing;
	}

	public void Update() {
		float distance = ship.transform.position.magnitude;
		float x = (distance - radiationTransition.min) / (radiationTransition.max - radiationTransition.min);

		x = Mathf.Clamp01(x);

		Color c = radiationOverlay.color;
		c.a = x * radiationMaxAlpha;
		radiationOverlay.color = c;

		geigerPoints += x * Random.value * Time.deltaTime;

		while(geigerPoints > 1f / maxClickRate) {
			AudioManager.PlaySound(geigerClick);
			geigerPoints -= 1f / maxClickRate;
		}

		if(state == GameState.Playing) {
			if(x > 0 && !ship.radiationProof) {
				ship.DoDamage(x * radiationMaxDamageRate * Time.deltaTime, gameObject);
			}

			if(distance > winRadius) {
				Win();
			}

			if (ship.health == 0) {
				Kill();
			}
		}
	}

	public void StartGame() {
		state = GameState.Starting;
		switcher.StartSwitch(startSwitchID);

		if(ship.health == 0) {
			ship.Respawn();
		}
	}

	public void Pause() {
		pauseOverlay.SetActive(true);
		gameOverlay.SetActive(false);
		Time.timeScale = 0;

		state = GameState.Paused;
	}

	public void Unpause() {
		pauseOverlay.SetActive(false);
		gameOverlay.SetActive(true);
		Time.timeScale = 1;

		state = GameState.Playing;
	}

	public void Kill() {
		pauseOverlay.SetActive(false);
		gameOverlay.SetActive(false);
		deathOverlay.SetActive(true);

		state = GameState.SwitchingScene;
	}

	public void Win() {
		pauseOverlay.SetActive(false);
		gameOverlay.SetActive(false);
		winOverlay.SetActive(true);

		state = GameState.Won;
	}

	public void Reload() {
		switcher.StartSwitch(reloadSwitchID);

		state = GameState.SwitchingScene;
	}

	public void OnPause(InputAction.CallbackContext context) {
		if(!context.started) return;
		if(state == GameState.Playing) {
			Pause();
		} else if(state == GameState.Paused) {
			Unpause();
		}
	}

	public void ReturnToMenu() {
		pauseOverlay.SetActive(false);
		gameOverlay.SetActive(false);
		Time.timeScale = 1.0f;
		
		switcher.StartSwitch(exitSwitchID);

		state = GameState.SwitchingScene;
	}
}
