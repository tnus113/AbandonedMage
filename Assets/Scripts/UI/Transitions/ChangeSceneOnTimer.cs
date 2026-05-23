using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneOnTimer : MonoBehaviour
{
    public float changeTimer;
    public string sceneName;
	public string transitionType = "CrossFade";

	private bool hasTriggered = false;

	private void Update()
	{
		if (hasTriggered) return;
		changeTimer -= Time.deltaTime;
		if (changeTimer <= 0)
		{
			hasTriggered = true;
			LevelManager.Instance.LoadScene(sceneName, transitionType);
		}
	}
}
