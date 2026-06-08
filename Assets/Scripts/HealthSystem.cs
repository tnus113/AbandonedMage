using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider healthBar;

    public event Action<float> OnHealthChanged;
    public static event Action OnBossDefeated;

    public bool isDead { get; private set; } = false;
    public bool isInvincible = false;

	private Animator animator;

	private void Start()
	{
        animator = GetComponent<Animator>();
		currentHealth = maxHealth;
        UpdateUI();
	}

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        if (isInvincible) return;
		currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
		if (gameObject.tag == "Enemy")
		{
            if (gameObject.GetComponent<Flamethrower>().flamethrower.isPlaying == false)
            { 
                animator?.SetTrigger("damage");
			}
		}
		else
		{
			animator?.SetTrigger("damage");
		}
		OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
			if (gameObject.tag == "Player")
			{
				FindObjectOfType<DeathScreen>().ShowDeathScreen();
			}
			Die();
		}
	}

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
        OnHealthChanged?.Invoke(currentHealth);
	}

    public void Die()
    {
        isDead = true;
		animator?.SetTrigger("die");
        GetComponent<Collider>().enabled = false;

        if (gameObject.CompareTag("Enemy") && GetComponent<Flamethrower>() != null)
        {
            UnlockFireballForPlayer();
            Destroy(gameObject);
            OnBossDefeated?.Invoke();
        }
	}

    private void UnlockFireballForPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerActionHandler actionHandler = playerObj.GetComponent<PlayerActionHandler>();
            PlayerMovement playerMovement = playerObj.GetComponent<PlayerMovement>();

            if (actionHandler != null)
            {
                actionHandler.UnlockSkill("Fireball");
            }

            if (playerMovement != null && playerMovement.DialogueUI != null)
            {
                DialogueObject dialogue = ScriptableObject.CreateInstance<DialogueObject>();
                dialogue.Initialize(new string[] { 
                    "Congratulations! You have defeated the Fire Boss and absorbed its flames.",
                    "<< New skill learned: Fireball! >>",
                    "<< Say 'Fireball' or 'Fire' to cast this skill. >>"
                });
                playerMovement.DialogueUI.ShowDialogue(dialogue);
            }
        }
    }

    private void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
	}

    public float GetCurrentHealth()
    {
        return currentHealth;
	}
}
