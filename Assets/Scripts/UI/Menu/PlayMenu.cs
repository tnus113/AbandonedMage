using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
	public Button newGameButton;
	public Button continueButton;
	public GameObject warningPanel;
	public MainMenu mainMenu;
	private void OnEnable()
	{
		continueButton.interactable = false;
		if (DataPersistenceManager.instance != null && DataPersistenceManager.instance.HasGameData())
		{
			continueButton.interactable = true;
			newGameButton.onClick.RemoveAllListeners();
			newGameButton.onClick.AddListener(() =>
			{
				warningPanel.SetActive(true);
			});
		}
		else
		{
			newGameButton.onClick.RemoveAllListeners();
			newGameButton.onClick.AddListener(() =>
			{
				mainMenu.OnNewGameClicked();
			});
		}
	}
}
