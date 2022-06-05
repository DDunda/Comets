using UnityEngine;

public class MainMenuController : MonoBehaviour
{
	public SceneSwitcher switcher;
	public int startID;
	public int quitID;

	private bool _switching = false;

	public void Exit() {
		Application.Quit();
	}

	public void StartGame() {
		if(_switching) return;

		_switching = true;
		switcher.StartSwitch(startID);
	}

	public void QuitGame() {
		if(_switching) return;

		_switching = true;
		switcher.StartSwitch(quitID);
	}
}
