using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
	public GameObject pauseMenuButtons;
	public GameObject optionsMenuUI;

	public AudioMixer audioMixer;
	public Slider musicSlider;
	public Slider sfxSlider;

	private void Start()
	{
		LoadVolume();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (optionsMenuUI.activeSelf)
			{
				CloseOptions();
			}
			else if (gameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		optionsMenuUI.SetActive(false);
		Time.timeScale = 1f;
		gameIsPaused = false;
	}

	public void Pause()
	{
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		gameIsPaused = true;
	}

	public void OpenOptions()
	{
		pauseMenuButtons.SetActive(false);
		optionsMenuUI.SetActive(true);
	}

	public void CloseOptions()
	{
		SaveVolume();
		optionsMenuUI.SetActive(false);
		pauseMenuButtons.SetActive(true);
	}

	public void UpdateMusicVolume(float volume)
	{
		audioMixer.SetFloat("MusicVolume", volume);
	}

	public void UpdateSFXVolume(float volume)
	{
		audioMixer.SetFloat("SFXVolume", volume);
	}

	private void SaveVolume()
	{
		audioMixer.GetFloat("MusicVolume", out float musicVolume);
		audioMixer.GetFloat("SFXVolume", out float sfxVolume);
		PlayerPrefs.SetFloat("MusicVolume", musicVolume);
		PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
	}

	public void LoadVolume()
	{
		if (PlayerPrefs.HasKey("MusicVolume"))
		{
			musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
			sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
		}
	}

	public void LoadMenu()
	{
		Time.timeScale = 1f;
		gameIsPaused = false;
		LevelManager.Instance.LoadScene("MenuScene", "CrossFade");
		MusicManager.Instance.PlayMusic("Menu");
	}
}
