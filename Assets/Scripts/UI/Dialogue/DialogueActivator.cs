using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private Vector3 promptOffset = new Vector3(0f, 2.2f, 0f);

    private PlayerMovement cachedPlayer;
    private GameObject promptInstance;

	public void UpdateDialogueObject(DialogueObject dialogueObject)
	{
		this.dialogueObject = dialogueObject;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
		{
			player.Interactable = this;
			cachedPlayer = player;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
		{
			if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
			{
				player.Interactable = null;
			}
			cachedPlayer = null;
		}
	}

	public void Interact(PlayerMovement player)
    {
		player.DialogueUI.ShowDialogue(dialogueObject, this.gameObject);
    }

	private void ShowPrompt()
	{
		if (promptInstance == null)
		{
			promptInstance = new GameObject("InteractPrompt");
			promptInstance.transform.SetParent(this.transform);
			promptInstance.transform.localPosition = promptOffset;

			TextMesh textMesh = promptInstance.AddComponent<TextMesh>();
			textMesh.text = "Press E to interact";
			textMesh.fontSize = 32;
			textMesh.characterSize = 0.08f;
			textMesh.alignment = TextAlignment.Center;
			textMesh.anchor = TextAnchor.MiddleCenter;
			textMesh.color = Color.white;
		}
		promptInstance.SetActive(true);
	}

	private void HidePrompt()
	{
		if (promptInstance != null)
		{
			promptInstance.SetActive(false);
		}
	}

	private void Update()
	{
		bool isPlayerNear = cachedPlayer != null && cachedPlayer.Interactable as DialogueActivator == this;
		bool isDialogueOpen = cachedPlayer != null && cachedPlayer.DialogueUI != null && cachedPlayer.DialogueUI.isOpen;

		if (isPlayerNear && !isDialogueOpen)
		{
			ShowPrompt();
		}
		else
		{
			HidePrompt();
		}

		if (promptInstance != null && promptInstance.activeSelf)
		{
			Camera mainCam = Camera.main;
			if (mainCam != null)
			{
				promptInstance.transform.LookAt(promptInstance.transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
			}
		}
	}

	private void OnDestroy()
	{
		if (promptInstance != null)
		{
			Destroy(promptInstance);
		}
	}
}
