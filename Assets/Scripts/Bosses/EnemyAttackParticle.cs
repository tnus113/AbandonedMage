using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackParticle : MonoBehaviour
{
	private ParticleSystem particle;

	private void Start()
	{
		particle = GetComponent<ParticleSystem>();
	}

	private void OnParticleCollision(GameObject other)
	{
		HealthSystem playerHealth = other.GetComponentInParent<HealthSystem>();
		if (playerHealth != null && playerHealth.CompareTag("Player"))
		{
			playerHealth.TakeDamage(10);
		}
	}
}
