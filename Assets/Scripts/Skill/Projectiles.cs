using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class Projectiles : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;

	public GameObject explosionEffect;

    private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * speed;
		Destroy(gameObject, lifetime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			float damageAmount = 20f;
			HealthSystem enemyHealth = other.GetComponent<HealthSystem>();
			if (enemyHealth != null)
			{
				enemyHealth.TakeDamage(damageAmount);
			}
			if (explosionEffect != null)
			{
				Vector3 hitPoint = other.ClosestPoint(transform.position);
				Instantiate(explosionEffect, hitPoint, Quaternion.identity);
			}
			SoundManager.Instance.PlaySound3D("Explosion", transform.position);
			Destroy(gameObject);
		}
	}
}
