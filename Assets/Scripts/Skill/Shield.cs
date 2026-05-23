using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float duration = 5f;

    private HealthSystem healthSystem;

	private void Start()
	{
		healthSystem = GetComponentInParent<HealthSystem>();
		if (healthSystem != null)
		{
			healthSystem.isInvincible = true;
		}
		Destroy(gameObject, duration);
	}

	private void OnDestroy()
	{
		if (healthSystem != null)
		{
			healthSystem.isInvincible = false;
		}
	}
}
