using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class InputSelector : MonoBehaviour
{
    public TMP_Dropdown micDropdown;
    public string selectedMic;

	private void Start()
	{
		PopulateMicDropdown();
	}

	private void PopulateMicDropdown()
	{
		if (micDropdown == null) return;
		micDropdown.ClearOptions();
		string[] connectedMics = Microphone.devices;

		if (connectedMics.Length == 0)
		{
			micDropdown.AddOptions(new List<string> { "No input available!" });
			micDropdown.interactable = false;
			return;
		}

		List<string> options = new List<string>();
		foreach (string mic in connectedMics)
		{
			options.Add(mic);
		}

		micDropdown.AddOptions(options);
		micDropdown.value = 0;
		micDropdown.RefreshShownValue();
		selectedMic = connectedMics[0];
		micDropdown.onValueChanged.AddListener(OnInputValueChanged);
	}

	private void OnInputValueChanged(int index)
	{
		selectedMic = Microphone.devices[index];
		VoiceSensitivityFilter.Instance.microphoneClip = Microphone.Start(selectedMic, true, 1, 44100);
		Debug.Log("Selected mic: " + selectedMic);
	}

	private void OnDestroy()
	{
		if (micDropdown != null)
		{
			micDropdown.onValueChanged.RemoveListener(OnInputValueChanged);
		}
	}
}
