using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VoiceUIManager : MonoBehaviour
{
    public TextMeshProUGUI voiceText;
    public float displayTime = 2f;

	private void Start()
	{
		if (voiceText != null)
		{
			voiceText.text = "...";
		}

		if (VoiceManager.Instance != null)
		{
			VoiceManager.Instance.OnCommandRecognized += ShowRecognizedWord;
		}
	}

	private void ShowRecognizedWord(PlayerCommand command)
	{
		if (voiceText == null) return;
		voiceText.text = command.ToString().ToLower();
		StopAllCoroutines();
		StartCoroutine(ClearTextAfterDelay());
	}

	private IEnumerator ClearTextAfterDelay()
	{
		yield return new WaitForSeconds(displayTime);
		if (voiceText != null)
		{
			voiceText.text = "...";
		}
	}

	private void OnDestroy()
	{
		if (VoiceManager.Instance != null)
		{
			VoiceManager.Instance.OnCommandRecognized -= ShowRecognizedWord;
		}
	}
}
