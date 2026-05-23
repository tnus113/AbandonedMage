using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceSensitivityFilter : MonoBehaviour
{
    public static VoiceSensitivityFilter Instance { get; private set; }

    public float sensitivityThreshold = 0.5f;
    public int sampleWindow = 128;
    public float currentLoudness = 0f;
    public bool isLoudEnough = false;

    public AudioClip microphoneClip;
    private string micName;

	private void Awake()
	{
		if(Instance == null)
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
		if (Microphone.devices.Length > 0)
		{
            micName = Microphone.devices[0];
            microphoneClip = Microphone.Start(micName, true, 1, 44100);
            Debug.Log("da ket noi mic:" + micName);
		}
        else
        {
            Debug.LogError("khong tim thay micro nao!");
		}
	}

	private void Update()
	{
		if (microphoneClip != null)
		{
            currentLoudness = GetLoudnessFromMicrophone();
            isLoudEnough = currentLoudness > sensitivityThreshold;
		}
	}

    public float GetLoudnessFromMicrophone()
    {
        int currentMicPosition = Microphone.GetPosition(micName);
        int startPosition = currentMicPosition - sampleWindow;
        if (startPosition < 0)
        {
            return 0f;
		}
        float[] wavesData = new float[sampleWindow];
        microphoneClip.GetData(wavesData, startPosition);

        float totalLoudness = 0f;
        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(wavesData[i]);
		}
        return totalLoudness / sampleWindow;
	}
}
