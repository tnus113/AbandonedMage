using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackParticle : MonoBehaviour
{
	private ParticleSystem particle;
	private HealthSystem healthSystem;

	private void Start()
	{
		particle = GetComponent<ParticleSystem>();
	}

	private void OnParticleCollision(GameObject other)
	{
		if (other.GetComponent<Collider>().CompareTag("Player"))
		{
			healthSystem = other.GetComponent<HealthSystem>();
			healthSystem?.TakeDamage(10);
		}
	}
}
