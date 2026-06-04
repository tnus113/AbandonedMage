using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;

	public string[] Dialogue => dialogue;

    public bool HasResponses => Responses != null && Responses.Length > 0;

	public Response[] Responses => responses;

	public void Initialize(string[] dialogueLines, Response[] responsesList = null)
	{
		this.dialogue = dialogueLines;
		this.responses = responsesList;
	}
}
