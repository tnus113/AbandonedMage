using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    private HealthSystem healthSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healthSystem = other.GetComponent<HealthSystem>();
            healthSystem?.TakeDamage(10);
        }
    }
}
