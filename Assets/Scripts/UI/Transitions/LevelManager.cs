using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

	public Slider progressBar;
	public GameObject transitionContainer;

	private SceneTransition[] transitions;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		transitions = transitionContainer.GetComponentsInChildren<SceneTransition>();
	}

	public void LoadScene(string sceneName, string transitionName)
	{
		StartCoroutine(LoadSceneAsync(sceneName, transitionName));
	}

	private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
	{
		SceneTransition transition = transitions.First(s => s.name == transitionName);

		AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
		scene.allowSceneActivation = false;

		yield return transition.AnimateTransitionIn();

		progressBar.gameObject.SetActive(true);

		do
		{
			progressBar.value = scene.progress;
			yield return null;
		}
		while (scene.progress < 0.9f);

		scene.allowSceneActivation = true;
		progressBar.gameObject.SetActive(false);

		yield return transition.AnimateTransitionOut();
	}

	public void LoadSceneChain(string intermediateScene, string finalScene, string transitionName)
	{
		StartCoroutine(LoadSceneChainAsync(intermediateScene, finalScene, transitionName));
	}

	private IEnumerator LoadSceneChainAsync(string intermediateScene, string finalScene, string transitionName)
	{
		SceneTransition transition = transitions.First(s => s.name == transitionName);

		yield return transition.AnimateTransitionIn();

		progressBar.gameObject.SetActive(true);

		AsyncOperation scene1 = SceneManager.LoadSceneAsync(intermediateScene);
		scene1.allowSceneActivation = false;

		do
		{
			progressBar.value = scene1.progress * 0.5f;
			yield return null;
		}
		while (scene1.progress < 0.9f);

		scene1.allowSceneActivation = true;

		while (SceneManager.GetActiveScene().name != intermediateScene)
		{
			yield return null;
		}

		AsyncOperation scene2 = SceneManager.LoadSceneAsync(finalScene);
		scene2.allowSceneActivation = false;

		do
		{
			progressBar.value = 0.5f + (scene2.progress * 0.5f);
			yield return null;
		}
		while (scene2.progress < 0.9f);

		scene2.allowSceneActivation = true;
		progressBar.gameObject.SetActive(false);

		yield return transition.AnimateTransitionOut();
	}
}
