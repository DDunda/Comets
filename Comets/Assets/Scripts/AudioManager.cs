using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {
	public AudioClip clip;
	[Range(0f, 1f)]
	public float volume = 1f;
	public float pitch = 1f;

	public Sound(AudioClip clip, float volume, float pitch) {
		this.clip = clip;
		this.volume = volume;
		this.pitch = pitch;
	}
}

[System.Serializable]
public class Song {
	public AudioClip clip;
	public string name;
	[Range(0f, 1f)]
	public float volume = 1f;
	public float pitch = 1f;
}

public class AudioManager : MonoBehaviour {
	private static List<AudioSource> sources = new List<AudioSource>();
	private static List<Song> _songs = new List<Song>();
	private static AudioSource musicSource;

	private static Song currentSong = null;
	private static Song nextSong = null;

	public enum MusicState {
		NotPlaying,
		Playing,
		Starting,
		Ending
	};

	private static AudioManager manager = null;
	private static MusicState _state = MusicState.NotPlaying;
	private static float _transitionTime;

	public Song[] songs;
	public string defaultSong;
	public float transitionTime;

	public MusicState state { get => _state; }

	public static void PlaySound(Sound sound, GameObject obj) {
		AudioSource src = obj.AddComponent<AudioSource>();
		src.clip = sound.clip;
		src.pitch = Mathf.Max(0.01f, sound.pitch);
		src.volume = Mathf.Clamp01(sound.volume);
		src.Play();

		sources.Add(src);
	}

	public static void PlaySound(Sound sound) {
		PlaySound(sound, manager.gameObject);
	}

	private static void SetSong(Song s) {
		currentSong = s;

		musicSource.clip = s.clip;
		musicSource.volume = 0;
		musicSource.pitch = s.pitch;
		musicSource.Play();

		_state = MusicState.Starting;
	}

	public static bool StopSong() {
		if (_state == MusicState.NotPlaying || _state == MusicState.Ending) {
			return false;
		} else {
			_state = MusicState.Ending;
			return true;
		}
	}

	public static bool StartSong(string name) {
		Song s = _songs.Find(s => s.name == name);
		if(s == null)
			return false;

		switch (_state)
		{
			case MusicState.NotPlaying:
				SetSong(s);
				return true;

			case MusicState.Ending:
				if(s == currentSong) {
					_state = MusicState.Starting;
				} else {
					nextSong = s;
				}
				return true;

			case MusicState.Starting:
			case MusicState.Playing:
				if(s == currentSong) {
					return false;
				} else {
					_state = MusicState.Ending;
					nextSong = s;
					return true;
				}

			default:
				return false;
		}
	}

	void Start() {
		if(manager != null) {
			Destroy(this);
			return;
		}

		manager = this;
		DontDestroyOnLoad(gameObject);

		_songs.AddRange(songs);
		_transitionTime = transitionTime;

		musicSource = gameObject.AddComponent<AudioSource>();
		musicSource.loop = true;

		StartSong(defaultSong);
	}

	void Update() {
		List<AudioSource> toRemove = sources.FindAll(src => src == null || !src.isPlaying);
		foreach (var src in toRemove)
		{
			sources.Remove(src);
			if (src != null)
			{
				Destroy(src);
			}
		}

		switch (_state)
		{
			case MusicState.NotPlaying:
			case MusicState.Playing:
				break;

			case MusicState.Starting:
				musicSource.volume += currentSong.volume / _transitionTime;
				if(musicSource.volume >= currentSong.volume) {
					musicSource.volume = currentSong.volume;
					_state = MusicState.Playing;
				}
				break;

			case MusicState.Ending:
				musicSource.volume -= currentSong.volume / _transitionTime;
				if(musicSource.volume <= 0) {
					musicSource.volume = 0;
					musicSource.Stop();

					if(nextSong != null) {
						SetSong(nextSong);
						nextSong = null;
						_state = MusicState.Starting;
					} else {
						currentSong = null;
						_state = MusicState.NotPlaying;
					}
				}
				break;
		}
	}
}