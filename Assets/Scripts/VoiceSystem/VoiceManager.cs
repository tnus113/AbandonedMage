using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager Instance { get; private set; }

    public event Action<PlayerCommand> OnCommandRecognized;

    private KeywordRecognizer keywordRecognizer;

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
            return;
		}
	}

	private void Start()
	{
		if (SpeechToCommandMapper.Instance == null)
		{
            Debug.Log("chua co mapper");
            return;
		}
        string[] autoKeywords = SpeechToCommandMapper.Instance.GetAllKeywords();
		if (autoKeywords != null && autoKeywords.Length > 0)
		{
            keywordRecognizer = new KeywordRecognizer(autoKeywords);
            keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
            keywordRecognizer.Start();
            Debug.Log("da tao recognizer voi " + autoKeywords.Length + " tu khoa");
		}
	}

    private void RecognizedSpeech(PhraseRecognizedEventArgs args)
    {
        string spokenWord = args.text;
		if (VoiceSensitivityFilter.Instance.isLoudEnough)
		{
            Debug.Log("noi qua nho!");
            return;
		}
		PlayerCommand command = SpeechToCommandMapper.Instance.GetCommandFromSpeech(spokenWord);
        if (command != PlayerCommand.None)
        {
            Debug.Log("da nhan dien duoc lenh: " + command);
            OnCommandRecognized?.Invoke(command);
        }
	}
    private void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.OnPhraseRecognized -= RecognizedSpeech;
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
	}
}
