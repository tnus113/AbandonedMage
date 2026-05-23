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
