using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;

public class SkipCutScene : MonoBehaviour
{
	public string sceneName;
	public string transitionType = "CrossFade";
	public string musicTrack;
	public void Skip()
    {
		DataPersistenceManager.instance.LoadGame();
		LevelManager.Instance.LoadScene(sceneName, transitionType);
		MusicManager.Instance.PlayMusic(musicTrack);
	}
}