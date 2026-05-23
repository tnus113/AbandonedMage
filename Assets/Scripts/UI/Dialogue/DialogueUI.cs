using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class DialogueUI : MonoBehaviour
{
	[SerializeField] private GameObject dialogueBox;
	[SerializeField] private TMP_Text textLabel;

	public bool isOpen { get; private set; }

	private ResponseHandler responseHandler;
	private TypewriterEffect typewriterEffect;
	private GameObject currentSpeaker;

	private void Start()
	{
		typewriterEffect = GetComponent<TypewriterEffect>();
		responseHandler = GetComponent<ResponseHandler>();

		CloseDialogueBox();
	}

	public void ShowDialogue(DialogueObject dialogueObject, GameObject speaker = null)
	{
		if (speaker != null)
		{
			currentSpeaker = speaker;
		}
		isOpen = true;
		dialogueBox.SetActive(true);
		StartCoroutine(StepThroughDialogue(dialogueObject));
	}

	public void AddResponseEvents(ResponseEvent[] responseEvents)
	{
		responseHandler.AddResponseEvents(responseEvents);
	}

	private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
	{
		for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
		{
			string dialogue = dialogueObject.Dialogue[i];

			yield return RunTypingEffect(dialogue);

			textLabel.text = dialogue;

			if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
			{
				break;
			}

			yield return null;
			yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
		}

		if (dialogueObject.HasResponses)
		{
			if (currentSpeaker != null)
			{
				bool foundMatchingEvent = false;
				foreach (DialogueResponseEvents responseEvents in currentSpeaker.GetComponents<DialogueResponseEvents>())
				{
					if (responseEvents.DialogueObject == dialogueObject)
					{
						AddResponseEvents(responseEvents.Events);
						foundMatchingEvent = true;
						break;
					}
				}
				if (!foundMatchingEvent)
				{
					AddResponseEvents(null);
				}
			}
			responseHandler.ShowResponses(dialogueObject.Responses);
		}
		else
		{
			CloseDialogueBox();
		}
	}

	private IEnumerator RunTypingEffect(string dialogue)
	{
		typewriterEffect.Run(dialogue, textLabel);
		while (typewriterEffect.IsRunning)
		{
			yield return null;
			if (Input.GetMouseButtonDown(0))
			{
				typewriterEffect.Stop();
			}
		}
	}

	public void CloseDialogueBox()
	{
		isOpen = false;
		dialogueBox.SetActive(false);
		textLabel.text = string.Empty;
		currentSpeaker = null;
	}
}
