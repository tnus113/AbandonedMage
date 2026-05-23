using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechToCommandMapper : MonoBehaviour
{
    public static SpeechToCommandMapper Instance { get; private set; }

    private Dictionary<string, PlayerCommand> commandMap;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		InitializeMapping();
	}

	private void InitializeMapping()
	{
		commandMap = new Dictionary<string, PlayerCommand>();

		commandMap.Add("attack", PlayerCommand.Attack);

		commandMap.Add("fireball", PlayerCommand.Fireball);
		commandMap.Add("fire", PlayerCommand.Fireball);

		commandMap.Add("shield", PlayerCommand.WindShield);
		commandMap.Add("windshield", PlayerCommand.WindShield);
	}

	//nhan dien giong noi tra ve lenh tuong ung, neu ko co lenh tra ve none
	public PlayerCommand GetCommandFromSpeech(string spokenWord)
	{
		spokenWord = spokenWord.ToLower().Trim();
		if (commandMap.TryGetValue(spokenWord, out PlayerCommand mappedCommand))
		{
			return mappedCommand;
		}
		else
		{
			return PlayerCommand.None;
		}
	}

	//tra ve tat ca cac tu khoa co the nhan dien duoc
	public string[] GetAllKeywords()
	{
		string[] keywordsArray = new string[commandMap.Keys.Count];
		commandMap.Keys.CopyTo(keywordsArray, 0);
		return keywordsArray;	
	}
}
