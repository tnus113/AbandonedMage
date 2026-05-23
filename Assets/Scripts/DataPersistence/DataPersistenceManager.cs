using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
	[Header("File Storage Config")]
	[SerializeField] private string fileName;

	public static DataPersistenceManager instance { get; private set; }

	private GameData gameData;
	private List<IDataPersistence> dataPersistenceObjects;
	private FileDataHandler dataHandler;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Multiple instances of DataPersistenceManager found! Destroying the new one.");
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
		this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.dataPersistenceObjects = FindAllDataPersistenceObjects();
		LoadGame();
	}

	public void NewGame()
	{
		this.gameData = new GameData();
	}

	public void LoadGame()
	{
		this.gameData = dataHandler.Load();

		if (this.gameData == null)
		{
			Debug.LogWarning("No game data found. Starting a new game.");
			NewGame();
		}
		foreach (IDataPersistence dataPersistenceObj  in dataPersistenceObjects)
		{
			dataPersistenceObj.LoadData(gameData);
		}
	}

	public void SaveGame()
	{
		foreach (IDataPersistence dataPersistenceObj  in dataPersistenceObjects)
		{
			dataPersistenceObj.SaveData(ref gameData);
		}
		Debug.Log("Saving game data to: " + Application.persistentDataPath + "/" + fileName);

		dataHandler.Save(gameData);
	}

	private void OnApplicationQuit()
	{
		SaveGame();
	}

	private List<IDataPersistence> FindAllDataPersistenceObjects()
	{
		IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
		return new List<IDataPersistence>(dataPersistenceObjects);
	}

	public bool HasGameData()
	{
		return dataHandler != null && dataHandler.HasGameData();
	}
}
