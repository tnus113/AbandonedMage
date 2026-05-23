using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 3f;
    public float delayBeforeFade = 1.5f;

	private bool canRestart = false;

	private void Start()
	{
		if (canvasGroup != null)
		{
			canvasGroup.alpha = 0f;
			canvasGroup.gameObject.SetActive(false);
		}
		Time.timeScale = 1f;
	}

	private void Update()
	{
		if (canRestart && Input.GetMouseButtonDown(0))
		{
			RestartLevel();
		}
	}

	public void ShowDeathScreen()
	{
		if (canvasGroup != null)
		{
			canvasGroup.gameObject.SetActive(true);
			StartCoroutine(FadeInCoroutine());
		}
	}

	private IEnumerator FadeInCoroutine()
	{
		yield return new WaitForSeconds(delayBeforeFade);
		float elapsedTime = 0f;
		while (elapsedTime < fadeDuration)
		{
			elapsedTime += Time.deltaTime;
			canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
			yield return null;
		}
		canvasGroup.alpha = 1f;
		canRestart = true;
	}

	public void RestartLevel()
	{
		Time.timeScale = 1f;
		LevelManager.Instance.LoadScene(SceneManager.GetActiveScene().name, "CrossFade");
	}
}
