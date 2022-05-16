using UnityEngine;

[System.Serializable]
public class Powerup : ScriptableObject {
	public string description;
	public GameObject UIPrefab;
	
	[System.NonSerialized]
	protected GameObject UIElement;

	public virtual void OnDestroy() {
		Destroy(UIElement);
	}

	public virtual void SetUI(GameObject UIElement) {
		this.UIElement = UIElement;
	}

	public virtual bool isReady { get => true; }
	public virtual bool isDepleted { get => false; }

	public virtual void OnCollect(IResourceAdder collector) {}
	public virtual void OnActivate(GameObject parent) {}
}