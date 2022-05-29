using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public ShipController ship;
	public TraderScript trader;

	public GameObject pauseOverlay;
	public GameObject deathOverlay;

	public Animation fades;

	public enum GameState {
		Starting,
		Playing,
		Paused,
		Dead,
		Restarting,
		Quitting
	}

	public static GameState state = GameState.Starting;

	public float pauseTimeDelay;

	void Start() {
	}

	void StartGame() {
		state = GameState.Starting;
		fades.Play("Scene fade in");
	}

	void Pause() {

	}

	void ReturnToMenu() {
		SceneManager.LoadScene(0);
	}
}
