using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthUI : MonoBehaviour
{
    private HealthSystem healthSystem;
    private Transform player;
    private float displayRange = 50f;

	private void Start()
	{
		healthSystem = GetComponent<HealthSystem>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update()
	{
		float distance = Vector3.Distance(player.position, healthSystem.transform.position);
		if (distance < displayRange)
		{
			healthSystem.healthBar.gameObject.SetActive(true);
		}
		else
		{
			healthSystem.healthBar.gameObject.SetActive(false);
		}
	}
}
