using UnityEngine;

public class SoundDeleter : MonoBehaviour 
{
	private AudioSource[] sources;

	void Start()
	{
		sources = gameObject.GetComponents<AudioSource>();
		foreach (var src in sources)
		{
			if(src.pitch < 0) src.timeSamples = src.clip.samples - 1;
		}
	}

	void Update()
	{
		foreach (var src in sources)
		{
			//if(src.timeSamples != src.clip.samples)
			if(src.isPlaying)
				return;
		}

		Destroy(gameObject);
	}
}