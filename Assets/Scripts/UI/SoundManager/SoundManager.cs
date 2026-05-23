using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

	[SerializeField] private SoundLibrary soundLibrary;
	[SerializeField] private AudioSource sfx2DSource;

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

	public void PlaySound3D(AudioClip clip, Vector3 pos)
	{
		if (clip != null)
		{
			AudioSource.PlayClipAtPoint(clip, pos);
		}
	}

	public void PlaySound3D(string clipName, Vector3 pos)
	{
		PlaySound3D(soundLibrary.GetClipFromName(clipName), pos);
	}

	public void PlaySound2D(string soundName)
	{
		sfx2DSource.PlayOneShot(soundLibrary.GetClipFromName(soundName));
	}
}
