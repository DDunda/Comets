using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
	[System.Serializable]
	public class SceneSwitch {
		public string switchNickname;
		public string animationName;
		public UnityEvent switchEvent;
	}

	public Animation fade;
	public SceneSwitch[] switchEvents;

	private bool _switching = false;
	private SceneSwitch selectedEvent = null;

	public bool switching { get => _switching; }

	public bool StartSwitch(int i) {
		if(i >= switchEvents.Length)
			return false;

		if(_switching)
			return false;

		selectedEvent = switchEvents[i];
		fade.Play(selectedEvent.animationName);

		return true;
	}

    public void AnimationEnded()
    {
		_switching = false;

		if(selectedEvent == null)
			return;

		selectedEvent.switchEvent.Invoke();
		selectedEvent = null;
	}

	public static void SetScene(int i) { 
		SceneManager.LoadScene(i);
	}

	public static void SetScene(string name) { 
		SceneManager.LoadScene(name);
	}
}