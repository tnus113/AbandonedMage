using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

	[SerializeField] private AudioSource musicSource;
	[SerializeField] private MusicLibrary musicLibrary;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void PlayMusic(string trackName, float fadeDuration = 0.5f)
	{
		StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
	}

	IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
	{
		float percent = 0f;
		while (percent < 1f)
		{
			percent += Time.deltaTime * 1/ fadeDuration;
			musicSource.volume = Mathf.Lerp(0.5f, 0f, percent);
			yield return null;
		}

		musicSource.clip = nextTrack;
		musicSource.Play();

		percent = 0f;
		while (percent < 1f)
		{
			percent += Time.deltaTime * 1/ fadeDuration;
			musicSource.volume = Mathf.Lerp(0f, 0.5f, percent);
			yield return null;
		}
	}
}
