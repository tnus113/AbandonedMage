using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public AudioMixer audioMixer;

	public Slider musicSlider;
	public Slider sfxSlider;

	private void Start()
	{
		LoadVolume();
		MusicManager.Instance.PlayMusic("Menu");
	}

	public void Play(string sceneName, string transitionType, string musicTrack)
    {
		LevelManager.Instance.LoadScene(sceneName, transitionType);
		MusicManager.Instance.PlayMusic(musicTrack);
	}

    public void Quit()
    {
        Application.Quit();
	}

	public void UpdateMusicVolume(float volume)
	{
		audioMixer.SetFloat("MusicVolume", volume);
	}

	public void UpdateSoundVolume(float volume)
	{
		audioMixer.SetFloat("SFXVolume", volume);
	}

	public void SaveVolume()
	{
		audioMixer.GetFloat("MusicVolume", out float musicVolume);
		PlayerPrefs.SetFloat("MusicVolume", musicVolume);

		audioMixer.GetFloat("SFXVolume", out float sfxVolume);
		PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
	}

	public void LoadVolume()
	{
		musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
		sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
	}

	public void OnContinueClicked()
	{
		DataPersistenceManager.instance.LoadGame();
		string lastScene = DataPersistenceManager.instance.GetSavedSceneName();
		string musicTrack = (lastScene == "CutScene") ? "CutScene" : "Game";
		
		if (lastScene != "GameScene" && lastScene != "CutScene")
		{
			LevelManager.Instance.LoadSceneChain("GameScene", lastScene, "CrossFade");
			MusicManager.Instance.PlayMusic(musicTrack);
		}
		else
		{
			Play(lastScene, "CrossFade", musicTrack);
		}
	}

	public void OnNewGameClicked()
	{
		DataPersistenceManager.instance.NewGame();
		DataPersistenceManager.instance.SaveGame();
		Play("CutScene", "CrossFade", "CutScene");
	}
}
