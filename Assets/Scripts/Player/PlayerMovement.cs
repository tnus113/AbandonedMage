using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
	[SerializeField] DialogueUI dialogueUI;

	public DialogueUI DialogueUI => dialogueUI;
	public IInteractable Interactable { get; set; }

	public float speed = 5f;
	public float rotationSpeed = 10f;

	public float dashSpeed;
	public float dashDuration;
	public float dashCooldown;
	private float dashCooldownTimer;
	public GameObject dashEffect;

	public float gravity = -9.81f;
	private Vector3 velocity;

	private CharacterController characterController;
	private Animator animator;

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}

	public void LoadData(GameData data)
	{
		if (characterController != null) characterController.enabled = false;
		this.transform.position = data.playerPosition;
		Physics.SyncTransforms();
		if (characterController != null) characterController.enabled = true;
	}

	public void SaveData(ref GameData data)
	{
		data.playerPosition = this.transform.position;
	}

	void Update()
	{
		if (dialogueUI.isOpen)
		{
			return;
		}
		if (dashCooldownTimer > 0)
		{
			dashCooldownTimer -= Time.deltaTime;
			if (dashCooldownTimer < 0)
			{
				dashCooldownTimer = 0;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0)
		{
			dashEffect.SetActive(true);
			dashCooldownTimer = dashCooldown;
			SoundManager.Instance.PlaySound3D("Dash", transform.position);
			StartCoroutine(DashRoutine());
		}

		if (characterController.isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		float moveX = Input.GetAxisRaw("Horizontal");
		float moveZ = Input.GetAxisRaw("Vertical");

		Vector3 direction = new Vector3(moveX, 0f, moveZ).normalized;

		bool isRunning = direction.magnitude >= 0.1f;
		if (isRunning)
		{
			Quaternion targetRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}

		characterController.Move(direction * speed * Time.deltaTime);
		animator.SetBool("isRunning", isRunning);

		velocity.y += gravity * Time.deltaTime;
		characterController.Move(velocity * Time.deltaTime);

		//Dialogue test
		if (Input.GetKeyDown(KeyCode.E))
		{
			Interactable?.Interact(this);
		}
	}

	private IEnumerator DashRoutine()
	{
		float startTime = Time.time;
		while (Time.time < startTime + dashDuration)
		{
			characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
			gameObject.GetComponent<CharacterController>().detectCollisions = false;
			yield return null;
		}
		dashEffect.SetActive(false);
		gameObject.GetComponent<CharacterController>().detectCollisions = true;
	}
}
